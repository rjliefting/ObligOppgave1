using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Ansatt : Bruker
    {
        public string Stilling { get; set; }
        public string Avdeling { get; set; }

        public Ansatt(int id, string navn, string epost, string stilling, string avdeling) : base(id, navn, epost)
        {
            Stilling = stilling;
            Avdeling = avdeling;
        }
    }
}
