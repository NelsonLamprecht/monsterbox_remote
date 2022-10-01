using System;
using Xamarin.Forms;

namespace MonsterBoxRemote.Mobile.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "Back");            
        }

        void BtnMonsterBoxClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MonsterBoxControllerPage());
        }       
    }
}