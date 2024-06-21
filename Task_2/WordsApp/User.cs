
using Newtonsoft.Json;

public class User
{
    [JsonProperty("name")]
    public string Name;    

    public User(string name)
    {
       Name = name;           
    }

    public string GetName() 
    {
        return Name;
    }
}