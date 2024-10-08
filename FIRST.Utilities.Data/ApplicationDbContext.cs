using FIRST.Utilities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<FtcEvent> FtcEvents { get; set; }
    public DbSet<ActiveFtcEvent> ActiveFtcEvents { get; set; }
    public DbSet<FtcTeam> FtcTeams { get; set; }
    public DbSet<FtcMatch> FtcMatches { get; set; }
    public DbSet<FtcMatchParticipant> FtcMatchParticipants { get; set; }
    public DbSet<ActiveProgramSeason> ActiveProgramSeasons { get; set; }
    public DbSet<ActiveFtcMatch> ActiveFtcMatches { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .Entity<FtcEvent>()
            .HasOne(e => e.ActiveEvent)
            .WithOne(a => a.Event)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder
            .Entity<FtcMatch>()
            .HasOne(e => e.ActiveMatch)
            .WithOne(a => a.Match)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<IdentityRole>()
            .HasData(
                new IdentityRole("UtilitiesAdmin"),
                new IdentityRole("FtcEventAdmin")
            );
    }
}