using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BusinessLayer;

namespace DataLayer
{
    public class PharmacyAppLoader
    {
        public static PharmacyApp LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<PharmacyApp>(jsonString);
            }
            return new PharmacyApp();
        }
    }

    public class PharmacyAppSaver
    {
        public static void SaveToFile(PharmacyApp pharmacyApp, string filePath)
        {
            string jsonString = JsonSerializer.Serialize(pharmacyApp, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
        }
    }
}
