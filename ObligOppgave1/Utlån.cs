using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Utlån
    {
        public int BokId { get; }
        private int BrukerId { get; }
        public DateTime Start { get; }
        public DateTime Slutt { get; set; }

        public Utlån()
        {
            BokId = -1;
            BrukerId = -1;
            Start = DateTime.Now;
            Slutt = DateTime.Now;
        }
        public Utlån(int bokId, int brukerId, int durationDays)
        {
            BokId = bokId;
            BrukerId = brukerId;
            Start = DateTime.Now;
            Slutt = DateTime.Now.AddDays(durationDays);
        }
    }
}
