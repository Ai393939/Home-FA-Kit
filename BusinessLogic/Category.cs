using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Subcategory
    {
        private string id;
        private string name;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Subcategory() {}
    }

    public class Category : Subcategory
    {
        private List<Subcategory> subcategories = new List<Subcategory>();

        public List<Subcategory> Subcategories
        {
            get { return subcategories; }
            set {  subcategories = value; }
        }

        public Category() { }

        public void AddSubcategory(Subcategory subcategory)
        {
            subcategories.Add(subcategory);
        }
    }
}