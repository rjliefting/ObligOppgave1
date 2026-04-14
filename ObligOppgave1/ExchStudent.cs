using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class ExchStudent : Student
    {
        public string HomeUniversity { get; }
        public string Nation { get; }
        public DateOnly StartDate {get; set; }
        public DateOnly EndDate { get; set; }

        public ExchStudent(string name, string email, int role, string homeUni, string nation, DateOnly startDate, DateOnly endDate) : base (name, email, role)
        {
            HomeUniversity = homeUni;
            Nation = nation;
            StartDate = startDate;
            EndDate = endDate;
        }
        public ExchStudent(int id, string name, string email, int role, string homeUni, string nation, DateOnly startDate, DateOnly endDate, string password) : base(id, name, email, role, password)
        {
            HomeUniversity = homeUni;
            Nation = nation;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
