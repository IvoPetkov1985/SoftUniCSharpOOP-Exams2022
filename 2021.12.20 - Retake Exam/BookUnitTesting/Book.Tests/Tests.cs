namespace Book.Tests
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class Tests
    {
        private Book book;

        [SetUp]
        public void Setup()
        {
            book = new("MyBook", "Author10");
        }

        [Test]
        public void ConstructorShouldWorkProperly()
        {
            Book book7 = new("MyName", "Author15");
            Assert.IsNotNull(book);
            string expectedTitle = "MyName";
            string expectedAuthor = "Author15";
            Assert.That(expectedTitle, Is.EqualTo(book7.BookName));
            Assert.That(expectedAuthor, Is.EqualTo(book7.Author));
            int expectedCount = 0;
            Assert.That(expectedCount, Is.EqualTo(book7.FootnoteCount));
        }

        [Test]
        public void BookNameShouldBeSetCorrectly()
        {
            string expected = "MyBook";
            string actual = book.BookName;
            Assert.That(expected, Is.EqualTo(actual));
        }

        [TestCase(null)]
        [TestCase("")]
        public void BookNameShouldThrowIfNameNotCorrect(string name)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => book = new(name, "Author12"), $"Invalid {nameof(book.BookName)}!");
        }

        [Test]
        public void AuthorShouldBeSetCorrectly()
        {
            string expected = "Author10";
            string actual = book.Author;
            Assert.That(expected, Is.EqualTo(actual));
        }

        [TestCase(null)]
        [TestCase("")]
        public void AuthorShouldThrowIfNotCorrectValue(string author)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => book = new("MyName", author), $"Invalid {nameof(book.Author)}!");
        }

        [Test]
        public void AddFootNoteShouldWorkCorrectly()
        {
            book.AddFootnote(1001, "Some text");
            book.AddFootnote(1002, "Another text");
            int expectedCount = 2;
            int actualCount = book.FootnoteCount;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public void AddFootNoteShouldThrowIfKeyExists()
        {
            book.AddFootnote(1010, "Unique text");
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => book.AddFootnote(1010, "Duplicated note"), "Footnote already exists!");
        }

        [Test]
        public void FindFootNoteShouldReturnTheCorrectString()
        {
            book.AddFootnote(1001, "Some text");
            book.AddFootnote(1002, "Another text");
            string expectedMsg = "Footnote #1001: Some text";
            string actualMsg = book.FindFootnote(1001);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
        }

        [Test]
        public void FindFootNoteShouldThrowIfKeyDoesNotExist()
        {
            book.AddFootnote(1001, "Some text");
            book.AddFootnote(1002, "Another text");
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => book.FindFootnote(810), "Footnote does not exists!");
        }

        [Test]
        public void AlterFootNoteShouldThrowIfKeyDoesNotExist()
        {
            book.AddFootnote(1001, "Some text");
            book.AddFootnote(1002, "Another text");
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => book.AlterFootnote(810, "Some new bla bla"), "Footnote does not exists!");
        }

        [Test]
        public void AlterFootNoteShouldWorkCorrectlyIfKeyExists()
        {
            book.AddFootnote(1001, "Some text");
            book.AddFootnote(1002, "Another text");
            book.AddFootnote(1914, "Levski1914");
            book.AlterFootnote(1914, "Levski Sofia");
            string expectedMsg = "Footnote #1914: Levski Sofia";
            string actualMsg = book.FindFootnote(1914);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
            int expectedCount = 3;
            int actualCount = book.FootnoteCount;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }
    }
}