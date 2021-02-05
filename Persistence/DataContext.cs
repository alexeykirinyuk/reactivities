using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public sealed class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Value> Values { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<UserActivity> UserActivities { get; set; }

        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Value>()
                .HasData(
                    new Value { Id = 1, Name = "Value #1" },
                    new Value { Id = 2, Name = "Value #2" },
                    new Value { Id = 3, Name = "Value #3" }
                );

            builder.Entity<UserActivity>(e =>
            {
                e.HasKey(ua => new { ua.AppUserId, ua.ActivityId });

                e.HasOne(ua => ua.AppUser)
                    .WithMany(au => au.UserActivities)
                    .HasForeignKey(ua => ua.AppUserId);

                e.HasOne(ua => ua.Activity)
                    .WithMany(a => a.UserActivities)
                    .HasForeignKey(ua => ua.ActivityId);
            });
        }
    }
}
