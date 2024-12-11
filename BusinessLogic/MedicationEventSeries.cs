using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BusinessLayer
{
    public class MedicationEventSeries
    {
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Medicine Medicine { get; set; }
        public ObservableCollection<DayOfWeek> Days { get; set; } = new ObservableCollection<DayOfWeek>();
        public TimeOnly Time { get; set; }

        public MedicationEventSeries() { }

        public void Apply(Calendar calendar, ObservableCollection<DateTime> dates)
        {
            for (int i = 0; i < dates.Count; i++)
            {
                MedicationEvent medicationEvent = calendar.FindEvent(dates[i]);

                if (dates[i] >= DateFrom && (!DateTo.HasValue || dates[i] <= DateTo))
                {
                    if (Days.Count == 0 || Days.Contains(dates[i].DayOfWeek))
                    {
                        if (medicationEvent == null)
                        {
                            medicationEvent = new MedicationEvent();
                            medicationEvent.Date = dates[i];
                            calendar.Events.Add(medicationEvent);
                        }

                        medicationEvent.AddMedicine(Medicine, Time);
                    }
                }
            }
        }
    }
}