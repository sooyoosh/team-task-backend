using Microsoft.EntityFrameworkCore;
using TeamTaskManager.Entities;

namespace TeamTaskManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        // DbSets — جداول
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TeamInvitation> TeamInvitation { get; set; }


        //Fluent Api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>()
     .HasOne(t => t.Owner)
     .WithMany(u => u.OwnedTeams)
     .HasForeignKey(t => t.OwnerId)
     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamMember>()
                .HasKey(tm => new { tm.UserId, tm.TeamId });

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.User)
                .WithMany(u => u.Teams)
                .HasForeignKey(tm => tm.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TeamInvitation>()
    .HasOne(inv => inv.InvitedUser)
    .WithMany() 
    .HasForeignKey(inv => inv.InvitedUserId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamInvitation>()
                .HasOne(inv => inv.InvitedByUser)
                .WithMany() 
                .HasForeignKey(inv => inv.InvitedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamInvitation>()
                .HasOne(inv => inv.Team)
                .WithMany() 
                .HasForeignKey(inv => inv.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
