using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace GUIConsole
{
    public class ConsoleUI : IGameUI
    {
        public void ShowPlayerTurn(int currentTurnPlayer)
        {
            Console.WriteLine($"Turn of Player : {currentTurnPlayer}");
        }

        public void ShowPlayerPoint(int currentTurnPlayer, int points)
        {
            Console.WriteLine($"Player {currentTurnPlayer} points : {points}");
            Console.WriteLine(Environment.NewLine);
        }

        public void ShowWinner(int currentTurnPlayer, int points, int currentRound)
        {

            Console.WriteLine($"Player {currentTurnPlayer} won with {points}!");
            Console.WriteLine($"Game round: {currentRound}");
        }

        public void ShowDiceResult(int firstDice, int secondDice, int thirdDice)
        {
            Console.WriteLine($"Dices Result {firstDice} / {secondDice} / {thirdDice}");
        }

        public int AskPlayerCount()
        {
            Console.WriteLine("Players count : ");
            return int.Parse(Console.ReadLine() ?? string.Empty);
        }

        public void BevueCommited(Player player)
        {
            Console.WriteLine($"Player {player.Number} committed a Bevue !");
        }
    }
}
