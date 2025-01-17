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
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
