namespace STKBC.Stats.Data.Models;

public class Season
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Key { get; set; }
    public Guid? LeagueId { get; set; }
    public DateTime StartDate { get; set; }
}




public static class SeasonTypes
{
    // public (bool success, Season season) GetSummerSeasonForYear(int? year)
    // {
    //     if(!year.HasValue || int.l year.Value)
    // }
}