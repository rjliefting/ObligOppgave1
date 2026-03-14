using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Bruker
    {
        public int Id { get; }
        public string Navn { get; }
        public string Epost { get; set; }
        public Bruker (int id, string navn, string epost)
        {
            Id = id;
            Navn = navn;
            Epost = epost;
        }

        public void EditEpost(string nyEpost)
        {
            Epost = nyEpost;
        }
    }
}
