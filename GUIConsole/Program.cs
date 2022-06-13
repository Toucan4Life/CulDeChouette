// See https://aka.ms/new-console-template for more information

using BLL;
using GUIConsole;

var consoleUi = new ConsoleUI();
var playerCount = consoleUi.AskPlayerCount();

var game = new Game(playerCount, consoleUi);
game.Launch();

Console.WriteLine("Press any keys to exit");
Console.ReadKey();