using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Student : Bruker
    {
        public List<Kurs> Kurser { get; set; }

        public Student(int id, string navn, string epost) : base (id, navn, epost)
        {
            Kurser = new List<Kurs>();
        }
    }
}
