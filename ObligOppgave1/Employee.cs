using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Employee : User
    {
        public string Department { get; set; }

        public Employee(string name, string email, string department, int role) : base(name, email, role)
        {
            Department = department;
        }
        public Employee(int id, string name, string email, string department, int role, string password) : base(id, name, email, role, password)
        {
            Department = department;
        }
    }
}
