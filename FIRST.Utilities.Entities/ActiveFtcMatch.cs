using System.ComponentModel.DataAnnotations;

namespace FIRST.Utilities.Entities;

public class ActiveFtcMatch
{
    [Key]
    public int FtcMatchId { get; set; }

    public FtcMatch Match { get; set; } = null!;
}