using Newtonsoft.Json;

public class Step
{
    [JsonProperty("player")]
    public User player;

    [JsonProperty("word")]
    public string word;

    public Step(User player, string word)
    {
        this.player = player;
        this.word = word;
    }

}