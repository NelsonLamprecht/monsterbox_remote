using System;
using System.Diagnostics;

using Xamarin.Forms;

using Meadow.Foundation.Web.Maple;
using MonsterBoxRemote.Mobile.ViewModel;

namespace MonsterBoxRemote.Mobile.View
{
    public partial class MonsterBoxControllerPage : ContentPage
    {
        private MonsterBoxControllerViewModel ViewModel { get; }

        public MonsterBoxControllerPage()
        {
            InitializeComponent();
            BindingContext = new MonsterBoxControllerViewModel();
            ViewModel = BindingContext as MonsterBoxControllerViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.GetServers();
        }    

        private void BeginIterationsStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue);
            if (int.TryParse(e.NewValue.ToString(), out var value))
            {
                if (ViewModel != null)
                {
                    ViewModel.BeginIterations = value;
                }
            }
        }

        private void EndIterationsStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue);
            if (int.TryParse(e.NewValue.ToString(), out var value))
            {
                if (ViewModel != null)
                {
                    ViewModel.EndIterations = value;
                }
            }
        }

        private void BeginDelayStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue);
            if (int.TryParse(e.NewValue.ToString(), out var value))
            {
                if (ViewModel != null)
                {
                    ViewModel.BeginDelay = value;
                }
            }
        }

        private void EndDelayStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue);
            if (int.TryParse(e.NewValue.ToString(), out var value))
            {
                if (ViewModel != null)
                {
                    ViewModel.EndDelay = value;
                }
            }            
        }

        private void PagePicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            if (picker.SelectedItem is PageModel selectedItem)
            {
                switch (selectedItem.Name)
                {
                    case "Options Page":
                        {
                            Navigation.PushAsync(new MonsterBoxOptionsPage());
                            break;
                        }
                    case "Controller Page":
                        {
                            Navigation.PushAsync(new MonsterBoxControllerPage());
                            break;
                        }
                }
            }
        }

        private void PickerMonsterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            if (picker.SelectedItem is ServerModel selectedItem)
            {
                ViewModel.MonsterBoxDevice = selectedItem;
            }
        }

        private void PickerScareCrow_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            if (picker.SelectedItem is ServerModel selectedItem)
            {
                ViewModel.ScareCrowDevice = selectedItem;
            }
        }
    }
}