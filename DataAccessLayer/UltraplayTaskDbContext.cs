using AbstractionProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class UltraplayTaskDbContext : DbContext
    {
        public virtual DbSet<Sport> Sports { get; set; }
        
        public virtual DbSet<Event> Events { get; set; }
        
        public virtual DbSet<Match> Matches { get; set; }
        
        public virtual DbSet<Bet> Bets { get; set; }
        
        public virtual DbSet<Odd> Odds { get; set; }
        
        public UltraplayTaskDbContext(DbContextOptions<UltraplayTaskDbContext> options)
                            : base(options)
        { }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sport>()
                        .HasMany(sportEntity => sportEntity.Events)
                        .WithOne(eventEntity => eventEntity.Sport)
                        .HasForeignKey(eventEntity => eventEntity.SportID)
                        .HasPrincipalKey(sportEntity => sportEntity.ID)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                        .HasOne(eventEntity => eventEntity.Sport)
                        .WithMany(sportEntity => sportEntity.Events)
                        .HasForeignKey(eventEntity => eventEntity.SportID)
                        .HasPrincipalKey(sportEntity => sportEntity.ID);

            modelBuilder.Entity<Event>()
                        .HasMany(eventEntity => eventEntity.Matches)
                        .WithOne(matchEntity => matchEntity.Event)
                        .HasForeignKey(matchEntity => matchEntity.EventID)
                        .HasPrincipalKey(eventEntity => eventEntity.ID)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>()
                        .HasOne(matchEntity => matchEntity.Event)
                        .WithMany(eventEntity => eventEntity.Matches)
                        .HasForeignKey(matchEntity => matchEntity.EventID)
                        .HasPrincipalKey(eventEntity => eventEntity.ID);

            modelBuilder.Entity<Match>()
                        .HasMany(matchEntity => matchEntity.Bets)
                        .WithOne(betEntity => betEntity.Match)
                        .HasForeignKey(betEntity => betEntity.MatchID)
                        .HasPrincipalKey(matchEntity => matchEntity.ID)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bet>()
                        .HasOne(betEntity => betEntity.Match)
                        .WithMany(matchEntity => matchEntity.Bets)
                        .HasForeignKey(betEntity => betEntity.MatchID)
                        .HasPrincipalKey(matchEntity => matchEntity.ID);

            modelBuilder.Entity<Bet>()
                        .HasMany(betEntity => betEntity.Odds)
                        .WithOne(oddEntity => oddEntity.Bet)
                        .HasForeignKey(oddEntity => oddEntity.BetID)
                        .HasPrincipalKey(betEntity => betEntity.ID)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Odd>()
                        .HasOne(oddEntity => oddEntity.Bet)
                        .WithMany(betEntity => betEntity.Odds)
                        .HasForeignKey(oddEntity => oddEntity.BetID)
                        .HasPrincipalKey(betEntity => betEntity.ID);

        }
    }
}
