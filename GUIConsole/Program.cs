// See https://aka.ms/new-console-template for more information

using BLL;

Console.WriteLine("Players count : ");
var playersCount = int.Parse(Console.ReadLine());
var game = new Game(playersCount);
var rnd = new Random();

while (!game.IsWon)
{
    Console.WriteLine($"Turn of Player : {game.CurrentTurnPlayer.Number}");

    Console.WriteLine("Press any keys to throw the dices");
    //Console.ReadKey();

    var firstDice = rnd.Next(1, 7);
    var secondDice = rnd.Next(1, 7);
    var thirdDice = rnd.Next(1, 7);
    
    Console.WriteLine($"Dices Result {firstDice} / {secondDice} / {thirdDice}");

    game.CalculateScore(new List<int> {firstDice, secondDice, thirdDice });



    Console.WriteLine($"Player {game.CurrentTurnPlayer.Number} points : {game.CurrentTurnPlayer.Points}");
    Console.WriteLine(Environment.NewLine);

    if (game.IsWon)
    {
        break;
    }

    game.NextTurn();

}

Console.WriteLine($"Player {game.CurrentTurnPlayer.Number} won with {game.CurrentTurnPlayer.Points}!");
Console.WriteLine($"Game round: {game.CurrentRound}");

Console.WriteLine("Press any keys to exit");
Console.ReadKey();