using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class School
    {
        private List<User> users;
        private List<Course> courses;

        public School()
        {
            users = new List<User>();
            courses = new List<Course>();
        }
        public User GetUserById(int userId)
        {
            var user = (from b in users where b.Id == userId select b).SingleOrDefault();
            if(user != null)
            {
                return user;
            }
            return null;
        }
        public Student GetStudentById(int userId)
        {
            var user = (from b in users where b.Id == userId select b).SingleOrDefault();
            if (user != null)
            {
                return (Student)user;
            }
            return null;
        }

        public ExchStudent GetExchStudentById(int userId)
        {
            var user = (from b in users where b.Id == userId select b).SingleOrDefault();
            if (user != null)
            {
                return (ExchStudent)user;
            }
            return null;
        }
        public Employee GetEmployeeById(int userId)
        {
            var user = (from b in users where b.Id == userId select b).SingleOrDefault();
            if (user != null)
            {
                return (Employee)user;
            }
            return null;
        }
        public void AddEmployee(Employee newEmployee, string password)
        {
            if (UserExists(newEmployee.Id))
            {
                throw new Exception();
            }
            else
            {
                users.Add(new Employee(users.Count, newEmployee.Name, newEmployee.Email, newEmployee.Department, newEmployee.Role, password));
            }
        }
        public void AddStudent(Student newStudent, string password)
        {
            if (UserExists(newStudent.Id))
            {
                throw new Exception();
            }
            else
            {
                users.Add(new Student(users.Count, newStudent.Name, newStudent.Email, newStudent.Role, password));
            }
        }
        public void AddUtvStudent(ExchStudent newExchStudent, string password)
        {
            if (UserExists(newExchStudent.Id))
            {
                throw new Exception();
            }
            else
            {
                users.Add(new ExchStudent(users.Count, newExchStudent.Name, newExchStudent.Email, newExchStudent.Role, newExchStudent.HomeUniversity, newExchStudent.Nation, newExchStudent.StartDate, newExchStudent.EndDate, password));
            }
        }
        public int GetIdOfLastAddedUser()
        {
            return users.Count - 1;
        }
        public void AddStudentToCourse(Student student, Course course)
        {
            course.Students.Add(student);
            student.Grades.Add(new Study(course));
        }
        public bool UserExists(int userId)
        {
            var query = from user in users where user.Id == userId select user;
            if (query.Any())
            {
                return true;
            }
            return false;
        }
        public Course GetCourseById(int courseId)
        {
            var course = (from c in courses where c.Id == courseId select c).SingleOrDefault();
            if(course != null)
            {
                return course;
            }
            return null;
        }
        public List<Course> GetCourses()
        {
            return courses;
        }
        public List<Course> GetCoursesByStudent(Student student)
        {
            List<Course> result = new List<Course>();
            var query = from c in courses where c.Students.Contains(student) select c;
            foreach (Course course in query)
            {
                result.Add(course);
            }

            return result;
        }
        public List<Course> GetCoursesByEmployee(Employee employee)
        {
            List<Course> result = new List<Course>();
            var query = from c in courses where c.Students.Contains(employee) select c;
            foreach (Course course in query)
            {
                result.Add(course);
            }

            return result;
        }
        public void RemoveStudentFromCourse(int studentId, int courseId)
        {
            var course = (from c in courses where c.Id == courseId select c).SingleOrDefault();
            if(course != null)
            {
                var student = (from s in course.Students where s.Id == studentId select s).SingleOrDefault();
                course.Students.Remove(student);
            }
        }
        public List<Course> SearchCourses(string search)
        {
            List<Course> searchResult = new List<Course>();
            if (int.TryParse(search, out int result))
            {
                var course = (from c in courses where c.Id == result select c).SingleOrDefault();
                if (course != null)
                {
                    searchResult.Add(course);
                }
            }

            var query = from course in courses where course.Name.ToLower().Contains(search.ToLower()) select course;
            foreach(Course course in query)
            {
                searchResult.Add(course);
            }

            return searchResult;
        }
        public void AddCourse(Course newCourse)
        {
            if (CourseExists(newCourse.Id))
            {
                throw new Exception();
            }
            else
            {
                courses.Add(newCourse);
            }
        }
        public bool CourseExists(int courseId)
        {
            var query = from course in courses where course.Id == courseId select course;
            if (query.Any())
            {
                return true;
            }
            return false;
        }
        public bool CourseExists(string courseNavn)
        {
            var query = from course in courses where course.Name == courseNavn select course;
            if (query.Any())
            {
                return true;
            }
            return false;
        }
        public void AddCurriculumToCourse(string newCurriculum, Course course)
        {
            course.AddCurriculum(newCurriculum);
        }
        public bool Login(int userId, string password)
        {
            return GetUserById(userId).Login(password);
        }
    }
}
