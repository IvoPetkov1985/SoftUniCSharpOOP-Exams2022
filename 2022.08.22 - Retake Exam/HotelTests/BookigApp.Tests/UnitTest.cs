using FrontDeskApp;
using NUnit.Framework;
using System;
using System.Linq;

namespace BookigApp.Tests
{
    public class Tests
    {
        private Hotel hotel14;
        private Room room810;
        private Room room811;
        private Room room812;

        [SetUp]
        public void Setup()
        {
            hotel14 = new("Vega", 3);
            room810 = new(3, 75);
            room811 = new(2, 50);
            room812 = new(4, 88);
        }

        [Test]
        public void HotelAndCollectionsShouldNotBeNullUponInitialization()
        {
            Hotel hotel = new("California", 4);
            Assert.IsNotNull(hotel);
            Assert.IsNotNull(hotel.Bookings);
            Assert.IsNotNull(hotel.Rooms);
        }

        [Test]
        public void HotelConstructorShouldSetTheCorrectValues()
        {
            Hotel hotel = new("Ramada", 5);
            string expectedName = "Ramada";
            int expectedCategory = 5;
            Assert.That(expectedName == hotel.FullName);
            Assert.That(expectedCategory == hotel.Category);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        public void FullNameShouldThrowIfNullOrEmpty(string name)
        {
            Hotel hotel;
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(()
                => hotel = new(name, 4));
        }

        [TestCase("Ramada")]
        [TestCase("Serdika")]
        [TestCase("Astoria")]
        public void FullNameShouldSetTheCorrectValue(string name)
        {
            Hotel hotel = new(name, 5);
            Assert.That(name == hotel.FullName);
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        public void CategoryShouldSetTheCorrectValue(int category)
        {
            Hotel hotel = new("Berlin", category);
            Assert.That(category == hotel.Category);
        }

        [TestCase(0)]
        [TestCase(6)]
        [TestCase(12)]
        [TestCase(-2)]
        public void CategoryShouldThrowIfIncorrectInput(int category)
        {
            Hotel hotel;
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => hotel = new("Trimonzium", category));
        }

        [Test]
        public void TurnoverShouldBeInitiallyZero()
        {
            double expectedValue = 0;
            Assert.That(expectedValue == hotel14.Turnover);
        }

        [Test]
        public void AddRoomShouldWorkCorrectly()
        {
            hotel14.AddRoom(room811);
            hotel14.AddRoom(room812);
            int expectedCount = 2;
            Assert.That(expectedCount == hotel14.Rooms.Count);
            Assert.IsTrue(hotel14.Rooms.Contains(room811));
        }

        [Test]
        public void BookRoomShouldWorkCorrectly()
        {
            hotel14.AddRoom(room810);
            hotel14.AddRoom(room811);
            hotel14.AddRoom(room812);
            hotel14.BookRoom(2, 1, 2, 160);
            double expectedTurnover = 150;
            Assert.That(expectedTurnover == hotel14.Turnover);
            Assert.IsTrue(hotel14.Bookings.Any(b => b.BookingNumber == 1));
            Assert.IsTrue(hotel14.Bookings.Any(b => b.ResidenceDuration == 2));
            int expectedCount = 1;
            Assert.That(expectedCount == hotel14.Bookings.Count);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void BookRoomShouldThrowIfAdultsCountNotPositive(int count)
        {
            hotel14.AddRoom(room810);
            hotel14.AddRoom(room812);
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => hotel14.BookRoom(count, 2, 2, 300));
        }

        [TestCase(-1)]
        [TestCase(-4)]
        public void BookRoomShouldThrowIfChildrenCountIsNegative(int count)
        {
            hotel14.AddRoom(room810);
            hotel14.AddRoom(room811);
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => hotel14.BookRoom(2, count, 2, 310));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-15)]
        public void BookRoomShouldThrowIfDurationNotPositive(int count)
        {
            hotel14.AddRoom(room810);
            hotel14.AddRoom(room811);
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => hotel14.BookRoom(2, 1, count, 220));
            int expectedBookings = 0;
            int expectedTurnover = 0;
            Assert.That(expectedBookings == hotel14.Bookings.Count);
            Assert.That(expectedTurnover == hotel14.Turnover);
        }

        [Test]
        public void BookRoomShouldWorkCorrectlyIfBookingNotSuccessful()
        {
            hotel14.AddRoom(room810);
            hotel14.AddRoom(room811);
            hotel14.AddRoom(room812);
            hotel14.BookRoom(3, 4, 3, 1000);
            hotel14.BookRoom(1, 1, 3, 40);
            hotel14.BookRoom(2, 0, 10, 1000);
            int expectedCount = 3;
            Assert.That(expectedCount == hotel14.Bookings.Count);
        }
    }
}