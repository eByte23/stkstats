namespace STKBC.Stats.Data.Models;


public class FileImportRequest
{
    public Guid? Id { get; set; }
    public DateTime? GameDate { get; set; }
    public string? ExternalRef { get; set; }
    public string? FileName { get; set; }
    public string? HomeTeam { get; set; }
    public string? AwayTeam { get; set; }
    public string? ImportType { get; set; }
    public Guid? FileId { get; set; }
    public string? FileHash { get; set; }
    public DateTime? UploadedAt { get; set; }
}