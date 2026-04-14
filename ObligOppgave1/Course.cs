using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave2
{
    public class Course
    {
        public int Id { get; }
        public string Name { get; }
        public int StudyPoints { get; }
        public int Capacity { get; }
        public List<User> Students { get; }
        public Employee Tutor { get; set; }
        public string Curriculum { get; set; }

        public Course(int id, string name, int studypoints, int capacity, Employee employee)
        {
            Id = id;
            Name = name;
            StudyPoints = studypoints;
            Capacity = capacity;
            Students = new List<User>();
            Tutor = employee;
            Curriculum = "Ingen pensum registrert";
        }
        public void AddCurriculum(string newCurriculum)
        {
            Curriculum = newCurriculum;
        }
    }
}
