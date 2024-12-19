using System;
using Microsoft.Maui.Controls;
using BusinessLayer;
using Home_FA_Kit.Resources.Strings;

namespace Home_FA_Kit
{
    public partial class EditFirstAidKitPage : ContentPage
    {
        private MainPage _mainPage;
        private FirstAidKit _originalPharmacy;
        private bool _isPharmacySaved = false;

        public EditFirstAidKitPage(MainPage mainPage, FirstAidKit originalPharmacy)
        {
            InitializeComponent();
            _mainPage = mainPage;
            _originalPharmacy = originalPharmacy;

            pharmacyNameEntry.Text = originalPharmacy.Name;
        }

        private async void OnSavePharmacyClicked(object sender, EventArgs e)
        {
            var newPharmacyName = pharmacyNameEntry.Text;
            if (!string.IsNullOrEmpty(newPharmacyName))
            {
                _originalPharmacy.Name = newPharmacyName;
                _mainPage.UpdatePharmacy();
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