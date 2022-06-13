namespace BLL;

public class Game
{
    public IGameUI _userInterface;
    public int _playersNumber;

    public Game(IGameUI UI)
    {
        _userInterface = UI;
        CurrentRound = 1;
        CurrentTurn = 1;
        _playersNumber = _userInterface.AskPlayerCount();
        Players = CreatePlayers(_playersNumber);
    }

    public int CurrentTurn { get; set; }

    public void Launch()
    {
        var rnd = new Random();


        while (!IsWon)
        {
            _userInterface.ShowPlayerTurn(CurrentTurnPlayer.Number);

            var firstDice = rnd.Next(1, 7);
            var secondDice = rnd.Next(1, 7);
            var thirdDice = rnd.Next(1, 7);

            _userInterface.ShowDiceResult(firstDice,secondDice,thirdDice);

            CurrentTurnPlayer.Points += CalculateScore(new List<int> { firstDice, secondDice, thirdDice });

            _userInterface.ShowPlayerPoint(CurrentTurnPlayer.Number, CurrentTurnPlayer.Points);

            if (IsWon)
            {
                break;
            }

            CurrentTurn++;
            if (CurrentTurn % _playersNumber == 1) CurrentRound++;
        }

        _userInterface.ShowWinner(CurrentTurnPlayer.Number, CurrentTurnPlayer.Points, CurrentRound);
    }

    public List<Player> CreatePlayers(int playersNumber)
    {
        if (playersNumber is < 2 or > 6)
        {
            throw new ArgumentException($"Players count should be between 2 and 6. Actual player count: {playersNumber}");
        }

        return Enumerable.Range(1, playersNumber).Select(i => new Player(i)).ToList();
    }

    public List<Player> Players { get; set; }
    public bool IsWon => Players.Any(p => p.Points >= 343);

    public Player CurrentTurnPlayer => Players[(CurrentTurn - 1) % _playersNumber];

    public int CurrentRound
    {
        get => (int) Math.Round((decimal) CurrentTurn / 3, MidpointRounding.ToPositiveInfinity);
        private set { }
    }

    public int CalculateScore(IEnumerable<int> dices)
    {

        if (dices.Count() != 3)
        {
            throw new ArgumentException($"There should only be 3 dices. Actual dice count : {dices}");
        }
        dices = dices.OrderBy(t => t);
        //CulDeChouette
        if (dices.GroupBy(t => t).Count(t => t.Count() == 3) != 0)
        {
            var culdechouette = dices.First();
            return 40 + 10 * culdechouette;
        }
        //Chouette Velutée
        //else if (dices.First() == dices.ElementAt(1) && dices.First()+dices.ElementAt(1) == dices.ElementAt(2))
        //{
        //    var chouette = dices.GroupBy(t => t).First(t => t.Count() == 2).Key;
        //    CurrentTurnPlayer.Points += chouette * chouette;
        //}
        //Chouette

        if (dices.GroupBy(t=>t).Count(t => t.Count()==2) !=0)
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

public class Player
{
    public Player(int number)
    {
        Number = number;
    }
    public int Number { get; set; }
    public int Points { get; set; }
}