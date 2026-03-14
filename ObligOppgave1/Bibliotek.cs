using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Bibliotek
    {
        private List<Bok> bøker;

        public List<Bok> SearchBøker(string søk)
        {
            List<Bok> søkeresultater = new List<Bok>();
            if (int.TryParse(søk, out int result))
            {
                var bok = (from b in bøker where b.Id == result select b).SingleOrDefault();
                if(bok != null)
                {
                    søkeresultater.Add(bok);
                }
            }

            var titleQuery = from bok in bøker where bok.Tittel.ToLower().Contains(søk.ToLower()) select bok;
            foreach (Bok bok in titleQuery)
            {
                søkeresultater.Add(bok);
            }
            return søkeresultater;
        }

        public Bok GetBok(int bokId)
        {
            var bok = (from b in bøker where b.Id == bokId select b).SingleOrDefault();
            if (bok != null)
            {
                return bok;
            }
            return null;
        }
        public void AddBok(Bok nyBok)
        {
            bøker.Add(new Bok(bøker.Count, nyBok.Tittel, nyBok.Forfatter, nyBok.År));
        }
        public DateTime BokAvailability(int bokId)
        {
            var bok = (from b in bøker where b.Id == bokId select b).SingleOrDefault();
            if (bok != null)
            {
                return bok.Lån.Slutt;
            }
            
            return DateTime.Now;
        }

        public int BookTitleCount(string tittel)
        {
            return (from bok in bøker where bok.Tittel == tittel select bok).Count();
        }
        
        public void LånBok(int bokId, int brukerId)
        {
            var query = from bok in bøker where bok.Id == bokId select bok;
            foreach(Bok bok in query)
            {
                if(bok.Lån.Slutt <= DateTime.Now)
                {
                    bok.Lån = new Utlån(bokId, brukerId, 30);
                }
            }
        }
        public void ReturnerBok(int bokId)
        {
            var bok = (from b in bøker where b.Id == bokId select b).SingleOrDefault();
            if(bok != null)
            {
                bok.Lån.Slutt = DateTime.Now;
                bok.Historikk.Add(bok.Lån);
            }
        }
        public bool BookExists(int bokId)
        {
            var query = from bok in bøker where bok.Id == bokId select bok;
            if (query.Any())
            {
                return true;
            }
            return false;
        }
        public List<Bok> GetAktiveUtlåner()
        {
            List<Bok> aktiveUtlån = new List<Bok>();
            var query = from bok in bøker where bok.Lån.Slutt > DateTime.Now select bok;
            foreach(Bok bok in query)
            {
                aktiveUtlån.Add(bok);
            }
            return aktiveUtlån;
        }
        public List<Utlån> GetHistorikk(int bokId)
        {
            var bok = (from b in bøker where b.Id == bokId select b).SingleOrDefault();
            if(bok != null)
            {
                return bok.Historikk;
            }
            return new List<Utlån>();
        }
        public Bibliotek()
        {
            bøker = new List<Bok> { new Bok(1, "Around the World in 80 Days", "Mark Beaumont", 2018), new Bok(2, "Guds Smugler", "Broder Andreas", 2020), new Bok(3, "Gezien, Gekend, Geliefd", "Gary Chapman & York Moore", 2021), new Bok(4, "In Quest of the Historical Adam", "William Lane Craig", 2021) };
        }
    }
}
