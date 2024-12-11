using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLayer
{
    public class FirstAidKit : INotifyPropertyChanged
    {
        private string name;
        private ObservableCollection<Medicine> medicines = new ObservableCollection<Medicine>();

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Medicine> Medicines
        {
            get { return medicines; }
            set { medicines = value; }
        }

        public FirstAidKit(string name)
        {
            Name = name;
        }

        public FirstAidKit() { }

        public void AddMedicine(Medicine medicine)
        {
            medicines.Add(medicine);
        }

        public void RemoveMedicine(Medicine medicine)
        {
            medicines.Remove(medicine);
        }

        public List<Medicine> FindMedicinesByCategory(Category category)
        {
            return medicines.Where(m => m.Category == category).ToList();
        }

        public List<Medicine> FindMedicinesByExpirationDate(DateTime date)
        {
            return medicines.Where(m => m.ExpirationDate <= date).ToList();
        }

        public List<Medicine> FindMedicinesBySubCategory(Category subCategory)
        {
            return medicines.Where(m => m.Category == subCategory).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ComparePharmacyByName : IComparer<FirstAidKit>
    {
        public int Compare(FirstAidKit x, FirstAidKit y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}