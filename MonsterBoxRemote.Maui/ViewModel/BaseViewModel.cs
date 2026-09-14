using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Meadow.Foundation.Web.Maple;

namespace MonsterBoxRemote.Maui.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged, IDisposable
    {
        private const int MapleClientListenTimeout = 10000;

        // Addresses added via AddManualDeviceCommand, so GetServers() can leave
        // them in HostList across rescans instead of wiping them out along with
        // the auto-discovered entries.
        private readonly HashSet<string> _manualDeviceAddresses = new(StringComparer.OrdinalIgnoreCase);

        public MapleClient client { get; private set; }

        int _serverPort;
        public int ServerPort
        {
            get => _serverPort;
            set { _serverPort = value; OnPropertyChanged(nameof(ServerPort)); }
        }

        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(nameof(IsBusy)); }
        }

        string? _manualDeviceAddress;
        public string? ManualDeviceAddress
        {
            get => _manualDeviceAddress;
            set { _manualDeviceAddress = value; OnPropertyChanged(nameof(ManualDeviceAddress)); }
        }

        public ObservableCollection<ServerModel> HostList { get; set; }

        public Command AddManualDeviceCommand { get; }

        public BaseViewModel()
        {
            HostList = new ObservableCollection<ServerModel>();

            //HostList.Add(new ServerModel() { Name="Meadow (192.168.1.73)", IpAddress="192.168.1.73" });
            //HostList.Add(new ServerModel() { Name = "Meadow (192.168.1.74)", IpAddress = "192.168.1.74" });

            ServerPort = 5417;

            // MapleClient.ListenTimeout has a protected setter as of Maple.Client 0.96.0,
            // so the timeout must be supplied via the constructor instead of an initializer.
            client = new MapleClient(listenTimeout: TimeSpan.FromMilliseconds(MapleClientListenTimeout));
            client.Servers.CollectionChanged += ServersCollectionChanged;

            AddManualDeviceCommand = new Command(AddManualDevice);
        }

        private void AddManualDevice()
        {
            var address = ManualDeviceAddress?.Trim();
            if (string.IsNullOrEmpty(address) || Uri.CheckHostName(address) == UriHostNameType.Unknown)
            {
                Debug.WriteLine($"Ignoring invalid manual device address: '{address}'");
                return;
            }

            if (!HostList.Any(server => string.Equals(server.IpAddress, address, StringComparison.OrdinalIgnoreCase)))
            {
                HostList.Add(new ServerModel { Name = $"Manual ({address})", IpAddress = address });
            }

            _manualDeviceAddresses.Add(address);
            ManualDeviceAddress = string.Empty;
        }

        public async Task GetServers()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            try
            {
                // Only the auto-discovered entries are cleared, so manually-added
                // devices (AddManualDeviceCommand) survive repeat scans - e.g.
                // every time the Controller tab reappears - instead of
                // disappearing along with everything else.
                for (int i = HostList.Count - 1; i >= 0; i--)
                {
                    if (!_manualDeviceAddresses.Contains(HostList[i].IpAddress))
                    {
                        HostList.RemoveAt(i);
                    }
                }

                await client.StartScanningForAdvertisingServers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                await MainThread.InvokeOnMainThreadAsync(() => IsBusy = false);
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private void ServersCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add when e.NewItems != null:
                    foreach (ServerModel server in e.NewItems)
                    {
                        Debug.WriteLine($"'{server.Name}' @ ip:[{server.IpAddress}]");

                        var discovered = new ServerModel { Name = $"{server.Name} ({server.IpAddress})", IpAddress = server.IpAddress };

                        // MapleClient raises this from its UDP listener thread, not the UI
                        // thread. Android/iOS tolerate an off-thread ObservableCollection
                        // mutation; WinUI3 hard-crashes on it (native 0xc000027b in
                        // Microsoft.UI.Xaml.dll), so this must be marshaled back to the
                        // main thread before touching HostList - the duplicate check has
                        // to happen there too, atomically with the add, rather than
                        // reading HostList from this thread.
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            if (!HostList.Any(h => string.Equals(h.IpAddress, discovered.IpAddress, StringComparison.OrdinalIgnoreCase)))
                            {
                                HostList.Add(discovered);
                            }
                        });
                    }
                    break;
            }
        }

        public void Dispose()
        {
            client.Servers.CollectionChanged -= ServersCollectionChanged;
            GC.SuppressFinalize(this);
        }
    }
}
