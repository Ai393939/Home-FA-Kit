using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BusinessLayer
{
    public class PharmacyStatistics : INotifyPropertyChanged
    {
        private FirstAidKit _pharmacy;
        private int _totalMedicines;
        private ObservableCollection<Medicine> _expiredMedicines;
        private ObservableCollection<string> _categories;
        private int _categoryCount;

        public FirstAidKit Pharmacy
        {
            get => _pharmacy;
            set
            {
                _pharmacy = value;
                OnPropertyChanged(nameof(Pharmacy));
            }
        }

        public int TotalMedicines
        {
            get => _totalMedicines;
            set
            {
                _totalMedicines = value;
                OnPropertyChanged(nameof(TotalMedicines));
            }
        }

        public ObservableCollection<Medicine> ExpiredMedicines
        {
            get => _expiredMedicines;
            set
            {
                _expiredMedicines = value;
                OnPropertyChanged(nameof(ExpiredMedicines));
            }
        }

        public ObservableCollection<string> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public int CategoryCount
        {
            get => _categoryCount;
            set
            {
                _categoryCount = value;
                OnPropertyChanged(nameof(CategoryCount));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}