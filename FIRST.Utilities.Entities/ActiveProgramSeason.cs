using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Entities;

[Index(
    nameof(ProgramCode), 
    IsUnique = true, 
    Name = nameof(ProgramCode))]
public class ActiveProgramSeason
{
    public int ActiveProgramSeasonId { get; set; }
    
    [StringLength(10)]
    public required string ProgramCode { get; set; }
    
    [StringLength(80)]
    public required int SeasonYear { get; set; }
}