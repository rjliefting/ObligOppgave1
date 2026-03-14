using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Skole
    {
        private List<Bruker> brukere;
        private List<Kurs> kurser;
        public Skole()
        {
            brukere = new List<Bruker>();
            kurser = new List<Kurs>();
        }
        public Bruker GetBrukerById(int brukerId)
        {
            var bruker = (from b in brukere where b.Id == brukerId select b).SingleOrDefault();
            if(bruker != null)
            {
                return bruker;
            }
            return null;
        }
        public void AddBruker(Bruker nyBruker)
        {
            if (BrukerExists(nyBruker.Id))
            {
                throw new Exception();
            }
            else
            {
                brukere.Add(nyBruker);
            }
        }
        public bool BrukerExists(int brukerId)
        {
            var query = from bruker in brukere where bruker.Id == brukerId select bruker;
            if (query.Any())
            {
                return true;
            }
            return false;
        }
        public Kurs GetKursById(int kursId)
        {
            var kurs = (from k in kurser where k.Id == kursId select k).SingleOrDefault();
            if(kurs != null)
            {
                return kurs;
            }
            return null;
        }
        public List<Kurs> GetKurser()
        {
            return kurser;
        }
        public void AvmeldStudent(int studentId, int kursId)
        {
            var kurs = (from k in kurser where k.Id == kursId select k).SingleOrDefault();
            if(kurs != null)
            {
                var student = (from s in kurs.Studenter where s.Id == studentId select s).SingleOrDefault();
                kurs.Studenter.Remove(student);
            }
        }
        public List<Kurs> SearchKurser(string søk)
        {
            List<Kurs> søkeresultater = new List<Kurs>();
            if (int.TryParse(søk, out int result))
            {
                var kurs = (from k in kurser where k.Id == result select k).SingleOrDefault();
                if (kurs != null)
                {
                    søkeresultater.Add(kurs);
                }
            }

            var query = from kurs in kurser where kurs.Navn.ToLower().Contains(søk.ToLower()) select kurs;
            foreach(Kurs kurs in query)
            {
                søkeresultater.Add(kurs);
            }

            return søkeresultater;
        }
        public void AddKurs(Kurs nyttKurs)
        {
            if (KursExists(nyttKurs.Id))
            {
                throw new Exception();
            }
            else
            {
                kurser.Add(nyttKurs);
            }
        }
        public bool KursExists(int kursId)
        {
            var query = from kurs in kurser where kurs.Id == kursId select kurs;
            if (query.Any())
            {
                return true;
            }
            return false;
        }
        public bool KursExists(string kursNavn)
        {
            var query = from kurs in kurser where kurs.Navn == kursNavn select kurs;
            if (query.Any())
            {
                return true;
            }
            return false;
        }
    }
}
