using Meadow.Foundation.Web.Maple.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MonsterBoxRemote.Mobile.ViewModel
{
    public class MonsterBoxControllerViewModel : BaseViewModel
    {
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

        public ServerModel MonsterBoxDevice { get; internal set; }

        public ServerModel ScareCrowDevice { get; internal set; }

        public Command SendMonsterBoxCommand { set; get; }

        public Command SendScarecrowCommand { set; get; }

        public MonsterBoxControllerViewModel() : base()
        {
            IsBusy = false;
            SendMonsterBoxCommand = new Command(async (obj) => await SendMeadowCommand(MonsterBoxDevice?.IpAddress, obj as string));
            SendScarecrowCommand = new Command(async (obj) => await SendMeadowCommand(ScareCrowDevice?.IpAddress, obj as string));
        }

        async Task SendMeadowCommand(string hostAddress, string command)
        {
            if (IsBusy || string.IsNullOrEmpty(hostAddress) || string.IsNullOrEmpty(command))
            {
                return;
            }

            IsBusy = true;

            try
            {
                bool response = false;
                switch (command.ToLower())
                {
                    case "shake":
                        {
                            var query = new Dictionary<string, string>()
                            {
                                ["bi"] = BeginIterations.ToString(),
                                ["ei"] = EndIterations.ToString(),
                                ["bd"] = BeginDelay.ToString(),
                                ["ed"] = EndDelay.ToString(),
                            };
                            var complexCommand = RequestUriUtil.GetUriWithQueryString(command, query).ToLower();
                            response = await PostHttpDataWithCommand(hostAddress,complexCommand);
                            break;
                        }
                    case "werewolf":
                    case "laugh":
                    case "chains":
                    case "heartbeat":
                    case "dragongrowl":
                    case "doorcreek":
                    case "metalhit":
                    case "raven":
                    case "creature1":
                    case "creature2":
                    case "creature3":
                    case "creature4":
                    case "creature5":
                        {
                            Dictionary<string, string> query = GetSoundFileParameters(command);
                            if (query != null)
                            {
                                var complexCommand = RequestUriUtil.GetUriWithQueryString("sound", query).ToLower();
                                response = await PostHttpDataWithCommand(hostAddress,complexCommand);
                            }
                            else
                            {
                                Debug.WriteLine("Unknown sound file.");
                            }
                            break;
                        }
                    default:
                        {
                            response = await PostHttpDataWithCommand(hostAddress,command);
                            break;
                        }
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

        private static Dictionary<string, string> GetFileSoundParameters(int fileNumber, int fileDuration)
        {
            return new Dictionary<string, string>()
            {
                ["filenumber"] = fileNumber.ToString(),
                ["fileduration"] = fileDuration.ToString()
            };
        }

        private static Dictionary<string, string> GetSoundFileParameters(string command)
        {
            switch (command)
            {
                case "werewolf":
                    {
                        return GetFileSoundParameters(1,9);
                    }
                case "laugh":
                    {
                        return GetFileSoundParameters(2,2);
                    }
                case "chains":
                    {
                        return GetFileSoundParameters(3, 13);
                    }
                case "heartbeat":
                    {
                        return GetFileSoundParameters(4, 12);
                    }
                case "dragongrowl":
                    {
                        return GetFileSoundParameters(5, 5);
                    }
                case "doorcreek":
                    {
                        return GetFileSoundParameters(6, 2);
                    }
                case "creature1":
                    {
                        return GetFileSoundParameters(7, 5);
                    }
                case "creature2":
                    {
                        return GetFileSoundParameters(8, 3);
                    }
                case "creature3":
                    {
                        return GetFileSoundParameters(9, 7);
                    }
                case "creature4":
                    {
                        return GetFileSoundParameters(10, 7);
                    }
                case "creature5":
                    {
                        return GetFileSoundParameters(11, 6);
                    }
                case "metalhit":
                    {
                        return GetFileSoundParameters(12, 8);
                    }
                case "raven":
                    {
                        return GetFileSoundParameters(13, 2);
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        private async Task<bool> PostHttpDataWithCommand(string hostAddress, string command)
        {
            bool response;
            
            response = await client.PostAsync(hostAddress, ServerPort, command, string.Empty);

            return response;
        }
    }
}