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

        // Словарь с локализованными названиями дней недели
        private readonly Dictionary<string, Dictionary<DayOfWeek, string>> _localizedDayNames = new()
    {
        {
            "ru", new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "Понедельник" },
                { DayOfWeek.Tuesday, "Вторник" },
                { DayOfWeek.Wednesday, "Среда" },
                { DayOfWeek.Thursday, "Четверг" },
                { DayOfWeek.Friday, "Пятница" },
                { DayOfWeek.Saturday, "Суббота" },
                { DayOfWeek.Sunday, "Воскресенье" }
            }
        },
        {
            "en", new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "Monday" },
                { DayOfWeek.Tuesday, "Tuesday" },
                { DayOfWeek.Wednesday, "Wednesday" },
                { DayOfWeek.Thursday, "Thursday" },
                { DayOfWeek.Friday, "Friday" },
                { DayOfWeek.Saturday, "Saturday" },
                { DayOfWeek.Sunday, "Sunday" }
            }
        }
    };

        public AddMedicineToTakePage(PharmacyApp pharmacyApp, DateTime currentDate)
        {
            InitializeComponent();
            _pharmacyApp = pharmacyApp;
            _currentDate = currentDate;

            // Заполняем Picker лекарств
            foreach (var medicine in _pharmacyApp.Pharmacies.SelectMany(p => p.Medicines))
            {
                medicinePicker.Items.Add(medicine.Name);
            }

            // Устанавливаем даты по умолчанию
            dateFromPicker.Date = _currentDate;
            dateToPicker.Date = _currentDate.AddMonths(1);

            // Получаем отсортированные дни недели
            var daysOfWeek = _localizedDayNames[_pharmacyApp.AppSettings.Language].Keys.ToList(); // Получаем ключи (дни недели)

            // Инициализируем daysOfWeekListView с русскими названиями
            daysOfWeekListView.ItemsSource = daysOfWeek.Select(day => new DayOfWeekViewModel
            {
                DayName = _localizedDayNames[_pharmacyApp.AppSettings.Language][day], // Используем русское название
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

        private void OnSelectAllDaysClicked(object sender, EventArgs e)
        {
            foreach (var item in daysOfWeekListView.ItemsSource)
            {
                if (item is DayOfWeekViewModel dayViewModel)
                {
                    dayViewModel.IsSelected = true;
                }
            }

            _selectedDays.Clear();
            foreach (var day in Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>())
            {
                _selectedDays.Add(day);
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