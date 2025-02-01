using NUnit.Framework;
using System;

namespace SmartphoneShop.Tests
{
    [TestFixture]
    public class SmartphoneShopTests
    {
        private Shop shop;
        private Smartphone smartphone1;
        private Smartphone smartphone2;
        private Smartphone smartphone3;
        private Smartphone smartphone4;

        [SetUp]
        public void Setup()
        {
            shop = new(4);
            smartphone1 = new("Redmi A3", 5000);
            smartphone2 = new("Samsung A22", 5000);
            smartphone3 = new("Nokia 5", 4500);
            smartphone4 = new("Samsung A20E", 3200);
        }

        [TestCase(3)]
        [TestCase(11)]
        [TestCase(40)]
        public void ConstructorShouldInitializeProperly(int capacity)
        {
            shop = new(capacity);
            Assert.IsNotNull(shop);
            Assert.IsTrue(capacity == shop.Capacity);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(101)]
        public void CapacityShouldSetTheCorrectValue(int capacity)
        {
            shop = new(capacity);
            Assert.That(capacity, Is.EqualTo(shop.Capacity));
            int expectedCount = 0;
            Assert.That(expectedCount == shop.Count);
        }

        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(-11)]
        [TestCase(-2)]
        public void CapacityShouldThrowIdValueIsNegative(int capacity)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => shop = new(capacity), "Invalid capacity.");
        }

        [Test]
        public void AddMethodShouldIncreaseTheCounter()
        {
            shop.Add(smartphone1);
            shop.Add(smartphone3);
            int expectedCount = 2;
            int actualCount = shop.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public void AddMethodShouldThrowIfModelNameExists()
        {
            shop.Add(smartphone1);
            shop.Add(smartphone2);
            shop.Add(smartphone3);
            Smartphone smartphone5 = new("Nokia 5", 5500);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => shop.Add(smartphone5), "The phone model Nokia 5 already exist.");
        }

        [Test]
        public void AddMethodShouldThrowIfCapacityExceeded()
        {
            shop.Add(smartphone1);
            shop.Add(smartphone2);
            shop.Add(smartphone3);
            shop.Add(smartphone4);
            Smartphone smartphone6 = new("Huawai P6", 2300);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => shop.Add(smartphone6), "The shop is full.");
            int expectedCount = 4;
            int actualCount = shop.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public void RemoveMethodShouldWorkCorrectly()
        {
            shop.Add(smartphone1);
            shop.Add(smartphone2);
            shop.Add(smartphone3);
            shop.Add(smartphone4);
            shop.Remove("Nokia 5");
            int expectedCount = 3;
            int actualCount = shop.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }

        [Test]
        public void RemoveMethodShouldThrowIfModelDoesNotExist()
        {
            shop.Add(smartphone1);
            shop.Add(smartphone3);
            shop.Add(smartphone4);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => shop.Remove("Motorola 15"), "The phone model Motorola 15 doesn't exist.");
        }

        [Test]
        public void TestPhoneMethodShouldWorkCorrectly()
        {
            shop.Add(smartphone2);
            shop.Add(smartphone4);
            shop.TestPhone("Samsung A20E", 1200);
            int expectedBattery = 2000;
            int actualBattery = smartphone4.CurrentBateryCharge;
            Assert.That(expectedBattery, Is.EqualTo(actualBattery));
            shop.TestPhone("Samsung A22", 550);
            int expectedBattery2 = 4450;
            int actualBattery2 = smartphone2.CurrentBateryCharge;
            Assert.That(expectedBattery2, Is.EqualTo(actualBattery2));
        }

        [Test]
        public void TestPhoneShouldThrowIfModelDoesNotExist()
        {
            shop.Add(smartphone1);
            shop.Add(smartphone2);
            shop.Add(smartphone3);
            shop.Add(smartphone4);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => shop.TestPhone("Huawai Ascend", 310), "The phone model Huawai Ascend doesn't exist.");
        }

        [Test]
        public void TestPhoneShouldThrowIfTheBatteryIsLow()
        {
            shop.Add(smartphone3);
            shop.Add(smartphone4);
            shop.TestPhone("Samsung A20E", 1200);
            shop.TestPhone("Samsung A20E", 1000);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => shop.TestPhone("Samsung A20E", 1001), "The phone model Samsung A20E is low on batery.");
        }

        [Test]
        public void ChargePhoneShouldSetTheCurrentChargeToTheMaximum()
        {
            shop.Add(smartphone3);
            shop.Add(smartphone4);
            shop.TestPhone("Samsung A20E", 1200);
            shop.TestPhone("Samsung A20E", 2000);
            shop.ChargePhone("Samsung A20E");
            int expectedCharge = 3200;
            int actualCharge = smartphone4.CurrentBateryCharge;
            Assert.That(expectedCharge, Is.EqualTo(actualCharge));
            Assert.That(smartphone4.CurrentBateryCharge, Is.EqualTo(smartphone4.MaximumBatteryCharge));
        }

        [Test]
        public void ChargePhoneMethodShouldThrowIfModelNameIsInvalid()
        {
            shop.Add(smartphone1);
            shop.Add(smartphone2);
            shop.Add(smartphone4);
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                => shop.ChargePhone("Nokia Lumia 520"), "The phone model Nokia Lumia 520 doesn't exist.");
        }
    }
}
