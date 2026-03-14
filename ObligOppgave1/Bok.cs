using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Bok
    {
        public int Id { get; }
        public string Tittel { get; }
        public string Forfatter { get; }
        public int År { get; set; }
        public Utlån Lån { get; set; }
        public List<Utlån> Historikk { get; }

        public Bok(int id, string tittel, string forfatter, int år)
        {
            Id = id;
            Tittel = tittel;
            Forfatter = forfatter;
            År = år;
            Lån = new Utlån();
            Historikk = new List<Utlån>();
        }
        public Bok(string tittel, string forfatter, int år)
        {
            Tittel = tittel;
            Forfatter = forfatter;
            År = år;
            Lån = new Utlån();
            Historikk = new List<Utlån>();
        }
    }
}
