namespace RfidAccess.Web.DataAccess.Context
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using RfidAccess.Web.Models;

    public class RfidContext : IdentityDbContext<User>
    {
        public DbSet<Person> People { get; set; }

        public DbSet<Record> Records { get; set; }

        public DbSet<WeekTimeSlots> WeekTimeSlots { get; set; }

        public DbSet<ErrorLog> ErrorLogs { get; set; }

        public RfidContext(DbContextOptions<RfidContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Record>()
                .HasOne(r => r.Person)
                .WithMany(r => r.Records)
                .HasForeignKey(r => r.PersonId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ErrorLog>()
                .HasOne(x => x.Person)
                .WithMany()
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
