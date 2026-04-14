using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObligOppgave1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave1.Tests
{
    [TestClass]
    public class LibraryTests
    {
        private Library _library;

        [TestInitialize]
        public void Setup()
        {
            // Start each test with a fresh library
            _library = new Library();
        }

        [TestMethod()]
        public void AddBook_ShouldIncreaseCount()
        {
            // Arrange
            var newBook = new Book(0, "Test Title", "Test Author", 2023);
            int initialCount = _library.BookTitleCount("Test Title");

            // Act
            _library.AddBook(newBook);

            // Assert
            Assert.AreEqual(initialCount + 1, _library.BookTitleCount("Test Title"));
        }

        [TestMethod()]
        public void GetBookById_ExistingId_ShouldReturnBook()
        {
            // Act
            var book = _library.GetBookById(1); // ID 1 exists in your constructor

            // Assert
            Assert.IsNotNull(book);
            Assert.AreEqual("Around the World in 80 Days", book.Title);
        }

        [TestMethod()]
        public void GetBookById_NonExistingId_ShouldReturnNull()
        {
            // Act
            var book = _library.GetBookById(999);

            // Assert
            Assert.IsNull(book);
        }

        [TestMethod()]
        public void SearchBooks_PartialTitle_ShouldReturnResults()
        {
            // Act
            var results = _library.SearchBooks("Guds");

            // Assert
            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.Any(b => b.Title.Contains("Guds")));
        }

        [TestMethod()]
        public void BookAvailability_ValidBook_ShouldReturnDateTime()
        {
            // Act
            var date = _library.BookAvailability(1);

            // Assert
            // Since you initialize Loan in the constructor, it should return a date
            Assert.IsInstanceOfType(date, typeof(DateTime));
        }
    }
}