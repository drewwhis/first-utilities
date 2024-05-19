using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Models.Database;

[Index(nameof(ProgramCode), IsUnique = true)]
public class Program
{
    public int ProgramId { get; set; }
    [StringLength(25)] public string ProgramCode { get; set; } = null!;
    public int ActiveSeasonYear { get; set; }
}