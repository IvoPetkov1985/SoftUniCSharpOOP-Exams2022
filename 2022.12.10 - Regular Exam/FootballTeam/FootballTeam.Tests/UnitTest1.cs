using NUnit.Framework;
using System;

namespace FootballTeam.Tests
{
    public class Tests
    {
        private FootballTeam team;
        private FootballPlayer player1;
        private FootballPlayer player3;
        private FootballPlayer player8;

        [SetUp]
        public void Setup()
        {
            team = new("Slavia 1913", 15);
            player1 = new("Petkov", 1, "Goalkeeper");
            player3 = new("Tombak", 3, "Midfielder");
            player8 = new("Stoichkov", 8, "Forward");
        }

        [Test]
        public void ConstructorShouldCreateTheTeamCorrectly()
        {
            team = new("Loko PD", 22);
            Assert.IsNotNull(team);
            Assert.IsNotNull(team.Players);
        }

        [TestCase(15)]
        [TestCase(27)]
        [TestCase(33)]
        public void ConstructorShouldSetTheCorrectValues(int capacity)
        {
            team = new("CSKA-Lovech", capacity);
            string expectedName = "CSKA-Lovech";
            int expectedCapacity = capacity;
            string actualName = team.Name;
            int actualCapacity = team.Capacity;
            Assert.IsTrue(expectedName == actualName);
            Assert.IsTrue(expectedCapacity == actualCapacity);
        }

        [TestCase("Levski 1914")]
        [TestCase("Marek Dupnica")]
        [TestCase("Minyor Pernik 1919")]
        public void NameShoudBeSetCorrectly(string teamName)
        {
            team = new(teamName, 21);
            Assert.That(teamName, Is.EqualTo(team.Name));
        }

        [TestCase(null)]
        [TestCase("")]
        public void NameShouldThrowIfNullOrEmpty(string name)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => team = new(name, 17), "Name cannot be null or empty!");
        }

        [TestCase(14)]
        [TestCase(8)]
        [TestCase(-10)]
        public void CapacityShouldThrowIfValueBelow15(int capacity)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => team = new("Septemvri Simitli", capacity), "Capacity min value = 15");
        }

        [Test]
        public void CapacityShouldBeSetCorrectly()
        {
            int expectedValue = 15;
            int actualValue = team.Capacity;
            Assert.That(expectedValue, Is.EqualTo(actualValue));
        }

        [Test]
        public void AddNewPlayerShouldReturnTheCorrectMessage()
        {
            string expectedMsg = "Added player Tombak in position Midfielder with number 3";
            string actualMsg = team.AddNewPlayer(player3);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
        }

        [Test]
        public void AddNewPlayerShouldIncreaseThePlayersCount()
        {
            team.AddNewPlayer(player1);
            team.AddNewPlayer(player3);
            int expectedCount = 2;
            int actualCount = team.Players.Count;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
            Assert.IsTrue(team.Players.Contains(player1));
        }

        [Test]
        public void AddNewPlayerShouldReturnTheCorrectStringIfCapacityExceeded()
        {
            team.AddNewPlayer(player1);
            team.AddNewPlayer(player3);
            FootballPlayer player6 = new("Georgiev", 6, "Midfielder");
            FootballPlayer player4 = new("Genev", 4, "Midfielder");
            FootballPlayer player8 = new("Stoev", 8, "Midfielder");
            FootballPlayer player20 = new("Semedo", 20, "Midfielder");
            FootballPlayer player11 = new("Fabien", 11, "Forward");
            FootballPlayer player14 = new("Raichev", 14, "Forward");
            FootballPlayer player17 = new("Kazaldzhiev", 17, "Forward");
            FootballPlayer player10 = new("Nikolov", 10, "Forward");
            FootballPlayer player18 = new("Balov", 18, "Midfielder");
            FootballPlayer player12 = new("Zeedorf", 12, "Forward");
            FootballPlayer player21 = new("Stoianov", 21, "Midfielder");
            FootballPlayer player2 = new("Vucov", 2, "Goalkeeper");
            FootballPlayer player5 = new("Krastev", 5, "Goalkeeper");
            team.AddNewPlayer(player6);
            team.AddNewPlayer(player4);
            team.AddNewPlayer(player8);
            team.AddNewPlayer(player20);
            team.AddNewPlayer(player11);
            team.AddNewPlayer(player14);
            team.AddNewPlayer(player17);
            team.AddNewPlayer(player10);
            team.AddNewPlayer(player18);
            team.AddNewPlayer(player12);
            team.AddNewPlayer(player21);
            team.AddNewPlayer(player2);
            team.AddNewPlayer(player5);
            FootballPlayer player9 = new("Ivanov", 9, "Goalkeeper");
            string expectedMsg = "No more positions available!";
            string actualMsg = team.AddNewPlayer(player9);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
            Assert.IsFalse(team.Players.Contains(player9));
        }

        [Test]
        public void PickPlayerShouldReturnTheCorrectPlayer()
        {
            team.AddNewPlayer(player1);
            team.AddNewPlayer(player3);
            Assert.That(player1, Is.EqualTo(team.PickPlayer("Petkov")));
        }

        [Test]
        public void PlayerScoreShouldIncreasePlayersGoalsCount()
        {
            team.AddNewPlayer(player1);
            team.AddNewPlayer(player3);
            team.PlayerScore(3);
            team.PlayerScore(3);
            team.PlayerScore(3);
            team.PlayerScore(1);
            int expectedPlayer3Goals = 3;
            int expectedPlayer1Goals = 1;
            Assert.That(expectedPlayer3Goals, Is.EqualTo(player3.ScoredGoals));
            Assert.That(expectedPlayer1Goals, Is.EqualTo(player1.ScoredGoals));
        }

        [Test]
        public void PlayerScoreShouldReturnTheCorrectMsg()
        {
            team.AddNewPlayer(player1);
            team.AddNewPlayer(player3);
            team.PlayerScore(3);
            team.PlayerScore(3);
            string expectedMsg = "Tombak scored and now has 3 for this season!";
            string actualMsg = team.PlayerScore(3);
            Assert.That(expectedMsg, Is.EqualTo(actualMsg));
        }

        [Test]
        public void PlayerConstructorShouldInitializeCorrectly()
        {
            player8 = new("Lazarov", 10, "Midfielder");
            Assert.IsNotNull(player8);
            string expectedName = "Lazarov";
            int expectedNum = 10;
            string expectedPosition = "Midfielder";
            int expectedGoals = 0;
            Assert.That(expectedName, Is.EqualTo(player8.Name));
            Assert.That(expectedNum, Is.EqualTo(player8.PlayerNumber));
            Assert.That(expectedPosition, Is.EqualTo(player8.Position));
            Assert.That(expectedGoals, Is.EqualTo(player8.ScoredGoals));
        }

        [TestCase("Dimitrichko")]
        [TestCase("Pesho")]
        [TestCase("Gosho")]
        public void PlayerNameShouldSetTheCorrectValue(string name)
        {
            player8 = new(name, 7, "Goalkeeper");
            Assert.That(name, Is.EqualTo(player8.Name));
        }

        [TestCase("")]
        [TestCase(null)]
        public void PlayerNameShouldThrowIfNullOrEmpty(string name)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => player8 = new(name, 16, "Midfielder"), "Name cannot be null or empty!");
        }

        [TestCase(1)]
        [TestCase(21)]
        [TestCase(10)]
        [TestCase(19)]
        public void PlayerNumberShouldSetTheCorrectValue(int num)
        {
            player8 = new("Mitko", num, "Forward");
            Assert.That(num, Is.EqualTo(player8.PlayerNumber));
        }

        [TestCase(0)]
        [TestCase(22)]
        [TestCase(-10)]
        public void PlayerNumberShouldThrowIfNotBetween1And21(int num)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => player8 = new("Goshko", num, "Goalkeeper"), "Player number must be in range [1,21]");
        }

        [TestCase("Goalkeeper")]
        [TestCase("Midfielder")]
        [TestCase("Forward")]
        public void PlayerPositionShouldSetTheCorrectValue(string position)
        {
            player8 = new("Nasko", 15, position);
            Assert.That(position, Is.EqualTo(player8.Position));
        }

        [TestCase("Vratar")]
        [TestCase("Defender")]
        [TestCase("Na baba ti hvarchiloto")]
        public void PlayerPositionShouldThrowIfIncorrectInput(string position)
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(()
                => player8 = new("Pesho", 1, position), "Invalid Position");
        }

        [Test]
        public void PlayerScoreMethodShouldIncrementGoalsCount()
        {
            player8.Score();
            player8.Score();
            player8.Score();
            int expectedCount = 3;
            int actualCount = player8.ScoredGoals;
            Assert.That(expectedCount, Is.EqualTo(actualCount));
        }
    }
}
