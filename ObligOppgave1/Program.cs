using System.Collections.Concurrent;
using System.Dynamic;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;

namespace ObligOppgave1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Velkommen til konsollapplikasjon for registrering og håndtering av kurs og bibliotek!\n");

            School school = new School();
            Library library = new Library();

            school.AddStudent(new Student("Rune", "rjliefting@uia.no", 1), "hei");
            school.AddEmployee(new Employee("Egil", "egilpost", "IT", 2), "hei");
            school.AddEmployee(new Employee("Kari", "kari@uia.no", "Bibliotek", 3), "hei");
            school.AddCourse(new Course(110, "Objektorientert Programmering", 10, 150, school.GetEmployeeById(1)));

            school.GetCourseById(110).Students.Add(school.GetStudentById(0));
            school.AddStudentToCourse(school.GetStudentById(0), school.GetCourseById(110));
            string password = "";
            if (GetIntInput("Ønsker du å logge inn(1) eller registrere ny bruker(2) ?\n", 1, 2) == 2)
            {
                Console.WriteLine("Velkommen som ny bruker! Vi trenger litt info for å registere deg inn i systemet.");
                int role = GetIntInput("Er du student (1), faglærer (2) eller bibliotekar (3)?\n", 1, 3);
                Console.WriteLine("Ditt navn:\n");
                string name = Console.ReadLine();
                Console.WriteLine("Din epost:\n");
                string email = Console.ReadLine();
                Console.WriteLine("Lag nytt passord:\n");
                password = Console.ReadLine();

                switch (role)
                {
                    case 1:
                        {
                            bool ans = YesOrNo("Er du utvekslingsstudent? (j/n):\n");

                            if (ans)
                            {
                                try
                                {
                                    Console.WriteLine("\n\nDa trenger vi litt ekstra info om deg:\n");
                                    Console.WriteLine("\nHva er hjemuniversitetet ditt?\n");
                                    string homeUni = Console.ReadLine();
                                    Console.WriteLine("\n\nHva er hjemlandet ditt?\n");
                                    string nation = Console.ReadLine();
                                    Console.WriteLine("\n\nHvilken dato begynner du som utvekslingsstudent?");
                                    int day = GetIntInput("Dag: ", 1, 31);
                                    int month = GetIntInput("Måned: ", 1, 12);
                                    int year = GetIntInput("År: ", 0000, 9999);

                                    DateOnly start = DateOnly.Parse(day + "/" + month + "/" + year);
                                    Console.WriteLine("\n\nHvilken dato slutter du som utvekslingsstudent?\n ");
                                    day = GetIntInput("Dag: ", 1, 31);
                                    month = GetIntInput("Måned: ", 1, 12);
                                    year = GetIntInput("År: ", 0000, 9999);
                                    DateOnly end = DateOnly.Parse(day + "/" + month + "/" + year);

                                    ExchStudent newUser = new ExchStudent(name, email, role, homeUni, nation, start, end);
                                    school.AddUtvStudent(newUser, password);
                                    Console.WriteLine("\nNy bruker registert. Din bruker-ID er: " + school.GetIdOfLastAddedUser());
                                    break;
                                }
                                catch
                                {
                                    Console.WriteLine("\nUgyldig inndata. Prøv på nytt.");
                                }
                            }
                            else
                            {
                                Student newUser = new Student(name, email, role);
                                school.AddStudent(newUser, password);
                                Console.WriteLine("\nNy bruker registert. Din bruker-ID er " + school.GetIdOfLastAddedUser());
                                break;
                            }
                            break;
                        }
                    case 2:
                    case 3:
                        {
                            Console.WriteLine("Hva er din avdeling?\n");
                            string department = Console.ReadLine();
                            Employee newUser = new Employee(name, email, department, role);
                            school.AddEmployee(newUser, password);
                            Console.WriteLine("\nNy user registert. Din bruker-ID er" + newUser.Id);
                            break;
                        }
                }
            }
            

            Console.WriteLine("\nLogg inn:\n");

            int userId = GetIntInput("Bruker-ID:\n");
            Console.WriteLine("Passord:\n");
            password = Console.ReadLine();

            while(!school.Login(userId, password))
            {
                Console.WriteLine("Feil bruker-ID eller passord. Prøv igjen.");
                userId = GetIntInput("Bruker-ID:\n");
                Console.WriteLine("Passord:\n");
                password = Console.ReadLine();
            }

            User user = school.GetUserById(userId);

            switch (user.Role)
            {
                case 1:
                    {
                        //student
                        Student student = school.GetStudentById(user.Id);
                        Console.WriteLine("Her er menyen til deg som student:\n[1] Påmelding/Avmelding til/fra kurs\n[2] Se dine kurser\n[3] Se dine karakterer\n[4] Låne/Levere bøker\n[5] Søke på bøker\n[0]Avslutt");
                        int valg = -1;
                        while (valg != 0)
                        {
                            valg = GetIntInput("Skriv valg:\n", 0, 5);
                            switch (valg)
                            {
                                case 1:
                                    {
                                        Console.WriteLine("Påmelding til/avmelding fra kurs\n");
                                        int courseId = GetIntInput("Skriv inn ID på kurs som du ønsker å melde deg til eller fra:\n");
                                        Course course = school.GetCourseById(courseId);
                                        if (course.Students.Contains(student))
                                        {
                                            if(YesOrNo("Ønsker du å melde deg av fra kurs " + course.Name + "? (j/n):\n"))
                                            {
                                                course.Students.Remove(student);
                                                Console.WriteLine("\nDu er blitt avmeldt fra kurset.\n");
                                                break;
                                            }
                                        }
                                        if(course.Capacity <= course.Students.Count)
                                        {
                                            Console.WriteLine("Dette kurset er dessverre fullt");
                                            break;
                                        }
                                        if(YesOrNo("Ønsker du å påmelde deg til " + course.Name + "? (j/n):\n"))
                                        {
                                            school.AddStudentToCourse(student, course);
                                            Console.WriteLine("\nDu er blitt påmeldt til kurset.\n");
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        Console.WriteLine("Se dine kurs\nDu er oppmeldt til følgende kurs:\n");
                                        foreach(Course c in school.GetCoursesByStudent(student))
                                        {
                                            Console.WriteLine(c.Name);
                                        }
                                        break;
                                    }
                                case 3:
                                    {
                                        Console.WriteLine("Se dine karakterer:\n");
                                        foreach(Study grade in student.Grades)
                                        {
                                            Console.WriteLine(grade.Course.Name + ": " + grade.Grade);
                                        }
                                        break;
                                    }
                                case 4:
                                    {
                                        Console.WriteLine("Låne/levere bøker\n");

                                        if(GetIntInput("Ønsker du å låne (1) eller levere (2) bok?:\n", 1, 2) == 2)
                                        {
                                            if (library.GetActiveLoansByUser(student).Count > 0)
                                            {
                                                Console.WriteLine("Dine aktive lån:\n");
                                                foreach (Book hits in library.GetActiveLoansByUser(student))
                                                {
                                                    PrintBook(hits);
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Du har ingen aktive lån");
                                            }
                                            int retur = GetIntInput("ID på bok: \n");
                                            if(!library.BookExists(retur) | !library.GetActiveLoansByUser(student).Contains(library.GetBookById(retur)))
                                            {
                                                Console.WriteLine("Denne boka finnes ikke eller er ikke lånt av deg. Prøv på nytt");
                                                break;
                                            }
                                            library.ReturnBook(retur);
                                            Console.WriteLine("Bok levert.");
                                            break;
                                        }
                                        

                                        int bookId = GetIntInput("Skriv ID på bok som du ønsker å låne:\n");
                                        if (!library.BookExists(bookId))
                                        {
                                            Console.WriteLine("Denne boka finnes ikke i vårt system");
                                            break;
                                        }
                                        if(library.BookAvailability(bookId) > DateTime.Now)
                                        {
                                            Console.WriteLine("Denne boka er dessverre ikke tilgjengelig nå. Den blir tilgjengelig fra og med " + library.BookAvailability(bookId));
                                            break;
                                        }
                                        library.LoanBook(bookId, student.Id);
                                        Console.WriteLine("Lån opprettet. Fristen for å levere er: " + library.BookAvailability(bookId));
                                        break;
                                    }
                                case 5:
                                    {
                                        Console.WriteLine("Søk på bøker.\nSkriv søkeord (Navn eller ID på bok):\n");
                                        string search = Console.ReadLine();
                                        SearchBooks(library, search);
                                        break;
                                    }
                                case 0:
                                    {
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Ugyldig inndata");
                                    break;
                            }
                            
                        }
                        break;
                    }
                case 2:
                    {
                        //faglærer
                        Employee employee = school.GetEmployeeById(userId);
                        Console.WriteLine("Her er menyen din som faglærer:\n[1] Opprette kurs\n[2] Søke på kurs\n[3] Sette karakterer\n[4] Registere pensum til kurs\n[5] Låne/Levere bøker\n[6] Søke på bøker\n[0] Avslutt");
                        int valg = -1;
                        while (valg != 0)
                        {
                            valg = GetIntInput("Skriv valg:\n", 0, 5);
                            switch (valg)
                            {
                                case 1:
                                    {
                                        Console.WriteLine("Opprett course");
                                        int courseId = GetIntInput("course-ID:", 1);

                                        while (courseId < 1 | school.CourseExists(courseId))
                                        {
                                            Console.WriteLine("\nUgyldig inndata, eller et course med samme ID er allerede registrert (" + school.GetCourseById(courseId).Name + ").\n");
                                            courseId = GetIntInput("course-ID:", 1);
                                        }


                                        Console.WriteLine("\nKursnavn: ");
                                        string courseName = Console.ReadLine();
                                        while (courseName.Length == 0 | school.CourseExists(courseName))
                                        {
                                            Console.WriteLine("\nUgyldig inndata, eller dette kursnavnet er allerede tatt. Velg et annet navn.\n");
                                        }

                                        int coursePoints = GetIntInput("\nHvor mange studiepoeng har dette kurset:\n", 0);

                                        int courseCapasity = GetIntInput("\nHvor mange studenter har kurset plass til?\n", 1);

                                        Course newCourse = new Course(courseId, courseName, coursePoints, courseCapasity, school.GetEmployeeById(employee.Id));
                                        school.AddCourse(newCourse);

                                        Console.WriteLine("\ncourseet er registrert!");
                                        break;
                                    }
                                case 2:
                                    {
                                        Console.WriteLine("\n[4] SØK PÅ course\nSkriv course-ID eller coursenavn\n");
                                        string search = Console.ReadLine();
                                        List<Course> result = school.SearchCourses(search);

                                        if (result.Count > 0)
                                        {
                                            foreach (Course hits in result)
                                            {
                                                Console.WriteLine("Søkeresultater:\n\n" + hits.Name + "(" + hits.Id + "). \nStudenter oppmeldt på dette kurset:\n");
                                                if (hits.Students.Count == 0)
                                                {
                                                    Console.WriteLine("Ingen studenter oppmeldt til dette kurset.");
                                                }
                                                else
                                                {
                                                    foreach (Student student in hits.Students)
                                                    {
                                                        Console.WriteLine(student.Name + " (" + student.Id + ")");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Ingen søkeresultater basert på søkeordet \"" + search + "\n");
                                        }
                                        break;
                                    }
                                case 3:
                                    {
                                        Console.WriteLine("Sette karakterer");
                                        Course course = school.GetCourseById(GetIntInput("Skriv ID på kurs som du vil sette karakter i:\n"));
                                        foreach(Student stu in course.Students)
                                        {
                                            Console.WriteLine("\nID: " + stu.Id);
                                            Console.WriteLine("Navn: " + stu.Name);
                                            Console.WriteLine("Karakter: " + stu.GetGradeBycourseId(course.Id));
                                        }
                                        Student stud = school.GetStudentById(GetIntInput("Skriv ID på student for å endre karakter:\n"));
                                        if(stud == null)
                                        {
                                            Console.WriteLine("Ugyldig inndata, eller student finnes ikke");
                                            break;
                                        }
                                        if(!course.Students.Contains(stud))
                                        {
                                            Console.WriteLine("Denne studenten er ikke påmeldt dette courseet.");
                                            break;
                                        }
                                        Console.WriteLine("\nID: " + stud.Id);
                                        Console.WriteLine("Navn: " + stud.Name);
                                        Console.WriteLine("Karakter: " + stud.GetGradeBycourseId(course.Id));

                                        stud.EditGradeBycourseId(course.Id, GetIntInput("Skriv ny karakter:\n", 1, 10));
                                        Console.WriteLine("Ny karakter satt");
                                        break;
                                    }
                                case 4:
                                    {
                                        Console.WriteLine("Registrere pensum til kurs");
                                        Course course = school.GetCourseById(GetIntInput("Skriv ID på kurs for å registrere pensum:\n"));
                                        if(course == null)
                                        {
                                            Console.WriteLine("Dette kurset finnes ikke.");
                                            break;
                                        }
                                        Console.WriteLine("Skriv pensum:\n");
                                        school.AddCurriculumToCourse(Console.ReadLine(), course);
                                        break;
                                    }
                                case 5:
                                    {
                                        Console.WriteLine("Låne/levere bøker\n");

                                        if (GetIntInput("Ønsker du å låne (1) eller levere (2) bok?:\n", 1, 2) == 2)
                                        {
                                            if (library.GetActiveLoansByUser(employee).Count > 0)
                                            {
                                                Console.WriteLine("Dine aktive lån:\n");
                                                foreach (Book treff in library.GetActiveLoansByUser(employee))
                                                {
                                                    PrintBook(treff);
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Du har ingen aktive lån");
                                            }
                                            int retur = GetIntInput("ID på bok: \n");
                                            if (!library.BookExists(retur) | !library.GetActiveLoansByUser(employee).Contains(library.GetBookById(retur)))
                                            {
                                                Console.WriteLine("Denne boka finnes ikke eller er ikke lånt av deg. Prøv på nytt");
                                                break;
                                            }
                                            library.ReturnBook(retur);
                                            Console.WriteLine("Bok levert.");
                                            break;
                                        }


                                        int bookId = GetIntInput("Skriv ID på bok som du ønsker å låne:\n");
                                        if (!library.BookExists(bookId))
                                        {
                                            Console.WriteLine("Denne boka finnes ikke i vårt system");
                                            break;
                                        }
                                        if (library.BookAvailability(bookId) > DateTime.Now)
                                        {
                                            Console.WriteLine("Denne boka er dessverre ikke tilgjengelig nå. Den blir tilgjengelig fra og med " + library.BookAvailability(bookId));
                                            break;
                                        }
                                        library.LoanBook(bookId, employee.Id);
                                        Console.WriteLine("Lån opprettet. Fristen for å levere er: " + library.BookAvailability(bookId));
                                        break;
                                    }
                                case 6:
                                    {
                                        Console.WriteLine("Søk på bøker.\nSkriv søkeord (Navn eller ID på bok):\n");
                                        string search = Console.ReadLine();
                                        SearchBooks(library, search);
                                        break;
                                    }
                                case 0:
                                    break;

                                default:
                                    Console.WriteLine("Ugyldig inndata");
                                    break;
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //bibliotekar
                        Console.WriteLine("Her er menyen din som bibliotekar:\n[1] Registere nye bøker\n[2] Se aktive lån\n[3] Se historikk til en bok\n[0] Avslutt");
                        int valg = -1;
                        while (valg != 0)
                        {
                            valg = GetIntInput("Skriv valg:\n", 0, 5);
                            switch (valg)
                            {
                                case 1:
                                    {
                                        Console.WriteLine("\nFor å registere ny bok, trenger vi litt info.\nHva er tittelen på boka?\n");
                                        string title = Console.ReadLine();
                                        while (title == "")
                                        {
                                            Console.WriteLine("\nUgyldig inndata. Skriv tittel på nytt:\n");
                                            title = Console.ReadLine();
                                        }

                                        Console.WriteLine("\nHva hater forfatteren til boka?\n");
                                        string author = Console.ReadLine();
                                        while (author == "")
                                        {
                                            Console.WriteLine("\nUgyldig inndata. Skriv navn på forfatter på nytt:\n");
                                            author = Console.ReadLine();
                                        }

                                        int year = GetIntInput("\nHvilket år ble boka publisert?\n");
                                        library.AddBook(new Book(title, author, year));

                                        Console.WriteLine(title + " har blitt lagt til i vårt bibliotek!\n");
                                        break;
                                    }
                                case 2:
                                    {
                                        Console.WriteLine("Se aktive lån");
                                        foreach(Book book in library.GetActiveLoans())
                                        {
                                            PrintBook(book);
                                        }
                                        break;
                                    }
                                case 3:
                                    {
                                        Console.WriteLine("Se historikk til bok");
                                        Book book = library.GetBookById(GetIntInput("Skriv ID på bok for å se historikk:\n"));
                                        if(book == null)
                                        {
                                            Console.WriteLine("Denne boka finnes ikke");
                                            break;
                                        }
                                        Console.WriteLine("Historikk til " + book.Title);
                                        foreach(Loan borrow in library.GetHistory(book.Id))
                                        {
                                            Console.WriteLine("Fra " + borrow.Start + " til " + borrow.End);
                                            Console.WriteLine("ble boka lånt ut av " + school.GetStudentById(borrow.UserId) + "(" + borrow.UserId + ")");
                                        }
                                        break;
                                    }
                                case 0:
                                    break;

                                default:
                                    Console.WriteLine("Ugyldig inndata");
                                    break;
                            }
                        }
                        break;
                    }
            }
        }
        private static bool YesOrNo (string msg)
        {
            Console.WriteLine(msg);
            string answer = Console.ReadLine();
            if(answer.ToLower() == "j")
            {
                return true;
            }
            return false;
        }
        private static void SearchBooks(Library library, string search)
        {
            List<Book> result = library.SearchBooks(search);
            if (result.Count == 0)
            {
                Console.WriteLine("\nIngen søkeresultater basert på søkeordet \"" + search + "+\"");
                return;
            }

            Console.WriteLine("Søkeresultater " + "(" + result.Count + ")" + ":\n\n");
            foreach (Book hits in result)
            {
                Console.WriteLine(hits.Title);
                Console.WriteLine("Forfatter: " + hits.Author);
                Console.WriteLine("Utgivelsesår: " + hits.Year);
                Console.WriteLine("Bok-ID: " + hits.Id);
                if (library.BookAvailability(hits.Id) >= DateTime.Now)
                {
                    Console.WriteLine("Denne boka er utlånt.\n\n");
                }
                else
                {
                    Console.WriteLine("Denne boka er tilgjengelig for utlån.\n\n");
                }
            }
        }
        private static void PrintBook(Book book)
        {
            Console.WriteLine(book.Title);
            Console.WriteLine("Forfatter: " + book.Author);
            Console.WriteLine("Utgivelsesår: " + book.Year);
            Console.WriteLine("Bok-ID: " + book.Id);
            Console.WriteLine("Innleveringsfrist: " + book.Loan.End);
        }
        private static int GetIntInput(string msg)
        {
            Console.WriteLine(msg);

            bool loop = true;
            while (loop)
            {
                string input = Console.ReadLine();
                try
                {
                    return int.Parse(input);
                }
                catch
                {
                    Console.WriteLine("\nUgyldig svar. Prøv igjen:\n");
                }
            }
            return -1;
        }
        private static int GetIntInput(string msg, int lower)
        {
            Console.WriteLine(msg);
            int res = 0;
            bool loop = true;
            while (loop)
            {
                string input = Console.ReadLine();
                try
                {
                    res = int.Parse(input);
                    if(res < lower)
                    {
                        Console.WriteLine("\n Ugyldig svar. Prøv igjen:\n");
                    }
                    else
                    {
                        return res;
                    }
                }
                catch
                {
                    Console.WriteLine("\nUgyldig svar. Prøv igjen:\n");
                }
            }
            return -1;
        }
        private static int GetIntInput(string msg, int lower, int upper)
        {
            Console.WriteLine(msg);
            int res = 0;
            bool loop = true;
            while (loop)
            {
                string input = Console.ReadLine();
                try
                {
                    res = int.Parse(input);
                    if (res < lower | res > upper)
                    {
                        Console.WriteLine("\n Ugyldig svar. Prøv igjen:\n");
                    }
                    else
                    {
                        return res;
                    }
                }
                catch
                {
                    Console.WriteLine("\nUgyldig svar. Prøv igjen:\n");
                }
            }
            return -1;
        }
    }
}