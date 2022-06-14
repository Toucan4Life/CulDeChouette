using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BLL.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Game_Should_Throw_Exception_When_Less_Than_2_Players()
        {
            var mock = new Mock<IGameUI>();
            Assert.ThrowsException<ArgumentException>(() => new Game(1, mock.Object));
        }

        [TestMethod]
        public void Game_Should_Throw_Exception_When_More_Than_6_Players()
        {
            var mock = new Mock<IGameUI>();
            mock.Setup(g => g.AskPlayerCount()).Returns(7);

            Assert.ThrowsException<ArgumentException>(() => new Game(7, mock.Object));
        }

        [TestMethod]
        public void New_Game_Should_Have_The_Specified_Players_Numbers()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(2, mock.Object);
            
            Assert.AreEqual(2, game.Players.Count);
        }

        [TestMethod]
        public void Players_Should_Number_Automatically_Attribued()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(2, mock.Object);
          
            Assert.IsTrue(game.Players.Exists(t => t.Number == 1));
            Assert.IsTrue(game.Players.Exists(t => t.Number == 2));
        }

        [TestMethod]
        public void Players_Should_Start_With_Zero_Points()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(2, mock.Object);
            Assert.AreEqual(0, game.Players.Single(t => t.Number == 1).Points);
            Assert.AreEqual(0, game.Players.Single(t => t.Number == 2).Points);
        }


        [TestMethod]
        public void Game_End_When_1_Player_Reach_343_Points()
        {
            var mock = new Mock<IGameUI>();

            var game = new Game(2, mock.Object);
            game.Players.Single(t => t.Number == 1).Points = 343;

            Assert.IsTrue(game.IsWon);
        }

        [TestMethod]
        public void Game_End_When_1_Player_Is_Over_343_Points()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(2,mock.Object);
            game.Players.Single(t => t.Number == 1).Points = 344;

            Assert.IsTrue(game.IsWon);
        }

        [TestMethod]
        public void Game_Should_Start_With_The_First_Player()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(2, mock.Object);
            Assert.AreEqual(1, game.CurrentTurnPlayer.Number);
        }

        [TestMethod]
        public void Second_Player_Should_Play_After_First_Player()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(2, mock.Object);
            game.CurrentTurn = 2;
            Assert.AreEqual(2, game.CurrentTurnPlayer.Number);
        }

        [TestMethod]
        public void First_Player_Should_Play_After_Last_Player()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(3, mock.Object)
            {
                CurrentTurn = 4,
            };
            Assert.AreEqual(1, game.CurrentTurnPlayer.Number);
        }

        [TestMethod]
        public void Game_Should_Start_At_Round_Zero()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(2, mock.Object);
            Assert.AreEqual(1, game.CurrentRound);
        }

        [TestMethod]
        public void After_All_Players_Turn_Round_Is_Increased()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(3, mock.Object)
            {
                CurrentTurn = 4
            };
            Assert.AreEqual(2, game.CurrentRound);
        }

        [TestMethod]
        public void A_Chouette_Should_Earn_The_Correct_Players_Points()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(3, mock.Object);
            Assert.AreEqual(4, game.CalculateScore(new List<int> { 1, 2, 2 }).Result);
        }

        [TestMethod]
        public void A_Cul_De_Chouette_Should_Earn_The_Correct_Players_Points()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(3, mock.Object);
           
            Assert.AreEqual(100, game.CalculateScore(new List<int> { 6, 6, 6 }).Result);
        }

        [TestMethod]
        public void A_Velute_Should_Earn_The_Correct_Players_Points()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(3, mock.Object);
          
            Assert.AreEqual(72, game.CalculateScore(new List<int> { 2, 4, 6 }).Result);
        }

        [TestMethod]
        public void Shouting_When_No_Special_Combination_Is_A_Bevue()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(3, mock.Object);
            game.Players.First().Points = 15;

            game.Shout(game.Players.First(), Game.ShoutPhrases.PasMouleCaillou);

            Assert.AreEqual(5, game.Players.First().Points);
            mock.Verify(t=>t.BevueCommited(game.Players.First()),Times.Once);
        }

        [TestMethod]
        public void Shouting_Incorrectly_When_Chouette_Velute_Award_Bevue()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(3, mock.Object);

            game.CalculateScore(new List<int> { 2, 2, 4 });

            game.Shout(game.Players.ElementAt(1), Game.ShoutPhrases.GrelotteCaPicote);

            Assert.AreEqual(-10, game.Players.ElementAt(1).Points);
            mock.Verify(t => t.BevueCommited(game.Players.ElementAt(1)), Times.Once);
        }

        [TestMethod]
        public void Shouting_Correctly_When_Chouette_Velute_Award_Points_To_First_Shouters()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(3, mock.Object);

            var score = game.CalculateScore(new List<int> {2, 2, 4});

            game.Shout(game.Players.ElementAt(1), Game.ShoutPhrases.PasMouleCaillou);
            score.Wait();
            Assert.AreEqual(32, game.Players.ElementAt(1).Points);
        }

        [TestMethod]
        public void Game_Wait_Correct_Specified_Time_When_Chouette_Velute_And_No_Shout()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(3, mock.Object,1000);
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var score = game.CalculateScore(new List<int> { 2, 2, 4 });
            score.Wait();
            stopwatch.Stop();

            Assert.IsTrue(stopwatch.ElapsedMilliseconds >= 1000);
        }

        [TestMethod]
        public void Game__Doesnt_Wait_Specified_Time_When_Chouette_Velute_And_Correct_Shout()
        {
            var mock = new Mock<IGameUI>();
            var game = new Game(3, mock.Object, 1000);
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var score = game.CalculateScore(new List<int> { 2, 2, 4 });
            game.Shout(game.Players.ElementAt(1), Game.ShoutPhrases.PasMouleCaillou);
            score.Wait();
            stopwatch.Stop();

            Assert.IsTrue(stopwatch.ElapsedMilliseconds <= 1000);
        }
    }
}