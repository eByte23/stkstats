namespace STKBC.Stats.Data.Models;


public class FileUpload
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Extension { get; set; }
    public int? Size { get; set; }
    public string? Hash { get; set; }
    public string? Location { get; set; }
}