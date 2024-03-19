using System;
using System.Collections.Generic;
using System.Text;

namespace Indo.SalesOrders
{
    public class OrderCountDto
    {
        public int CountConfirm { get; set; }
        public int CountCancelled { get; set; }
        public int CountDraft { get; set; }
    }
}
