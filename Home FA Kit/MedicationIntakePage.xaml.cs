using System;
using System.Linq;
using Microsoft.Maui.Controls;
using BusinessLayer;
using DataLayer;
using Home_FA_Kit.Resources.Strings;

namespace Home_FA_Kit
{
    public partial class MedicationIntakePage : ContentPage
    {
        private PharmacyApp _pharmacyApp;
        private MedicationEvent _currentEvent;
        private DateTime _currentDate;

        public MedicationIntakePage(PharmacyApp pharmacyApp)
        {
            InitializeComponent();
            _pharmacyApp = pharmacyApp;
            BindingContext = _pharmacyApp;
            SelectCurrentDate();
        }

        private void SelectCurrentDate()
        {
            _currentDate = _pharmacyApp.Calendar.Dates.FirstOrDefault(e => e == DateTime.Today);
            _currentEvent = _pharmacyApp.Calendar.FindEvent(_currentDate);
            if (_currentEvent != null)
            {
                UpdateMedicinesList();
            }
        }

        private void UpdateMedicinesList()
        {
            dateLabel.Text = _currentDate.ToString("dd.MM.yyyy");

            var sortedMedicines = _currentEvent?.Medicines.OrderBy(m => m.Time).ToList();

            medicinesListView.ItemsSource = sortedMedicines;
        }

        private void OnDateClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is DateTime selectedDate)
            {
                _currentDate = selectedDate;
                _currentEvent = _pharmacyApp.Calendar.FindEvent(_currentDate);
                UpdateMedicinesList();
            }
        }

        private async void OnAddMedicineClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddMedicineToTakePage(_pharmacyApp, _currentDate));
        }

        private void OnMedicineTakenClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is MedicineTakenStatus selectedMedicine)
            {
                selectedMedicine.MarkAsTaken();
                PharmacyAppSaver.SaveToFile(_pharmacyApp, "pharmacyApp.json");
            }
        }

        private async void OnEditMedicineClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is MedicineTakenStatus selectedMedicine)
            {
                await Navigation.PushAsync(new EditMedicationIntakePage(_pharmacyApp, _currentEvent, selectedMedicine));
                UpdateMedicinesList();
            }
        }

        private async void OnDeleteMedicineClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is MedicineTakenStatus selectedMedicine)
            {
                if (AppResources.Culture != null && AppResources.Culture.Name == "en")
                {
                    var result = await DisplayAlert("Confirmation", "Are you sure you want to delete this medicine?", "Yes", "No");
                    if (!result)
                    {
                        return;
                    }
                }
                else
                {
                    var result = await DisplayAlert("Подтверждение", "Вы уверены, что хотите удалить это лекарство?", "Да", "Нет");
                    if (!result)
                    {
                        return;
                    }
                }

                _currentEvent.RemoveMedicine(selectedMedicine.Medicine);

                var existingMedicines = _pharmacyApp.GetAllMedicines();
                _pharmacyApp.Calendar.CleanUpMedications(existingMedicines);

                PharmacyAppSaver.SaveToFile(_pharmacyApp, "pharmacyApp.json");

                UpdateMedicinesList();
            }
        }

        private async void OnPharmaciesClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void OnMedicationClicked(object sender, EventArgs e) { }

        private async void OnStatisticsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StatisticsPage(_pharmacyApp));
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage(_pharmacyApp));
        }
    }
}