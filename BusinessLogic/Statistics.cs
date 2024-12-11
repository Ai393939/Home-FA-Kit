using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace BusinessLayer
{
    public class Statistics : INotifyPropertyChanged
    {
        private ObservableCollection<FirstAidKit> _pharmacies = new ObservableCollection<FirstAidKit>();
        private ObservableCollection<PharmacyStatistics> _pharmacyStats = new ObservableCollection<PharmacyStatistics>();

        public ObservableCollection<PharmacyStatistics> PharmacyStats
        {
            get => _pharmacyStats;
            private set
            {
                _pharmacyStats = value;
                OnPropertyChanged(nameof(PharmacyStats));
            }
        }

        public Statistics() { }

        public Statistics(ObservableCollection<FirstAidKit> pharmacies)
        {
            _pharmacies = pharmacies;
            UpdateStatistics();
        }

        public void UpdateStatistics()
        {
            var newStats = new ObservableCollection<PharmacyStatistics>();

            foreach (var pharmacy in _pharmacies)
            {
                int totalMedicines = pharmacy.Medicines.Count;
                var expiredMedicines = new ObservableCollection<Medicine>(pharmacy.Medicines.Where(medicine => medicine.ExpirationDate < DateTime.Now));
                var categories = new ObservableCollection<string>();

                for (int i = 0; i < totalMedicines; i++)
                {
                    if (pharmacy.Medicines[i].Category != null)
                    {
                        if (!categories.Contains(pharmacy.Medicines[i].Category.Name))
                        {
                            categories.Add(pharmacy.Medicines[i].Category.Name);
                        }
                    }
                }

                newStats.Add(new PharmacyStatistics
                {
                    Pharmacy = pharmacy,
                    TotalMedicines = totalMedicines,
                    ExpiredMedicines = expiredMedicines,
                    Categories = categories,
                    CategoryCount = categories.Count
                });
            }

            PharmacyStats = newStats;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}