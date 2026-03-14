using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Kurs
    {
        public int Id { get; }
        public string Navn { get; }
        public int StudiePoeng { get; }
        public int Kapasitet { get; }
        public List<Student> Studenter { get; }

        public Kurs(int id, string navn, int studiePoeng, int kapasitet)
        {
            Id = id;
            Navn = navn;
            StudiePoeng = studiePoeng;
            Kapasitet = kapasitet;
            Studenter = new List<Student>();
        }
    }
}
