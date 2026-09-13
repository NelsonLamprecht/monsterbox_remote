using System;
using Microsoft.Extensions.DependencyInjection;

namespace MonsterBoxRemote.Maui.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly IServiceProvider _services;

        public MainPage(IServiceProvider services)
        {
            InitializeComponent();
            _services = services;
            NavigationPage.SetBackButtonTitle(this, "Back");
        }

        void BtnMonsterBoxClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(_services.GetRequiredService<MonsterBoxControllerPage>());
        }
    }
}
