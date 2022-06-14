using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IGameUI
    {
        void ShowPlayerTurn(int number);
        void ShowPlayerPoint(int number, int points);
        void ShowWinner(int number, int points, int currentRound);
        void ShowDiceResult(int firstDice, int secondDice, int thirdDice);
        int AskPlayerCount();
        void BevueCommited(Player player);
    }
}
