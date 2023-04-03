namespace STKBC.Data.Models;

public class Season
{
    public Guid? Id { get; set; }

    public string? Name { get; set; }

    // This should have a format like 2023 or 2023/2024
    public string? Year { get; set; }
}




public static class SeasonTypes
{
    public (bool success, Season season) GetSummerSeasonForYear(int? year)
    {
        if(!year.HasValue || int.l year.Value)
    }
}