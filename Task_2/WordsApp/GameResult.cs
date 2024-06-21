using Newtonsoft.Json;

class GameResult
{
    [JsonProperty("player1")]
    private User Player1;
    [JsonProperty("player2")]
    private User Player2;

    [JsonProperty("winner")]
    private User Winner;

    [JsonProperty("dateTime")]
    private DateTime DateTime;

    [JsonProperty("word")]
    private string Word;

    [JsonProperty("steps")]
    private List<Step> Steps = new List<Step>();

    [JsonConstructor]
    public GameResult(User player1, User player2, User winner, DateTime dateTime, string word, List<Step> steps) : this(player1, player2, winner, dateTime)
    {
        Word = word;
        Steps = steps;
    }

    public GameResult(User user1, User user2, string word, DateTime date)
    {
        this.Player1 = user1;
        this.Player2 = user2;
        this.Word = word;
        this.DateTime = date;
    }

    public GameResult(User user1, User user2, User winner, DateTime date)
    {
        this.Player1 = user1;
        this.Player2 = user2;
        this.Winner = winner;
        this.DateTime = date;
    }

    public void AddStep(Step step)
    {
        this.Steps.Add(step);
    }

    public User GetPlayer1()
    {
        return this.Player1;
    }

    public User GetPlayer2()
    {
        return this.Player2;
    }

    public void SetWinner(User winner)
    {
        this.Winner = winner;
    }

    public User GetWinner()
    {
        return this.Winner;
    }

    public User GetLooser()
    {
        return !Player1.GetName().Equals(GetWinner().GetName()) ? Player1 : Player2;
    }

    public DateTime GetGameDate()
    {
        return this.DateTime;
    }

    public bool IsTwoUsersPlay(User player1, User player2)
    {
        return Player1.GetName().Equals(player1.GetName()) && Player2.GetName().Equals(player2.GetName()) ||
                Player1.GetName().Equals(player2.GetName()) && Player2.GetName().Equals(player1.GetName());
    }

}