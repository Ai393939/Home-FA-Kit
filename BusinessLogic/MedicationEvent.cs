using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BusinessLayer
{
    public class MedicationEvent
    {
        public DateTime Date { get; set; }
        public ObservableCollection<MedicineTakenStatus> Medicines { get; set; } = new ObservableCollection<MedicineTakenStatus>();

        public MedicationEvent() { }

        public void AddMedicine(Medicine medicine, TimeOnly time)
        {
            Medicines.Add(new MedicineTakenStatus(medicine, time));
        }

        public void RemoveMedicine(Medicine medicine)
        {
            var medicineToRemove = Medicines.FirstOrDefault(m => m.Medicine == medicine);
            if (medicineToRemove != null)
            {
                Medicines.Remove(medicineToRemove);
            }
        }

        public void EditMedicineTime(Medicine medicine, TimeOnly newTime)
        {
            var medicineToEdit = Medicines.FirstOrDefault(m => m.Medicine == medicine);
            if (medicineToEdit != null)
            {
                medicineToEdit.Time = newTime;
            }
        }

        public void MarkAsTaken(Medicine medicine)
        {
            var medicineToMark = Medicines.FirstOrDefault(m => m.Medicine == medicine);
            if (medicineToMark != null)
            {
                medicineToMark.MarkAsTaken();
            }
        }
    }
}