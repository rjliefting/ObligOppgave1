using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave2
{
    public class Book
    {
        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public int Year { get; set; }
        public Loan Loan { get; set; }
        public List<Loan> History { get; }

        public Book(int id, string title, string author, int year)
        {
            Id = id;
            Title = title;
            Author = author;
            Year = year;
            Loan = new Loan();
            History = new List<Loan>();
        }
        public Book(string title, string author, int year)
        {
            Title = title;
            Author = author;
            Year = year;
            Loan = new Loan();
            History = new List<Loan>();
        }
    }
}
