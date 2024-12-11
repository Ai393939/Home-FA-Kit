using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLayer;

namespace DataLayer
{
    public static class CategoryLoader
    {
        public static List<Category> LoadCategories(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Category>>(jsonString);
            }
            return new List<Category>();
        }
    }

    public static class CategorySaver
    {
        public static void SaveCategories(List<Category> categories, string filePath)
        {
            string jsonString = JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
        }
    }
}
