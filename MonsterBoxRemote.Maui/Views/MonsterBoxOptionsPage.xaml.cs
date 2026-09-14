using System;
using System.Diagnostics;

using MonsterBoxRemote.Maui.ViewModel;
using Meadow.Foundation.Web.Maple;

namespace MonsterBoxRemote.Maui.Views
{
    public partial class MonsterBoxOptionsPage : ContentPage
    {
        private MonsterBoxControllerViewModel ViewModel { get; }

        public MonsterBoxOptionsPage(MonsterBoxControllerViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
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
