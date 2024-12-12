using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLayer
{
    public class Medicine : INotifyPropertyChanged
    {
        private string name;
        private string description;
        private int cost;
        private int quantity;
        private DateTime expirationDate;
        private string activeIngredient;
        private string manufacturer;
        private string country;
        private string pharmacologicalEffect;
        private string form;
        private string note;
        private Category category;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }

        public int Cost
        {
            get { return cost; }
            set { cost = value; OnPropertyChanged(); }
        }

        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; OnPropertyChanged(); }
        }

        public string ActiveIngredient
        {
            get { return activeIngredient; }
            set { activeIngredient = value; OnPropertyChanged(); }
        }

        public string Manufacturer
        {
            get { return manufacturer; }
            set { manufacturer = value; OnPropertyChanged(); }
        }

        public string Country
        {
            get { return country; }
            set { country = value; OnPropertyChanged(); }
        }

        public string PharmacologicalEffect
        {
            get { return pharmacologicalEffect; }
            set { pharmacologicalEffect = value; OnPropertyChanged(); }
        }

        public string Form
        {
            get { return form; }
            set { form = value; OnPropertyChanged(); }
        }

        public string Note
        {
            get { return note; }
            set { note = value; OnPropertyChanged(); }
        }

        public Category Category
        {
            get { return category; }
            set { category = value; OnPropertyChanged(); }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(); }
        }

        public Medicine() { }

        public Medicine(string name, string description, int cost, DateTime expirationDate, string activeIngredient, string manufacturer, string country,
            string pharmacologicalEffect, string form, string note, Category category, int quantity)
        {
            Name = name;
            Description = description;
            Cost = cost;
            ExpirationDate = expirationDate;
            ActiveIngredient = activeIngredient;
            Manufacturer = manufacturer;
            Country = country;
            PharmacologicalEffect = pharmacologicalEffect;
            Form = form;
            Note = note;
            Category = category;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return String.Format($"{Name} {Description} {Cost} {ExpirationDate} {ActiveIngredient} {Manufacturer} {Country} " +
                $"{PharmacologicalEffect} {Form} {Note} {Category.Name} {Category.Subcategories[0].Name} {Quantity} ");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}