namespace StatSys.CoreStats.Models;


public class TeamMetadata
{
    public string ShortId { get; set; }
    public string TeamId { get; set; }
    public string TeamName { get; set; }
    public string SeasonName { get; set; }
    public string SeasonId { get; set; }

    public string Record { get; set; }

    public List<(string fileName, string date)> Games { get; set; } = new();
}
