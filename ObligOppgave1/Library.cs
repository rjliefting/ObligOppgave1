using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1
{
    public class Library
    {
        private List<Book> books;

        public List<Book> SearchBooks(string search)
        {
            List<Book> results = new List<Book>();
            if (int.TryParse(search, out int result))
            {
                var book = (from b in books where b.Id == result select b).SingleOrDefault();
                if(book != null)
                {
                    results.Add(book);
                }
            }

            var titleQuery = from book in books where book.Title.ToLower().Contains(search.ToLower()) select book;
            foreach (Book book in titleQuery)
            {
                results.Add(book);
            }
            return results;
        }

        public Book GetBookById(int bokId)
        {
            var book = (from b in books where b.Id == bokId select b).SingleOrDefault();
            if (book != null)
            {
                return book;
            }
            return null;
        }
        public void AddBook(Book newBook)
        {
            books.Add(new Book(books.Count, newBook.Title, newBook.Author, newBook.Year));
        }
        public DateTime BookAvailability(int bookId)
        {
            var book = (from b in books where b.Id == bookId select b).SingleOrDefault();
            if (book != null)
            {
                return book.Loan.End;
            }
            
            return DateTime.Now;
        }

        public int BookTitleCount(string title)
        {
            return (from book in books where book.Title == title select book).Count();
        }
        
        public void LoanBook(int bookId, int userId)
        {
            var query = from book in books where book.Id == bookId select book;
            foreach(Book book in query)
            {
                if(book.Loan.End <= DateTime.Now)
                {
                    book.Loan = new Loan(bookId, userId, 30);
                }
            }
        }
        public void ReturnBook(int bookId)
        {
            var book = (from b in books where b.Id == bookId select b).SingleOrDefault();
            if(book != null)
            {
                book.Loan.End = DateTime.Now;
                book.History.Add(book.Loan);
            }
        }
        public bool BookExists(int bookId)
        {
            var query = from book in books where book.Id == bookId select book;
            if (query.Any())
            {
                return true;
            }
            return false;
        }
        public List<Book> GetActiveLoans()
        {
            List<Book> activeLoans = new List<Book>();
            var query = from book in books where book.Loan.End > DateTime.Now select book;
            foreach(Book book in query)
            {
                activeLoans.Add(book);
            }
            return activeLoans;
        }
        public List<Book> GetActiveLoansByUser(User user)
        {
            List<Book> activeLoans = new List<Book>();
            var query = from book in books where book.Loan.End > DateTime.Now & book.Loan.UserId == user.Id select book;
            foreach (Book book in query)
            {
                activeLoans.Add(book);
            }
            return activeLoans;
        }
        public List<Loan> GetHistory(int bookId)
        {
            var book = (from b in books where b.Id == bookId select b).SingleOrDefault();
            if(book != null)
            {
                return book.History;
            }
            return new List<Loan>();
        }
        public Library()
        {
            books = new List<Book> { new Book(1, "Around the World in 80 Days", "Mark Beaumont", 2018), new Book(2, "Guds Smugler", "Broder Andreas", 2020), new Book(3, "Gezien, Gekend, Geliefd", "Gary Chapman & York Moore", 2021), new Book(4, "In Quest of the Historical Adam", "William Lane Craig", 2021) };
        }
    }
}
