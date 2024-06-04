class Program
{

    const int WORD_LENGTH_FROM = 8;
    const int WORD_LENGTH_TO = 30;

    private static HashSet<string> words = new HashSet<string>();

    static bool endGame = false;
    
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

    static void CheckWord(string word, string newWord)
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

    private static string GetPlayerName(int step)
    {
        return "Игрок_" + ((step % 2 == 0) ? 1 : 2);
    }

    static void Main(string[] args)
    {            
        int step = 0;

        Console.WriteLine($"Пользователь, введите слово для начала игры от " + WORD_LENGTH_FROM + " до " + WORD_LENGTH_TO + " символов");
        string word;
        while (true)
        {
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

        Console.WriteLine($"Слово для игры: {word}");

        while (!endGame)
        {
            Console.Write($"{GetPlayerName(step)} : ");
            string newWord = Console.ReadLine();

            CheckWord(word, newWord);

            step++;
        }

        Console.WriteLine($"\nПобедил {GetPlayerName(step)} !!!");

    }

}