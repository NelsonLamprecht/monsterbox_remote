using System;
using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

using MonsterBoxRemote.Maui.ViewModel;

namespace MonsterBoxRemote.Maui.Views
{
    public partial class MonsterBoxOptionsPage : ContentPage
    {
        private readonly IServiceProvider _services;
        private MonsterBoxControllerViewModel ViewModel { get; }

        public MonsterBoxOptionsPage(MonsterBoxControllerViewModel viewModel, IServiceProvider services)
        {
            InitializeComponent();
            _services = services;
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

        private void PagePicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            var selectedItem = picker.SelectedItem as PageModel;
            if (selectedItem != null)
            {
                switch (selectedItem.Name)
                {
                    case "Options Page":
                        {
                            Navigation.PushAsync(_services.GetRequiredService<MonsterBoxOptionsPage>());
                            break;
                        }
                    case "Controller Page":
                        {
                            Navigation.PushAsync(_services.GetRequiredService<MonsterBoxControllerPage>());
                            break;
                        }
                }
            }
        }
    }
}
