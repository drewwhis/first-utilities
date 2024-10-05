using FIRST.Utilities.Entities;
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
    public DbSet<ActiveProgramSeason> ActiveProgramSeasons { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .Entity<FtcEvent>()
            .HasOne(e => e.ActiveEvent)
            .WithOne(a => a.Event)
            .OnDelete(DeleteBehavior.Cascade);

        // modelBuilder
        //     .Entity<FtcTeam>()
        //     .HasMany(t => t.Matches)
        //     .WithOne(m => m.Blue1Team)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // modelBuilder
        //     .Entity<FtcTeam>()
        //     .HasMany(t => t.Matches)
        //     .WithOne(m => m.Blue2Team)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // modelBuilder
        //     .Entity<FtcTeam>()
        //     .HasMany(t => t.Matches)
        //     .WithOne(m => m.Red1Team)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // modelBuilder
        //     .Entity<FtcTeam>()
        //     .HasMany(t => t.Matches)
        //     .WithOne(m => m.Red2Team)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}