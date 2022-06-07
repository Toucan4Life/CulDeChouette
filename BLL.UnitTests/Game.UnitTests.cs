using System;
using BLL.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Game_Should_Have_At_Least_2_Players()
        {
            Assert.ThrowsException<ArgumentException>(()=> new Game(1));
        }
    }
}