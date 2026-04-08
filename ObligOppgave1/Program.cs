using System.Dynamic;
using System.Threading.Tasks.Dataflow;

namespace ObligOppgave1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Velkommen til konsollapplikasjon for registrering og håndtering av kurs, bibliotek og brukere!\n");
            Console.WriteLine("\nHer er menyen:\n\n[1] Opprett kurs\n[2] Meld student til kurs\n[3] Skriv ut kurs og deltagere\n[4] Søk på kurs\n[5] Søk på bok\n[6] Lån bok\n[7] Returner bok\n[8] Registrer bok\n[9] Vis alternativer\n[0] Avslutt");

            Skole skole = new Skole();
            Bibliotek biep = new Bibliotek();

            skole.AddBruker(new Student(1, "Rune", "rjliefting@uia.no"));
            skole.AddKurs(new Kurs(110, "Objektorientert Programmering", 10, 150));

            int studentId;
            int valg = -1;
            while (valg != 0)
            {
                valg = GetIntInput("\nVelg alternativ (skriv 9 for å få opp menyen):\n");

                switch (valg)
                {
                    case 1:
                        {
                            Console.WriteLine("\n[1] OPPRETT KURS\n\nFor å opprette et nytt kurs, trenger vi litt informasjon om kurset.\n");

                            int kursId = GetIntInput("Kurs-ID:", 1);

                            while (kursId < 1 | skole.KursExists(kursId))
                            {
                                Console.WriteLine("\nUgyldig inndata, eller et kurs med samme ID er allerede registrert (" + skole.GetKursById(kursId).Navn + "). Velg annen ID:\n");
                            }


                            Console.WriteLine("\nKursnavn: ");
                            string kursNavn = Console.ReadLine()!;
                            while (kursNavn.Length == 0 | skole.KursExists(kursNavn))
                            {
                                Console.WriteLine("\nUgyldig inndata, eller dette kursnavnet er allerede tatt. Velg et annet navn.\n");
                            }

                            int kursPoeng = GetIntInput("\nHvor mange studiepoeng har dette kurset:\n", 0);

                            int kursKapasitet = GetIntInput("\nHvor mange studenter har kurset plass til?\n", 1);

                            Kurs nyttKurs = new Kurs(kursId, kursNavn, kursPoeng, kursKapasitet);
                            skole.AddKurs(nyttKurs);

                            Console.WriteLine("\nKurset er registrert!");
                            break;
                        }
                
                    case 2:
                        {
                            if (skole.GetKurser().Count == 0)
                            {
                                Console.WriteLine("Ingen kurs registert enda. Registrer kurs før du melder opp studenter til kurs");
                                break;
                            }

                            Console.WriteLine("\n[2] PÅMELDING STUDENT TIL KURS\n");
                            int kursId = GetIntInput("Skriv inn ID på kurs som du ønsker å legge en student til:");

                            Kurs kurs = skole.GetKursById(kursId);
                            if (kurs == null)
                            {
                                Console.WriteLine("\nDette kurset finnes ikke. Prøv på nytt.\n");
                                break;
                            }
                            
                            studentId = GetIntInput("\nRegistrer student til " + kurs.Navn + ". Du kan skrive inn StudentID på eksisterende studenter, eller opprette ny student med ny StudentID:");

                            Student student = (Student)skole.GetBrukerById(studentId);
                            if (student != null)
                            {
                                if (kurs.Studenter.Contains(student))
                                {
                                    Console.WriteLine("\nDenne studenten er allerede påmeldt til dette kurset.");
                                    break;
                                }

                                else if (kurs.Kapasitet <= kurs.Studenter.Count)
                                {
                                    Console.WriteLine("\nDette kurset er dessverre fullt, og kan ikke ta imot flere. Studenten er ikke meldt til kurset.\n");
                                    break;
                                }

                                kurs.Studenter.Add(student);
                                Console.WriteLine("\nStudent " + student.Navn + " har blitt lagt til kurs " + kurs.Navn);
                            }
                            else
                            {
                                Console.WriteLine("\nStudent med denne ID finnes ikke enda. Vi trenger litt info for å registere den:\n");

                                Console.WriteLine("\nNavn på ny student:\n");

                                string studentNavn = Console.ReadLine()!;

                                Console.WriteLine("\nStudenten sin epost:\n");

                                string studentEpost = Console.ReadLine()!;

                                Console.WriteLine("\nHusk å registrere hvilke andre kurser denne studenten tar.\n");


                                Console.WriteLine("\nEr denne studenten utvekslingsstudent (j/n)?\n");
                                string svar = Console.ReadLine()!;
                                while (!new string[] { "j", "n" }.Contains(svar))
                                {
                                    Console.WriteLine("\nUgyldig svar. Prøv igjen:\n");
                                }

                                if (svar == "n")
                                {
                                    student = new Student(studentId, studentNavn, studentEpost);
                                    skole.AddBruker(student);

                                    if (kurs.Kapasitet > kurs.Studenter.Count)
                                    {
                                        kurs.Studenter.Add(student);
                                        student.Kurser.Add(kurs);
                                        Console.WriteLine("\nStudent " + student.Navn + " har blitt lagt til kurs " + kurs.Navn + "(" + kurs.Id + ")!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nDette kurset er dessverre fullt. Den nye studenten er blitt registert i systemet, men ikke til kurset.\n");
                                    }
                                }

                                else if (svar == "j")
                                {
                                    try
                                    {
                                        Console.WriteLine("\n\nDa trenger vi litt ekstra info om den nye studenten:\n");
                                        Console.WriteLine("\nHva er hjemuniversitetet til denne studenten?\n");
                                        string hjemUni = Console.ReadLine()!;
                                        Console.WriteLine("\n\nHva er hjemlandet til denne studenten?\n");
                                        string nasjon = Console.ReadLine()!;
                                        Console.WriteLine("\n\nHvilken dato begynner denne studenten som utvekslingsstudent?");
                                        int day = GetIntInput("Dag: ", 1, 31);
                                        int month = GetIntInput("Måned: ", 1, 12);
                                        int year = GetIntInput("År: ", 0000, 9999);

                                        DateOnly start = DateOnly.Parse(day + "/" + month + "/" + year);
                                        Console.WriteLine("\n\nHvilken dato slutter studenten som utvekslingsstudent?\n ");
                                        day = GetIntInput("Dag: ", 1, 31);
                                        month = GetIntInput("Måned: ", 1, 12);
                                        year = GetIntInput("År: ", 0000, 9999);
                                        DateOnly slutt = DateOnly.Parse(day + "/" + month + "/" + year);
                                        UtvStudent utvStudent = new UtvStudent(studentId, studentNavn, studentEpost, hjemUni, nasjon, start, slutt);
                                        skole.AddBruker(utvStudent);
                                        kurs.Studenter.Add(utvStudent);
                                        Console.WriteLine("\nStudenten er meldt opp til kurset!");
                                    }
                                    catch
                                    {
                                        Console.WriteLine("\nUgyldig inndata. Prøv på nytt.");
                                    }
                                }
                            }
                            
                            break;
                        }

                    case 3:
                        {
                            Console.WriteLine("\n[3] SKRIV UT ALLE KURS OG DERES DELTAGERE\n");

                            if(skole.GetKurser().Count == 0)
                            {
                                Console.WriteLine("Ingen kurs registert enda. Ingen data å vise.");
                            }
                            else
                            {
                                foreach (Kurs kurs in skole.GetKurser())
                                {
                                    Console.WriteLine("Studenter oppmeldt til " + kurs.Navn + " (" + kurs.Id + "): \n");
                                    if(kurs.Studenter.Count == 0)
                                    {
                                        Console.WriteLine("Ingen stundenter meldt opp til " + kurs.Navn);
                                    }
                                    else
                                    {
                                        foreach (Student student in kurs.Studenter)
                                        {
                                            Console.WriteLine(student.Navn + " (" + student.Id + ")");
                                        }
                                    }
                                    Console.WriteLine("\n\n");
                                }
                            }
                            break;
                        }

                    case 4:
                        {
                            Console.WriteLine("\n[4] SØK PÅ KURS\nSkriv Kurs-ID eller kursnavn\n");
                            string søk = Console.ReadLine()!;
                            List<Kurs> søkeresultater = skole.SearchKurser(søk);

                            if (søkeresultater.Count > 0)
                            {
                                foreach (Kurs treff in søkeresultater)
                                {
                                    Console.WriteLine("Søkeresultater:\n\n" + treff.Navn + "(" + treff.Id + "). \nStudenter oppmeldt på dette kurset:\n");
                                    if (treff.Studenter.Count == 0)
                                    {
                                        Console.WriteLine("Ingen studenter oppmeldt til dette kurset.");
                                    }
                                    else
                                    {
                                        foreach (Student student in treff.Studenter)
                                        {
                                            Console.WriteLine(student.Navn + " (" + student.Id + ")");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ingen søkeresultater basert på søkeordet \"" + søk + "\n");
                            }
                            break;
                        }

                    case 5:
                        {
                            Console.WriteLine("\n[5] SØK BOK\nSkriv tittel eller Bok-ID:\n");
                            string søk = Console.ReadLine()!;

                            List<Bok> søkeResultater = biep.SearchBøker(søk);
                            if (søkeResultater.Count == 0)
                            {
                                Console.WriteLine("\nIngen søkeresultater basert på søkeordet \"" + søk + "+\"");
                                break;
                            }

                            Console.WriteLine("Søkeresultater " + "(" + søkeResultater.Count + ")" + ":\n\n");
                            foreach (Bok treff in søkeResultater)
                            {
                                Console.WriteLine(treff.Tittel);
                                Console.WriteLine("Forfatter: " + treff.Forfatter);
                                Console.WriteLine("Utgivelsesår: " + treff.År);
                                Console.WriteLine("Bok-ID: " + treff.Id);
                                if (biep.BokAvailability(treff.Id) >= DateTime.Now)
                                {
                                    Console.WriteLine("Denne boka er utlånt.\n\n");
                                }
                                else
                                {
                                    Console.WriteLine("Denne boka er tilgjengelig for utlån.\n\n");
                                }
                            }
                            break;
                        }
                        
                    
                    case 6:
                        {
                            Console.WriteLine("\n[6] LÅN BOK\n");

                            int bokId = GetIntInput("\nSkriv ID på bok du ønsker å låne:");
                            if (!biep.BookExists(bokId))
                            {
                                Console.WriteLine("\nDenne boka finnes ikke.\n");
                                break;
                            }
                            if (biep.BokAvailability(bokId) >= DateTime.Now)
                            {
                                Console.WriteLine("\nDenne boka er dessverre utlånt. Den vil være tilgjengelig fra og med " + biep.BokAvailability(bokId));
                                break;
                            }
                            int brukerId = GetIntInput("\nDenne boka (" + biep.GetBok(bokId).Tittel + ") er tilgjengelig. Skriv din brukerID:");
                            if (!skole.BrukerExists(brukerId))
                            {
                                Console.WriteLine("\nBruker ikke funnet. Prøv på nytt.\n");
                                break;
                            }
                            biep.LånBok(bokId, brukerId);
                            Console.WriteLine("\nLån gjennomført! Frist for innlevering er " + biep.BokAvailability(bokId));
                            break;
                        }
                    
                    case 7:
                        {
                            Console.WriteLine("\n[7] RETURNER BOK");
                            int bokId = GetIntInput("\nSkriv ID på bok du ønsker å levere:");
                            biep.ReturnerBok(bokId);
                            Console.WriteLine("\nBok returnert!");
                            break;
                        }

                    case 8:
                        {
                            Console.WriteLine("\n[8] REGISTRER NY BOK\n");
                            Console.WriteLine("\nFor å registere ny bok, trenger vi litt info.\nHva er tittelen på boka?\n");
                            string tittel = Console.ReadLine()!;
                            while (tittel == "")
                            {
                                Console.WriteLine("\nUgyldig inndata. Skriv tittel på nytt:\n");
                                tittel = Console.ReadLine()!;
                            }

                            Console.WriteLine("\nHva hater forfatteren til boka?\n");
                            string forfatter = Console.ReadLine()!;
                            while (forfatter == "")
                            {
                                Console.WriteLine("\nUgyldig inndata. Skriv navn på forfatter på nytt:\n");
                                tittel = Console.ReadLine()!;
                            }

                            int år = GetIntInput("\nHvilket år ble boka publisert?\n");
                            biep.AddBok(new Bok(tittel, forfatter, år));

                            Console.WriteLine(tittel + " har blitt lagt til i vårt bibliotek!\n");
                            break;
                        }

                    case 9:
                        {
                            Console.WriteLine("\nHer er menyen:\n\n[1] Opprett kurs\n[2] Meld student til kurs\n[3] Skriv ut kurs og deltagere\n[4] Søk på kurs og deltagere\n[5] Søk på bok\n[6] Lån bok\n[7] Returner bok\n[8] Registrer bok\n[9] Vis alternativer\n[0]Avslutt");
                            break;
                        }

                    default:
                        Console.WriteLine("Ugyldig inndata");
                        break;
                }
                
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n");
            }
            Console.WriteLine("Sees neste gang!");
        }
        private static int GetIntInput(string msg)
        {
            Console.WriteLine(msg);

            bool loop = true;
            while (loop)
            {
                string input = Console.ReadLine()!;
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
                string input = Console.ReadLine()!;
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
                string input = Console.ReadLine()!;
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