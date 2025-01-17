namespace UniversityLibrary.Test
{
    using NUnit.Framework;
    public class Tests
    {
        private UniversityLibrary library;
        private TextBook book1;
        private TextBook book2;
        private TextBook book3;
        private TextBook book4;

        [SetUp]
        public void Setup()
        {
            library = new UniversityLibrary();
            book1 = new TextBook("Title 1", "Author A", "Science");
            book2 = new TextBook("Title 2", "Author B", "Tech");
            book3 = new TextBook("Title 3", "Author C", "Action");
            book4 = new TextBook("Title 4", "Author D", "History");
        }

        [Test]
        public void LibraryConstructorShouldInitializeCorrectly()
        {
            library = new UniversityLibrary();
            Assert.IsNotNull(library);
            Assert.IsNotNull(library.Catalogue);
        }

        [Test]
        public void LibraryCatalogueShouldBeZeroInitially()
        {
            int expectedCount = 0;
            Assert.That(expectedCount, Is.EqualTo(library.Catalogue.Count));
        }

        [Test]
        public void AddBookShouldReturnTheCorrectString()
        {
            int inventoryNum = library.Catalogue.Count + 1;
            book3.InventoryNumber = inventoryNum;
            string expected = book3.ToString();
            string actual = library.AddTextBookToLibrary(book3);
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void AddedTextBooksShouldExistInCollection()
        {
            library.AddTextBookToLibrary(book3);
            library.AddTextBookToLibrary(book4);
            Assert.IsTrue(library.Catalogue.Contains(book3));
            Assert.IsTrue(library.Catalogue.Contains(book4));
            int expectedCount = 2;
            Assert.That(expectedCount, Is.EqualTo(library.Catalogue.Count));
        }

        [Test]
        public void AddedTextbookShouldHaveTheCorrectInventoryNumber()
        {
            library.AddTextBookToLibrary(book1);
            library.AddTextBookToLibrary(book2);
            library.AddTextBookToLibrary(book3);
            library.AddTextBookToLibrary(book4);
            int expectedThirdNum = 3;
            int expectedFourthNum = 4;
            Assert.That(expectedThirdNum, Is.EqualTo(book3.InventoryNumber));
            Assert.That(expectedFourthNum, Is.EqualTo(book4.InventoryNumber));
        }

        [Test]
        public void LoanBookShouldReturnTheCorrectString()
        {
            library.AddTextBookToLibrary(book1);
            library.AddTextBookToLibrary(book2);
            library.AddTextBookToLibrary(book3);
            library.AddTextBookToLibrary(book4);
            string studentName = "Gosho";
            string expectedMsg = $"{book4.Title} loaned to {studentName}.";
            string actualMsg = library.LoanTextBook(4, studentName);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
            Assert.That(book4.Holder, Is.EqualTo(studentName));
        }

        [Test]
        public void LoanBookShouldReturnTheCorrectStringIsNotYetReturned()
        {
            library.AddTextBookToLibrary(book1);
            library.AddTextBookToLibrary(book2);
            string studentName = "Dimi";
            library.LoanTextBook(2, studentName);
            string expectedMsg = $"{studentName} still hasn't returned {book2.Title}!";
            string actualMsg = library.LoanTextBook(2, studentName);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
        }

        [Test]
        public void ReturnBookShouldReturnTheCorrectString()
        {
            library.AddTextBookToLibrary(book1);
            library.AddTextBookToLibrary(book2);
            library.AddTextBookToLibrary(book3);
            library.AddTextBookToLibrary(book4);
            library.LoanTextBook(1, "Dimi");
            library.LoanTextBook(4, "Pesho");
            string expectedMsg = $"{book4.Title} is returned to the library.";
            string actualMsg = library.ReturnTextBook(4);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
        }

        [Test]
        public void ReturnBookShouldSetHolderToEmptyString()
        {
            library.AddTextBookToLibrary(book1);
            library.AddTextBookToLibrary(book2);
            library.AddTextBookToLibrary(book3);
            library.AddTextBookToLibrary(book4);
            library.LoanTextBook(1, "Dimi");
            library.LoanTextBook(4, "Pesho");
            library.ReturnTextBook(1);
            library.ReturnTextBook(4);
            string expectedFirstHolder = string.Empty;
            string expectedFourthHolder = string.Empty;
            Assert.That(expectedFirstHolder, Is.EqualTo(book1.Holder));
            Assert.That(expectedFourthHolder, Is.EqualTo(book4.Holder));
        }
    }
}