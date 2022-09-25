using System;
using Xamarin.Forms;

namespace MobileMaple.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "Back");            
        }

        void BtnLedClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LedControllerPage());
        }       
    }
}