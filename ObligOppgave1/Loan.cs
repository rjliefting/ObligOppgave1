using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave2
{
    public class Loan
    {
        public int BookId { get; }
        public int UserId { get; }
        public DateTime Start { get; }
        public DateTime End { get; set; }

        public Loan()
        {
            BookId = -1;
            UserId = -1;
            Start = DateTime.Now;
            End = DateTime.Now;
        }
        public Loan(int bookId, int userId, int durationDays)
        {
            BookId = bookId;
            UserId = userId;
            Start = DateTime.Now;
            End = DateTime.Now.AddDays(durationDays);
        }
    }
}
