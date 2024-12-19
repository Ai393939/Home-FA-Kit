using System;
using Microsoft.Maui.Controls;
using BusinessLayer;
using Home_FA_Kit.Resources.Strings;

namespace Home_FA_Kit
{
    public partial class CreateFirstAidKitPage : ContentPage
    {
        private MainPage _mainPage;
        private bool _isPharmacySaved = false;

        public CreateFirstAidKitPage(MainPage mainPage)
        {
            InitializeComponent();
            _mainPage = mainPage;
        }

        private async void OnSavePharmacyClicked(object sender, EventArgs e)
        {
            var pharmacyName = pharmacyNameEntry.Text;
            if (!string.IsNullOrEmpty(pharmacyName))
            {
                var newPharmacy = new FirstAidKit(pharmacyName);
                _mainPage.AddPharmacy(newPharmacy);
                _isPharmacySaved = true;
                await Navigation.PopAsync();
            }
            else
            {
                if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                {
                    await DisplayAlert("Error", "Pharmacy name cannot be empty", "OK");
                }
                else
                {
                    await DisplayAlert("Ошибка", "Название аптечки не может быть пустым", "OK");
                }
            }
        }
    }
}