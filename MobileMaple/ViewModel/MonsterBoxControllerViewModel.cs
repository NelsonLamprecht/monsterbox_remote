﻿using System;
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

        public MonsterBoxControllerViewModel() : base()
        {
            IsBusy = false;
            SendCommand = new Command(async (obj) => await SendMeadowCommand(obj as string));            
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
                            response = await PostHttpDataWithCommand(complexCommand);
                            break;
                        }
                    case "werewolf":
                    case "laugh":
                    case "chains":
                    case "heartbeat":
                    case "dragongrowl":
                    case "doorcreek":
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
                                response = await PostHttpDataWithCommand(complexCommand);
                            }
                            else
                            {
                                Debug.WriteLine("Unknown sound file.");                                
                            }
                            break;
                        }
                    default:
                        {
                            response = await PostHttpDataWithCommand(command);
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
                        return GetFileSoundParameters(2,3);
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
                        return GetFileSoundParameters(7, 05);
                    }
                case "creature2":
                    {
                        return GetFileSoundParameters(8, 03);
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
                default:
                    {
                        return null;
                    }
            }
        }

        private async Task<bool> PostHttpDataWithCommand(string command)
        {
            bool response;

            Debug.WriteLine($"{SelectedServer?.IpAddress}, {ServerPort}, {command}, {string.Empty}");
            
            response = await client.PostAsync(SelectedServer.IpAddress, ServerPort, command, string.Empty);

            return response;
        }
    }
}