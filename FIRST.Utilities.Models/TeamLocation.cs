namespace FIRST.Utilities.Models;

public class TeamLocation
{
    public int TeamNumber { get; set; }
    public required string TeamName { get; set; }
    public TeamLocationType LocationType { get; set; }
}