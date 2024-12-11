using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace BusinessLayer
{
    public class Calendar
    {
        [JsonIgnore]
        public ObservableCollection<DateTime> Dates { get; set; } = new ObservableCollection<DateTime>();
        public ObservableCollection<MedicationEvent> Events { get; set; } = new ObservableCollection<MedicationEvent>();


        public Calendar()
        {
            InitializeCalendar();
        }

        private void InitializeCalendar()
        {
            DateTime startDate = DateTime.Today.AddMonths(-1);
            DateTime endDate = DateTime.Today.AddMonths(3);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                Dates.Add(date);
            }
        }

        public MedicationEvent FindEvent(DateTime Date)
        {
            foreach (MedicationEvent medicationEvent in Events)
            {
                if (medicationEvent.Date == Date) return medicationEvent;
            }

            return null;
        }
    }
}