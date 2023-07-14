namespace StatSys.CoreStats.Models;

public class PlayerData
{
    // used for web slug format: <fname>-<last>-<last12-digist-uniqueid>
    // e.g. elijah-bate-29cd020d6b4b
    public string? ShortId { get; set; }


    // GUID unqiue by club and fname-lname
    public Guid UniqueId { get; set; }

    // fname lname
    public string? Name { get; set; }

    public string FirstName {get;set;}
    public string LastName {get;set;}
    public List<string> GameChangerIds { get; set; } = new();
    public HittingData Hitting { get; set; } = new();

    // public PitchingData Pitching { get; set; } = new();
}
