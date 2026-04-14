using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Study
    {
        public Course Course { get; }
        public int Grade { get; set; }
        
        public Study (Course course)
        {
            Course = course;
            Grade = -1;
        }
    }
}
