namespace BLL;

public class Game
{
    public Game(int playersNumber)
    {
        if (playersNumber is < 2 or > 6)
        {
            throw new ArgumentException($"Players count should be between 2 and 6. Actual player count: {playersNumber}");
        }

        Players = Enumerable.Range(1, playersNumber).Select(i => new Player(i)).ToList();
        CurrentTurnPlayer = Players.First();
        CurrentRound = 1;
    }

    public List<Player> Players { get; set; }
    public bool IsWon => Players.Any(p => p.Points >= 343);
    public Player CurrentTurnPlayer { get; set; }
    public int CurrentRound { get; set; }

    public void NextTurn()
    {
        if (Players.IndexOf(CurrentTurnPlayer) == Players.Count - 1)
        {
            NextRound();
        }
        else
        {
            CurrentTurnPlayer = Players.ElementAt(Players.IndexOf(CurrentTurnPlayer) + 1);
        }
    }

    private void NextRound()
    {
        CurrentTurnPlayer = Players.First();
        CurrentRound++;
    }

    public void CalculateScore(IEnumerable<int> dices)
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
            CurrentTurnPlayer.Points += 40 + 10 * culdechouette;
        }
        //Chouette Velutée
        //else if (dices.First() == dices.ElementAt(1) && dices.First()+dices.ElementAt(1) == dices.ElementAt(2))
        //{
        //    var chouette = dices.GroupBy(t => t).First(t => t.Count() == 2).Key;
        //    CurrentTurnPlayer.Points += chouette * chouette;
        //}
        //Chouette
        else if (dices.GroupBy(t=>t).Count(t => t.Count()==2) !=0)
        {
            var chouette = dices.GroupBy(t => t).First(t => t.Count() == 2).Key;
            CurrentTurnPlayer.Points += chouette * chouette;
        }
        //Velute
        else if (dices.First() + dices.ElementAt(1) == dices.ElementAt(2))
        {
            var velute = dices.ElementAt(2);
            CurrentTurnPlayer.Points += 2 * velute * velute;
        }
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