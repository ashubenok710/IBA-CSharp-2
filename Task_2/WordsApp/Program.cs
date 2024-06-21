class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.PrintWelcomeMessage();
        game.PrintRules();
        game.PrintCommands();

        game.GetPlayerNames();
        game.InputWord();
        game.PlayGame();
              
        Console.ReadLine();
    }

}