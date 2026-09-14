using System;

using MonsterBoxRemote.Maui.ViewModel;

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

        // Lets pressing Enter/Done on the keyboard submit the manual device
        // address, same as tapping Add - Entry has no Command/CommandParameter
        // of its own to bind Completed to directly.
        private void ManualDeviceEntry_Completed(object sender, EventArgs e)
        {
            if (ViewModel.AddManualDeviceCommand.CanExecute(null))
            {
                ViewModel.AddManualDeviceCommand.Execute(null);
            }
        }
    }
}
