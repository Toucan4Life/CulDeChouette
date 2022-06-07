namespace BLL.UnitTests;

public class Game
{
    public Game(int playersNumber)
    {
        if (playersNumber < 2)
        {
            throw new ArgumentException($"Players count should be between 2 and 6. Actual player count: {playersNumber}");
        }
    }
}