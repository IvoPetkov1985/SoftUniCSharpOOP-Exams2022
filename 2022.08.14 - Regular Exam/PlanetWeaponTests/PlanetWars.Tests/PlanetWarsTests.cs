using NUnit.Framework;
using System;
using System.Linq;

namespace PlanetWars.Tests
{
    public class Tests
    {
        [TestFixture]
        public class PlanetWarsTests
        {
            private Weapon weapon1;
            private Weapon weapon2;
            private Planet planet1;

            [SetUp]
            public void SetUp()
            {
                weapon1 = new("Weapon1", 3.2, 9);
                weapon2 = new("Weapon2", 50, 11);
                planet1 = new("Mars", 120);
            }

            [Test]
            public void WeaponConstructorShouldInitializeCorrectly()
            {
                weapon1 = new("Some weapon", 13.2, 3);
                Assert.IsNotNull(weapon1);
                string expectedName = "Some weapon";
                double expectedPrice = 13.2;
                int expectedPower = 3;
                Assert.That(expectedName == weapon1.Name);
                Assert.That(expectedPrice == weapon1.Price);
                Assert.That(expectedPower == weapon1.DestructionLevel);
            }

            [TestCase("Nuclear")]
            [TestCase("Bio")]
            [TestCase("Chemical")]
            public void WeaponNameShouldBeSetCorrectly(string name)
            {
                weapon1 = new(name, 11, 12);
                Assert.That(name == weapon1.Name);
            }

            [TestCase(11)]
            [TestCase(110.1)]
            [TestCase(1.2)]
            public void WeaponPriceShouldBeSetCorrectly(double price)
            {
                weapon1 = new("DirtyBomb", price, 11);
                Assert.That(price == weapon1.Price);
            }

            [TestCase(-10)]
            [TestCase(-1.1)]
            [TestCase(-0.01)]
            public void WeaponProceShouldThrowIfNegative(double price)
            {
                ArgumentException ex = Assert.Throws<ArgumentException>(()
                    => weapon1 = new("Bomb112", price, 10), "Price cannot be negative.");
            }

            [Test]
            public void DestructionLevelShouldBeSetCorrectly()
            {
                int expectedLevel = 9;
                Assert.That(expectedLevel == weapon1.DestructionLevel);
            }

            [Test]
            public void IncreaseLevelMethodShouldChangeByOne()
            {
                weapon1.IncreaseDestructionLevel();
                int expectedLevel = 10;
                Assert.That(expectedLevel == weapon1.DestructionLevel);
                Assert.IsTrue(weapon1.IsNuclear);
            }

            [Test]
            public void IsNuclearShouldBeSetCorrectly()
            {
                Assert.IsFalse(weapon1.IsNuclear);
            }

            [Test]
            public void PlanetConstructorShouldWorkCorrectly()
            {
                planet1 = new("Venus", 110.21);
                Assert.IsNotNull(planet1);
                Assert.IsNotNull(planet1.Weapons);
            }

            [Test]
            public void PlanetConstructorShouldSetValuesCorrectly()
            {
                planet1 = new("Melmak", 77.80);
                string expectedName = "Melmak";
                double expectedBudget = 77.80;
                int expectedCount = 0;
                Assert.That(expectedName == planet1.Name);
                Assert.That(expectedBudget == planet1.Budget);
                Assert.That(expectedCount == planet1.Weapons.Count);
            }

            [TestCase(null)]
            [TestCase("")]
            public void PlanetNameShouldThrowIfNullOrEmpty(string name)
            {
                ArgumentException ex = Assert.Throws<ArgumentException>(()
                    => planet1 = new(name, 112.5), "Invalid planet Name");
            }

            [TestCase("Mars")]
            [TestCase("Earth")]
            [TestCase("Pluto")]
            public void PlanetNameShouldBeSetCorrectly(string name)
            {
                planet1 = new(name, 210);
                Assert.That(name == planet1.Name);
            }

            [TestCase(0.1)]
            [TestCase(0)]
            [TestCase(111.4)]
            public void PlanetBudgetShouldBeSetCorrectly(double budget)
            {
                planet1 = new("Pluto", budget);
                Assert.That(budget == planet1.Budget);
            }

            [TestCase(-0.1)]
            [TestCase(-162.22)]
            [TestCase(-10)]
            public void PlanetBudgetShouldThrowIfNegative(double budget)
            {
                ArgumentException ex = Assert.Throws<ArgumentException>(()
                    => planet1 = new("Mars", budget), "Budget cannot drop below Zero!");
            }

            [Test]
            public void AddWeaponShouldIncreaseCount()
            {
                planet1.AddWeapon(weapon1);
                planet1.AddWeapon(weapon2);
                int expectedCount = 2;
                Assert.That(expectedCount == planet1.Weapons.Count);
                Assert.That(planet1.Weapons.Contains(weapon2));
            }

            [Test]
            public void MilitaryPowerRatioShouldCalculateCorrectly()
            {
                planet1.AddWeapon(weapon1);
                planet1.AddWeapon(weapon2);
                int expectedSum = 20;
                Assert.That(expectedSum == planet1.MilitaryPowerRatio);
            }

            [TestCase(11.1)]
            [TestCase(98.9)]
            [TestCase(1.1)]
            public void ProfitMethodShouldIncreaseTheBudget(double amount)
            {
                double budget = planet1.Budget;
                planet1.Profit(amount);
                double expectedNewBudget = budget + amount;
                Assert.That(expectedNewBudget == planet1.Budget);
            }

            [TestCase(20)]
            [TestCase(120)]
            [TestCase(71)]
            public void SpendMethodShouldDecreaseBudget(double amount)
            {
                double budget = planet1.Budget;
                planet1.SpendFunds(amount);
                double expectedNewBudget = budget - amount;
                Assert.That(expectedNewBudget == planet1.Budget);
            }

            [TestCase(121)]
            [TestCase(120.1)]
            [TestCase(310.1)]
            public void SpendMethodShouldThrowIfBudgetNotEnough(double amount)
            {
                InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                    => planet1.SpendFunds(amount), "Not enough funds to finalize the deal.");
            }

            [Test]
            public void AddWeaponShouldThrowIfWeaponExists()
            {
                planet1.AddWeapon(weapon1);
                planet1.AddWeapon(weapon2);
                Weapon weapon3 = new("Weapon1", 13, 2);
                InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                    => planet1.AddWeapon(weapon3), "There is already a Weapon1 weapon.");

                int expectedCount = 2;
                Assert.That(expectedCount == planet1.Weapons.Count);
            }

            [Test]
            public void RemoveWeaponShoudRemoveTheCorrectWeapon()
            {
                planet1.AddWeapon(weapon1);
                planet1.AddWeapon(weapon2);
                planet1.RemoveWeapon("Weapon2");
                int expectedCount1 = 1;
                Assert.That(expectedCount1 == planet1.Weapons.Count);
                Assert.IsFalse(planet1.Weapons.Contains(weapon2));
            }

            [TestCase("Weapon3")]
            [TestCase("Weapon")]
            public void RemoveWeaponShouldNotThrowIfNonExistingName(string weaponName)
            {
                planet1.AddWeapon(weapon1);
                planet1.AddWeapon(weapon2);
                planet1.RemoveWeapon(weaponName);
                int expectedCount = 2;
                Assert.That(expectedCount == planet1.Weapons.Count);
            }

            [Test]
            public void UpgradeWeaponShouldIncrementDestructionLevel()
            {
                planet1.AddWeapon(weapon1);
                planet1.AddWeapon(weapon2);
                planet1.UpgradeWeapon("Weapon1");
                planet1.UpgradeWeapon("Weapon2");
                int firstExpectedValue = 10;
                int secondExpectedValue = 12;
                int expectedRatio = 22;
                Assert.That(firstExpectedValue == weapon1.DestructionLevel);
                Assert.That(secondExpectedValue == weapon2.DestructionLevel);
                Assert.That(expectedRatio == planet1.MilitaryPowerRatio);
            }

            [Test]
            public void UpgradeWeaponShouldThrowIfWeaponNotExist()
            {
                planet1.AddWeapon(weapon1);
                planet1.AddWeapon(weapon2);
                planet1.UpgradeWeapon("Weapon1");
                planet1.UpgradeWeapon("Weapon2");
                InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                    => planet1.UpgradeWeapon("Weapon"), $"Weapon does not exist in the weapon repository of {planet1.Name}");
            }

            [Test]
            public void DestructOpponentShouldThrowIfOpponentTooStrong()
            {
                Planet planet2 = new("Mercury", 161);
                Weapon weapon21 = new("Weapon21", 16.1, 3);
                Weapon weapon22 = new("Weapon22", 16.1, 17);
                planet2.AddWeapon(weapon21);
                planet2.AddWeapon(weapon22);
                planet1.AddWeapon(weapon1);
                planet1.AddWeapon(weapon2);
                InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()
                    => planet1.DestructOpponent(planet2), "Mercury is too strong to declare war to!");
            }

            [Test]
            public void DestructOpponentShouldWorkCorrectly()
            {
                Planet planet2 = new("Mercury", 161);
                Weapon weapon21 = new("Weapon21", 16.1, 3);
                Weapon weapon22 = new("Weapon22", 16.1, 16);
                planet2.AddWeapon(weapon21);
                planet2.AddWeapon(weapon22);
                planet1.AddWeapon(weapon1);
                planet1.AddWeapon(weapon2);
                string expectedMsg = "Mercury is destructed!";
                Assert.That(expectedMsg == planet1.DestructOpponent(planet2));
            }
        }
    }
}
