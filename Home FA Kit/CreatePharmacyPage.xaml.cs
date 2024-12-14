using System;
using Microsoft.Maui.Controls;
using BusinessLayer;

namespace Home_FA_Kit
{
    public partial class CreatePharmacyPage : ContentPage
    {
        private MainPage _mainPage;
        private bool _isPharmacySaved = false;

        public CreatePharmacyPage(MainPage mainPage)
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
                await DisplayAlert("Ошибка", "Название аптечки не может быть пустым", "OK");
            }
        }
    }
}