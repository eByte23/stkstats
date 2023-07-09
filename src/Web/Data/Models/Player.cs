namespace STKBC.Stats.Data.Models;

public class Player
{
    public Guid? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Guid? CurrentClubId { get; set; }
    public string? PrimaryPosition { get; set; }
}