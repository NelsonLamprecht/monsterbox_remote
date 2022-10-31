using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xamarin.Forms;
using Meadow.Foundation.Web.Maple.Client;

namespace MonsterBoxRemote.Mobile.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
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

        bool _isServerListEmpty;
        public bool IsServerListEmpty
        {
            get => _isServerListEmpty;
            set { _isServerListEmpty = value; OnPropertyChanged(nameof(IsServerListEmpty)); }
        }

        string ipAddress;
        public string IpAddress
        {
            get => ipAddress;
            set { ipAddress = value; OnPropertyChanged(nameof(IpAddress)); }
        }

        public ObservableCollection<ServerModel> HostList { get; set; }

        public ObservableCollection<PageModel> PageList { get; set; }

        public Command SearchServersCommand { set; get; }

        public BaseViewModel()
        {
            HostList = new ObservableCollection<ServerModel>();
            PageList = new ObservableCollection<PageModel>();

            // TODO: Dynamic!
            PageList.Add(new PageModel()
            {
                Name = "Controller Page"
            });
            PageList.Add(new PageModel()
            {
                Name = "Options Page"
            });

            //HostList.Add(new ServerModel() { Name="Meadow (192.168.1.73)", IpAddress="192.168.1.73" });
            //HostList.Add(new ServerModel() { Name = "Meadow (192.168.1.74)", IpAddress = "192.168.1.74" });

            ServerPort = 5417;

            client = new MapleClient
            {
                ListenTimeout = MapleClientListenTimeout
            };
            client.Servers.CollectionChanged += ServersCollectionChanged;

            SearchServersCommand = new Command(async () => await GetServers());
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
                IsServerListEmpty = false;

                await client.StartScanningForAdvertisingServers();

                if (HostList.Count == 0)
                {
                    IsServerListEmpty = true;

                }
                else
                {
                    IsServerListEmpty = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
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
                        HostList.Add(new ServerModel() { Name = $"{server.Name} ({server.IpAddress})", IpAddress = server.IpAddress });
                        Console.WriteLine($"'{server.Name}' @ ip:[{server.IpAddress}]");
                    }
                    break;
            }
        }
    }
}
