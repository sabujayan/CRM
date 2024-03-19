using System;
using System.Collections.Generic;
using System.Text;

namespace Indo.ProjectOrders
{
    public class MonthlyEarningDto
    {
        public string MonthName { get; set; }
        public float Amount { get; set; }
        public float AmountConfirm { get; set; }
        public float AmountCancelled { get; set; }
    }
}
