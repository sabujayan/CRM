using System;
using System.Collections.Generic;
using System.Text;

namespace Indo.CustomerCreditNotes
{
    public class CreditNoteCountDto
    {
        public int CountConfirm { get; set; }
        public int CountCancelled { get; set; }
        public int CountDraft { get; set; }
    }
}
