// See https://aka.ms/new-console-template for more information

using BLL;
using GUIConsole;

var game = new Game(new ConsoleUI());
game.Launch();

Console.WriteLine("Press any keys to exit");
Console.ReadKey();