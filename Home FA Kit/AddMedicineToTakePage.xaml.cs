using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using BusinessLayer;
using DataLayer;

namespace Home_FA_Kit
{
    public partial class AddMedicineToTakePage : ContentPage
    {
        private PharmacyApp _pharmacyApp;
        private DateTime _currentDate;
        private ObservableCollection<DayOfWeek> _selectedDays = new ObservableCollection<DayOfWeek>();
        private TimeOnly _selectedTime;

        public AddMedicineToTakePage(PharmacyApp pharmacyApp, DateTime currentDate)
        {
            InitializeComponent();
            _pharmacyApp = pharmacyApp;
            _currentDate = currentDate;

            foreach (var medicine in _pharmacyApp.Pharmacies.SelectMany(p => p.Medicines))
            {
                medicinePicker.Items.Add(medicine.Name);
            }

            dateFromPicker.Date = _currentDate;
            dateToPicker.Date = _currentDate.AddMonths(1);

            daysOfWeekListView.ItemsSource = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(day => new DayOfWeekViewModel
            {
                DayName = day.ToString(),
                Day = day
            }).ToList();
        }

        private void OnDayCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.BindingContext is DayOfWeekViewModel dayViewModel)
            {
                if (e.Value)
                {
                    _selectedDays.Add(dayViewModel.Day);
                }
                else
                {
                    _selectedDays.Remove(dayViewModel.Day);
                }
            }
        }

        private void OnTimePickerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time" && sender is TimePicker timePicker)
            {
                _selectedTime = TimeOnly.FromTimeSpan(timePicker.Time);
            }
        }

        private async void OnAddMedicineClicked(object sender, EventArgs e)
        {
            var selectedMedicineName = medicinePicker.SelectedItem?.ToString();
            var selectedDateFrom = dateFromPicker.Date;
            var selectedDateTo = dateToPicker.Date;

            if (string.IsNullOrEmpty(selectedMedicineName))
            {
                await DisplayAlert("Ошибка", "Выберите лекарство", "OK");
                return;
            }

            if (_selectedDays.Count == 0)
            {
                await DisplayAlert("Ошибка", "Выберите хотя бы один день недели", "OK");
                return;
            }

            if (_selectedTime == default)
            {
                await DisplayAlert("Ошибка", "Выберите время приема", "OK");
                return;
            }

            var selectedMedicine = _pharmacyApp.Pharmacies.SelectMany(p => p.Medicines).FirstOrDefault(m => m.Name == selectedMedicineName);
            if (selectedMedicine != null)
            {
                var series = new MedicationEventSeries
                {
                    DateFrom = selectedDateFrom,
                    DateTo = selectedDateTo,
                    Medicine = selectedMedicine,
                    Days = _selectedDays,
                    Time = _selectedTime
                };

                series.Apply(_pharmacyApp.Calendar, _pharmacyApp.Calendar.Dates);
                PharmacyAppSaver.SaveToFile(_pharmacyApp, "pharmacyApp.json");
                await Navigation.PopAsync();
            }
        }
    }
}