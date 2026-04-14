using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave2
{
    public class Student : User
    {
        public List<Study> Grades { get; set; }

        public Student(string name, string email, int role) : base (name, email, role)
        {
            Grades = new List<Study>();
        }
        public Student(int id, string name, string email, int role, string password) : base (id, name, email, role, password)
        {
            Grades = new List<Study>();
        }
        public int GetGradeBycourseId(int courseId)
        {
            var grade = (from g in Grades where g.Course.Id == courseId select g).SingleOrDefault();
            if (grade != null)
            {
                return grade.Grade;
            }
            return -1;
        }
        public void EditGradeBycourseId(int courseId, int newGrade)
        {
            if(newGrade < 1 | newGrade > 10)
            {
                throw new Exception();
            }
            var grade = (from g in Grades where g.Course.Id == courseId select g).SingleOrDefault();
            if (grade != null)
            {
                grade.Grade = newGrade;
            }
        }
    }
}
