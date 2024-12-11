using System;
using Microsoft.Maui.Controls;
using BusinessLayer;

namespace Home_FA_Kit
{
    public partial class EditPharmacyPage : ContentPage
    {
        private MainPage _mainPage;
        private FirstAidKit _originalPharmacy;
        private bool _isPharmacySaved = false;

        public EditPharmacyPage(MainPage mainPage, FirstAidKit originalPharmacy)
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
                await DisplayAlert("Ошибка", "Название аптечки не может быть пустым", "OK");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (!_isPharmacySaved && !string.IsNullOrEmpty(pharmacyNameEntry.Text) && pharmacyNameEntry.Text != _originalPharmacy.Name)
            {
                ShowUnsavedChangesAlert();
                return true;
            }
            return base.OnBackButtonPressed();
        }

        private async void ShowUnsavedChangesAlert()
        {
            var result = await DisplayAlert("Предупреждение", "У вас остались несохранённые изменения. Всё равно выйти?", "Да", "Нет");
            if (result)
            {
                await Navigation.PopAsync();
            }
        }
    }
}