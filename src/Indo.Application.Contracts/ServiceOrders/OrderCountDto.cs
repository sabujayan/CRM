using System;
using System.Collections.Generic;
using System.Text;

namespace Indo.ServiceOrders
{
    public class OrderCountDto
    {
        public int CountConfirm { get; set; }
        public int CountCancelled { get; set; }
        public int CountDraft { get; set; }
    }
}
