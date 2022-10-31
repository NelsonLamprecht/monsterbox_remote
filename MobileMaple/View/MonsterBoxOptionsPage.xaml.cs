using System.Diagnostics;

using Xamarin.Forms;

using MonsterBoxRemote.Mobile.ViewModel;

namespace MonsterBoxRemote.Mobile.View
{
    public partial class MonsterBoxOptionsPage : ContentPage
    {
        private MonsterBoxControllerViewModel ViewModel { get; }

        public MonsterBoxOptionsPage()
        {
            InitializeComponent();
            BindingContext = new MonsterBoxControllerViewModel();
            ViewModel = BindingContext as MonsterBoxControllerViewModel;
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
    }
}