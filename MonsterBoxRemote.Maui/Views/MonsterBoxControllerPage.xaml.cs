using System;
using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

using MonsterBoxRemote.Maui.ViewModel;
using Meadow.Foundation.Web.Maple;

namespace MonsterBoxRemote.Maui.Views
{
    public partial class MonsterBoxControllerPage : ContentPage
    {
        private readonly IServiceProvider _services;
        private MonsterBoxControllerViewModel ViewModel { get; }

        public MonsterBoxControllerPage(MonsterBoxControllerViewModel viewModel, IServiceProvider services)
        {
            InitializeComponent();
            _services = services;
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.GetServers();
        }

        // Material Design 3's compact/medium breakpoint (600dp) - below this,
        // stack the MonsterBox/Scarecrow panels in one column instead of
        // side-by-side, since the side-by-side layout squeezes the sound-button
        // grid well under the 48dp/44pt minimum touch target size on a phone.
        private const double CompactWidthBreakpoint = 600;
        private bool? _isCompactLayout;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            UpdateDeviceLayout(width);
        }

        private void UpdateDeviceLayout(double width)
        {
            if (width <= 0)
            {
                return;
            }

            bool isCompact = width < CompactWidthBreakpoint;
            if (_isCompactLayout == isCompact)
            {
                return;
            }
            _isCompactLayout = isCompact;

            DeviceGrid.RowDefinitions.Clear();
            DeviceGrid.ColumnDefinitions.Clear();

            if (isCompact)
            {
                DeviceGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                DeviceGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                DeviceGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                DeviceGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

                Grid.SetColumnSpan(NavPickerBorder, 1);
                Grid.SetRow(MonsterBoxPanel, 1);
                Grid.SetColumn(MonsterBoxPanel, 0);
                Grid.SetColumnSpan(MonsterBoxPanel, 1);
                Grid.SetRow(ScarecrowPanel, 2);
                Grid.SetColumn(ScarecrowPanel, 0);
                Grid.SetColumnSpan(ScarecrowPanel, 1);
            }
            else
            {
                DeviceGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                DeviceGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                DeviceGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                DeviceGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

                Grid.SetColumnSpan(NavPickerBorder, 2);
                Grid.SetRow(MonsterBoxPanel, 1);
                Grid.SetColumn(MonsterBoxPanel, 0);
                Grid.SetColumnSpan(MonsterBoxPanel, 1);
                Grid.SetRow(ScarecrowPanel, 1);
                Grid.SetColumn(ScarecrowPanel, 1);
                Grid.SetColumnSpan(ScarecrowPanel, 1);
            }
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
