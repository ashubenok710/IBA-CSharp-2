using Newtonsoft.Json;
using System.Text.Json.Serialization;

public class Game
{

    const string HISTORY_FILE = "Games.json";

    const int WORD_LENGTH_FROM = 8;    
    const int WORD_LENGTH_TO = 30;

    private string word;

    [JsonPropertyName("Player1")]
    private User Player1;
    [JsonPropertyName("Player2")]
    private User Player2;
    
    private static HashSet<string> words = new HashSet<string>();
    
    static bool endGame = false;
    
    private int step = 0;

    [JsonPropertyName("Games")]
    private List<GameResult> Games = new List<GameResult>();

    public Game()
    {        
        ReadGames();
    }

    private static bool IsValidWord(string word)
    {
        return word.Length >= WORD_LENGTH_FROM & word.Length <= WORD_LENGTH_TO;
    }

    private static Dictionary<char, int> GetWordAsDictionary(string word)
    {
        return word.ToLower()
            .Where(c => char.IsLetter(c))
            .GroupBy(c => c)
            .ToDictionary(k => k.Key, v => v.Count());
    }

    private static bool CheckLetters(string word, string newWord)
    {
        Dictionary<char, int> d1 = GetWordAsDictionary(word);

        Dictionary<char, int> d2 = GetWordAsDictionary(newWord);

        foreach (var item in d2)
        {
            if (!d1.ContainsKey(item.Key)) return false;
            if (item.Value > d1[item.Key]) return false;
        }

        return true;
    }

    private static void CheckWord(string word, string newWord)
    {
        bool res = newWord.ToLower().All(word.ToLower().Contains);

        if (!CheckLetters(word, newWord) | !res | newWord == "" | words.Contains(newWord))
        {
            endGame = true;
        }
        else
        {
            words.Add(newWord);
        }
    }

    private User GetPlayer(int step)
    {
        return (step % 2 == 0) ? Player1 : Player2;
    }

    private string GetPlayerName(int step)
    {
        return ((step % 2 == 0) ? Player1.GetName() : Player2.GetName());
    }
    public void InputWord()
    {
        Console.WriteLine($"\nВведите слово для начала игры от " + WORD_LENGTH_FROM + " до " + WORD_LENGTH_TO + " символов");

        while (true)
        {
            Console.Write("Слово для игры: ");
            word = Console.ReadLine();
            if (word == null) break;

            if (IsValidWord(word))
            {
                break;
            }
            else
            {
                Console.WriteLine($"Слово для игры не может быть меньше " + WORD_LENGTH_FROM + " и больше " + WORD_LENGTH_TO + " символов.");
            }
        }
    }

    public void PrintWelcomeMessage()
    {
        Console.WriteLine("Добро пожаловать в игру в слова.");
    }

    public void PrintRules()
    {        
        Console.WriteLine("\n2 пользователя поочередно вводят слова, состоящие из букв первоначально указанного слова.\nПроигрывает тот, кто в свою очередь не вводит слово");
    }

    public void PrintCommands()
    {
        Console.WriteLine("\nСписок комманд:\n" +
            "/show-words – показать все введенные обоими пользователями слова в текущей игре;\n" +
            "/score – показать общий счет по играм для текущих игроков (извлечь из файла);\n" +
            "/total-score – показать общий счет для всех игроков.");
    }
    public void GetPlayerNames()
    {
        Console.WriteLine("\nВведите имена игроков\n");

        do
        {
            Console.Write("Игрок 1: ");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                Player1 = new User(name);
                break;
            }
        }
        while (true);

        do
        {
            Console.Write("Игрок 2: ");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                Player2 = new User(name);
                break;
            }
        }
        while (true);
    }
    
    private static bool IsCommand(string str)
    {
        return (str != null && (str.Contains('/')) ? true : false);
    }

    private void ExecuteCommand(string command)
    {
        switch (command)
        {
            case "/show-words":
                PrintWords();                
                break;
            case "/score":
                PrintScore();
                break;
            case "/total-score":                
                PrintTotalScore();
                break;
            default:
                Console.WriteLine($"Неизвестная команда \"{command}\"");
                break;
        }
    }

    public void PlayGame()
    {
        Console.WriteLine("\nИгра началась\n");

        GameResult gameResult = new GameResult(Player1, Player2, word, DateTime.Now);

        while (!endGame)
        {
            Console.Write($"{GetPlayerName(step)} : ");
            string newWord = Console.ReadLine();

            if (IsCommand(newWord))
            {
                ExecuteCommand(newWord);
            } 
            else 
            {
                CheckWord(word, newWord);

                gameResult.AddStep(new Step(GetPlayer(step), newWord));
                step++;
            }
            
        }
        gameResult.SetWinner(GetPlayer(step));

        Games.Add(gameResult);
        
        SaveGames();

        Console.WriteLine($"\nПобедил игрок {GetPlayer(step).GetName()} !!!");
    }

    private void SaveGames()
    {               
        File.WriteAllText(HISTORY_FILE, JsonConvert.SerializeObject(Games, Formatting.Indented));
    }
    
    private void PrintWords()
    {
        if (words.Count > 0) 
        {
            Console.WriteLine($"Слова: {string.Join(", ", words)}\n");
        }
        else
        {
            Console.WriteLine($"В игре слова еще не введены\n");
        }

    }

    private void PrintScore()
    {
        if (Games != null && Games.Count > 0) 
        {
            Console.WriteLine($"Игры с участием {Player1.GetName()} и {Player2.GetName()} ");
            int j = 1;
            for (int i = 0; i < Games.Count; i++)
            {
                if (Games[i].IsTwoUsersPlay(Player1, Player2))
                {
                    Console.WriteLine($"{j++}) {Games[i].GetGameDate()} победил игрок {Games[i].GetWinner().GetName()}");
                }
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine($"Общий счет по играм для текущих игроков не найден\n");
        }
    }

    private void PrintTotalScore()
    {
        if (Games != null && Games.Count > 0)
        {
            Console.WriteLine("\nИгрок | Побед | Поражений");

            SortedSet<string> gamers = new SortedSet<string>();
            foreach (var gameResult in Games) 
            {
                gamers.Add(gameResult.GetPlayer1().GetName());
                gamers.Add(gameResult.GetPlayer2().GetName());
            }

            var winners = Games.GroupBy(p => p.GetWinner().GetName())
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToDictionary(x => x.Name, x => x.Count);

            var loosers = Games.GroupBy(p => p.GetLooser().GetName())
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToDictionary(x => x.Name, x => x.Count); ;

            foreach (var gamer in gamers)
            {                
                Console.WriteLine($"{gamer} | {(winners.ContainsKey(gamer) ? winners[gamer] : 0)} | {(loosers.ContainsKey(gamer) ? loosers[gamer] : 0)}");
            }
        }
        else
        {
            Console.WriteLine("общий счет для всех игроков не найден");
        }
        
        Console.WriteLine();
    }

    private void ReadGames()
    {
        try
        {
            var json = File.ReadAllText(HISTORY_FILE);
            Games = JsonConvert.DeserializeObject<List<GameResult>>(json);
        }
        catch
        {
            Games = new List<GameResult>();
        }        
    }

}