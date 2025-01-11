namespace RfidAccess.Web.DataAccess.Context
{
    using Microsoft.EntityFrameworkCore;
    using RfidAccess.Web.Models;

    public class RfidContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public DbSet<Record> Records { get; set; }

        public DbSet<WeekTimeSlots> WeekTimeSlots { get; set; }

        public RfidContext(DbContextOptions<RfidContext> options)
            : base(options)
        {
        }
    }
}
