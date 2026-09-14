using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
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

        public ObservableCollection<ServerModel> HostList { get; set; }

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
                // Cleared up front so repeat scans (e.g. re-entering the Controller
                // tab) don't accumulate duplicate entries for the same physical
                // device if MapleClient re-announces already-known hosts.
                HostList.Clear();

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
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private void ServersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ServerModel server in e.NewItems)
                    {
                        Debug.WriteLine($"'{server.Name}' @ ip:[{server.IpAddress}]");

                        // MapleClient raises this from its UDP listener thread, not the UI
                        // thread. Android/iOS tolerate an off-thread ObservableCollection
                        // mutation; WinUI3 hard-crashes on it (native 0xc000027b in
                        // Microsoft.UI.Xaml.dll), so this must be marshaled back to the
                        // main thread before touching HostList.
                        var discovered = new ServerModel { Name = $"{server.Name} ({server.IpAddress})", IpAddress = server.IpAddress };
                        MainThread.BeginInvokeOnMainThread(() => HostList.Add(discovered));
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
