using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BusinessLayer
{
    public class PharmacyApp : INotifyPropertyChanged
    {
        private ObservableCollection<FirstAidKit> _pharmacies = new ObservableCollection<FirstAidKit>();
        private Settings _appSettings = new Settings();
        private Statistics _pharmacyStatistics;
        private Calendar _calendar = new Calendar();
        private List<Category> _categories;

        public ObservableCollection<FirstAidKit> Pharmacies
        {
            get => _pharmacies;
            set
            {
                _pharmacies = value;
                OnPropertyChanged(nameof(Pharmacies));
                UpdateStatistics();
            }
        }

        [JsonIgnore]
        public Statistics PharmacyStatistics
        {
            get => _pharmacyStatistics;
            private set
            {
                _pharmacyStatistics = value;
                OnPropertyChanged(nameof(PharmacyStatistics));
            }
        }

        public Settings AppSettings
        {
            get => _appSettings;
            set
            {
                _appSettings = value;
                OnPropertyChanged(nameof(AppSettings));
            }
        }

        public Calendar Calendar
        {
            get => _calendar;
            set
            {
                _calendar = value;
                OnPropertyChanged(nameof(Calendar));
            }
        }

        public List<Category> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public PharmacyApp()
        {
            _pharmacyStatistics = new Statistics(_pharmacies);
        }

        public void AddPharmacy(FirstAidKit pharmacy)
        {
            _pharmacies.Add(pharmacy);
            UpdateStatistics();
        }

        public void RemovePharmacy(FirstAidKit pharmacy)
        {
            _pharmacies.Remove(pharmacy);
            UpdateStatistics();
        }

        public void GetPharmacyStatistics()
        {
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            _pharmacyStatistics = new Statistics(_pharmacies);
            OnPropertyChanged(nameof(PharmacyStatistics));
        }

        public List<Medicine> GetAllMedicines()
        {
            var allMedicines = new List<Medicine>();

            // Проходим по всем аптечкам и собираем все лекарства
            foreach (var pharmacy in Pharmacies)
            {
                allMedicines.AddRange(pharmacy.Medicines);
            }

            return allMedicines;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}