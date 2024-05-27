using FIRST.Utilities.Models.Database;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Event> Events { get; set; }
    public DbSet<ActiveEvent> ActiveEvents { get; set; }
    public DbSet<Models.Database.Program> Programs { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<EventTeam> EventTeams { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .Entity<Event>()
            .HasOne(e => e.ActiveEvent)
            .WithOne(a => a.Event)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder
            .Entity<Event>()
            .HasMany(e => e.Teams)
            .WithOne(e => e.Event)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder
            .Entity<Models.Database.Program>()
            .HasData(new Models.Database.Program { ProgramId = 1, ProgramCode = "FTC", ActiveSeasonYear = 2023 });
        
        modelBuilder
            .Entity<Team>()
            .HasMany(t => t.Events)
            .WithOne(e => e.Team)
            .OnDelete(DeleteBehavior.Cascade);
    }
}