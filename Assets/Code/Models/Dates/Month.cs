using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlicedLife.Models.Date
{
    public class Month
    {
        public MonthNames MonthName { get; set; }
        public int AmountOfDays { get; set; }

        public Month(MonthNames monthName, int amountOfDays)
        {
            MonthName = monthName;
            AmountOfDays = amountOfDays;
        }
    }
}
