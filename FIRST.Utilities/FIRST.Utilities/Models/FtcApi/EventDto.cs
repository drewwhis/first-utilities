namespace FIRST.Utilities.Models.FtcApi;

public class EventDto
{
    public Guid EventId { get; set; }
    public string? Code { get; set; }
    public string? DivisionCode { get; set; }
    public string? Name { get; set; }
    public bool Remote { get; set; }
    public bool Hybrid { get; set; }
    public bool Published { get; set; }
    public int FieldCount { get; set; }
    public string? Type { get; set; }
    public required string TypeName { get; set; }
    public string? RegionCode { get; set; }
    public string? LeagueCode { get; set; }
    public string? DistrictCode { get; set; }
    public string? Venue { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? StateProv { get; set; }
    public string? Country { get; set; }
    public string? Website { get; set; }
    public string? LiveStreamUrl { get; set; }
    public string? Timezone { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
}