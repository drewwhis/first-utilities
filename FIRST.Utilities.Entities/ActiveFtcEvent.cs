using System.ComponentModel.DataAnnotations;

namespace FIRST.Utilities.Entities;

public class ActiveFtcEvent
{
    [Key]
    public int EventId { get; set; }

    public FtcEvent Event { get; set; } = null!;
}