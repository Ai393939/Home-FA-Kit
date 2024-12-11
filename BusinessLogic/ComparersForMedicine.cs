using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CompareMedicineByName : IComparer<Medicine>
    {
        public int Compare(Medicine x, Medicine y)
        {
            if (x.Name == null && y.Name == null) return 0;
            if (x.Name == null) return -1;
            if (y.Name == null) return 1;
            return x.Name.CompareTo(y.Name);
        }
    }

    public class CompareMedicineByCost : IComparer<Medicine>
    {
        public int Compare(Medicine x, Medicine y)
        {
            return x.Cost.CompareTo(y.Cost);
        }
    }

    public class CompareMedicineByExpirationDate : IComparer<Medicine>
    {
        public int Compare(Medicine x, Medicine y)
        {
            return x.ExpirationDate.CompareTo(y.ExpirationDate);
        }
    }

    public class CompareMedicineByActiveIngredient : IComparer<Medicine>
    {
        public int Compare(Medicine x, Medicine y)
        {
            if (x.ActiveIngredient == null && y.ActiveIngredient == null) return 0;
            if (x.ActiveIngredient == null) return -1;
            if (y.ActiveIngredient == null) return 1;
            return x.ActiveIngredient.CompareTo(y.ActiveIngredient);
        }
    }

    public class CompareMedicineByManufacturer : IComparer<Medicine>
    {
        public int Compare(Medicine x, Medicine y)
        {
            if (x.Manufacturer == null && y.Manufacturer == null) return 0;
            if (x.Manufacturer == null) return -1;
            if (y.Manufacturer == null) return 1;
            return x.Manufacturer.CompareTo(y.Manufacturer);
        }
    }

    public class CompareMedicineByCountry : IComparer<Medicine>
    {
        public int Compare(Medicine x, Medicine y)
        {
            if (x.Country == null && y.Country == null) return 0;
            if (x.Country == null) return -1;
            if (y.Country == null) return 1;
            return x.Country.CompareTo(y.Country);
        }
    }

    public class CompareMedicineByCategory : IComparer<Medicine>
    {
        public int Compare(Medicine x, Medicine y)
        {
            if (x.Category == null && y.Category == null) return 0;
            if (x.Category == null) return -1;
            if (y.Category == null) return 1;
            if (x.Category.Name == null && y.Category.Name == null) return 0;
            if (x.Category.Name == null) return -1;
            if (y.Category.Name == null) return 1;
            return x.Category.Name.CompareTo(y.Category.Name);
        }
    }
}