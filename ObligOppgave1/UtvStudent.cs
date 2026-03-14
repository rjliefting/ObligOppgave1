using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class UtvStudent : Student
    {
        public string HjemUniversitet { get; }
        public string Land { get; }
        public DateOnly StartDato {get; set; }
        public DateOnly SluttDato { get; set; }

        public UtvStudent(int id, string navn, string epost, string hjemUniversitet, string land, DateOnly startDato, DateOnly sluttDato) : base (id, navn, epost)
        {
            HjemUniversitet = hjemUniversitet;
            Land = land;
            StartDato = startDato;
            SluttDato = sluttDato;
        }
    }
}
