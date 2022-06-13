namespace BLL;

public class Game
{
    private readonly IGameUI _userInterface;

    public Game(int playerCount, IGameUI ui)
    {
        _userInterface = ui;
        CurrentRound = 1;
        CurrentTurn = 1;
        Players = CreatePlayers(playerCount);
    }

    public int CurrentTurn { get; set; }

    public List<Player> Players { get; }
    public bool IsWon => Players.Any(p => p.Points >= 343);

    public Player CurrentTurnPlayer => Players[(CurrentTurn - 1) % Players.Count];

    public int CurrentRound
    {
        get => (int) Math.Round((decimal) CurrentTurn / Players.Count, MidpointRounding.ToPositiveInfinity);
        private set { }
    }

    public void Launch()
    {
        var rnd = new Random();

        while (!IsWon)
        {
            _userInterface.ShowPlayerTurn(CurrentTurnPlayer.Number);

            var firstDice = rnd.Next(1, 7);
            var secondDice = rnd.Next(1, 7);
            var thirdDice = rnd.Next(1, 7);

            _userInterface.ShowDiceResult(firstDice, secondDice, thirdDice);

            CurrentTurnPlayer.Points += CalculateScore(new List<int> {firstDice, secondDice, thirdDice});

            _userInterface.ShowPlayerPoint(CurrentTurnPlayer.Number, CurrentTurnPlayer.Points);

            if (IsWon) break;

            CurrentTurn++;
        }

        _userInterface.ShowWinner(CurrentTurnPlayer.Number, CurrentTurnPlayer.Points, CurrentRound);
    }

    public List<Player> CreatePlayers(int playersNumber)
    {
        if (playersNumber is < 2 or > 6)
            throw new ArgumentException(
                $"Players count should be between 2 and 6. Actual player count: {playersNumber}");

        return Enumerable.Range(1, playersNumber).Select(i => new Player(i)).ToList();
    }

    public int CalculateScore(IEnumerable<int> dices)
    {
        if (dices.Count() != 3)
            throw new ArgumentException($"There should only be 3 dices. Actual dice count : {dices}");
        dices = dices.OrderBy(t => t);
        //CulDeChouette
        if (dices.GroupBy(t => t).Count(t => t.Count() == 3) != 0)
        {
            var culdechouette = dices.First();
            return 40 + 10 * culdechouette;
        }
        //Chouette Velut�e
        //else if (dices.First() == dices.ElementAt(1) && dices.First()+dices.ElementAt(1) == dices.ElementAt(2))
        //{
        //    var chouette = dices.GroupBy(t => t).First(t => t.Count() == 2).Key;
        //    CurrentTurnPlayer.Points += chouette * chouette;
        //}
        //Chouette

        if (dices.GroupBy(t => t).Count(t => t.Count() == 2) != 0)
        {
            var chouette = dices.GroupBy(t => t).First(t => t.Count() == 2).Key;
            return chouette * chouette;
        }
        //Velute

        if (dices.First() + dices.ElementAt(1) == dices.ElementAt(2))
        {
            var velute = dices.ElementAt(2);
            return 2 * velute * velute;
        }

        return 0;
    }
}