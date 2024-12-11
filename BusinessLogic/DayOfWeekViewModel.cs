using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class DayOfWeekViewModel
    {
        public string DayName { get; set; }
        public DayOfWeek Day { get; set; }
        public bool IsSelected { get; set; }
    }
}
