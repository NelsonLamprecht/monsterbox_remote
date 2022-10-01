using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MonsterBoxRemote.Mobile.ViewModel
{
    public class MonsterBoxControllerViewModel : BaseViewModel
    {
        bool _isShake;
        public bool IsShake
        {
            get => _isShake;
            set { _isShake = value; OnPropertyChanged(nameof(IsShake)); }
        }

        int _beginIterations = 25;
        public int BeginIterations
        {
            get => _beginIterations;
            set { _beginIterations = value; OnPropertyChanged(nameof(BeginIterations)); }
        }

        int _endIterations = 50;
        public int EndIterations
        {
            get => _endIterations;
            set { _endIterations = value; OnPropertyChanged(nameof(EndIterations)); }
        }

        int _beginDelay = 50;
        public int BeginDelay
        {
            get => _beginDelay;
            set { _beginDelay = value; OnPropertyChanged(nameof(BeginDelay)); }
        }

        int _endDelay = 75;
        public int EndDelay
        {
            get => _endDelay;
            set { _endDelay = value; OnPropertyChanged(nameof(EndDelay)); }
        }

        public MonsterBoxControllerViewModel() : base()
        {
            SendCommand = new Command(async (obj) => await SendMeadowCommand(obj as string));
            IsShake = true;
            
        }

        async Task SendMeadowCommand(string command)
        {
            if (IsBusy || SelectedServer == null)            
            {
                return;
            }

            IsBusy = true;

            try
            {
                bool response = false;
                switch (command)
                {
                    case "Shake":
                        {
                            var query = new Dictionary<string, string>()
                            {
                                ["bi"] = BeginIterations.ToString(),
                                ["ei"] = EndIterations.ToString(),
                                ["bd"] = BeginDelay.ToString(),
                                ["ed"] = EndDelay.ToString(),
                            };
                            var complexCommand = RequestUriUtil.GetUriWithQueryString(command, query).ToLower();
                            response = await PostHttpDataWithCommand(complexCommand);
                            break;
                        }
                    default:
                        {
                            response = await PostHttpDataWithCommand(command);
                            break;
                        }
                }
                

                if (response)
                {
                    IsShake = false;

                    switch (command) 
                    {
                        case "Shake": IsShake = true; break;
                        default: throw new Exception($"Unknown command fallthrough: {command}.");
                    }
                }
                else
                {
                    await App.Current.DisplayAlert("Error", "Request failed.", "Close");
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

        private async Task<bool> PostHttpDataWithCommand(string command) 
        {
            bool response;
            
            //Debug.WriteLine($"{SelectedServer?.IpAddress}, {ServerPort}, {command}, {string.Empty}");
            //response = true;
            
            response = await client.PostAsync(SelectedServer.IpAddress, ServerPort, command, string.Empty);
            
            return response;
        }
    }
}