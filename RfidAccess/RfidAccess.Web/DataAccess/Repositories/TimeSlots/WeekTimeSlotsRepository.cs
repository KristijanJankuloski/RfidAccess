using Microsoft.EntityFrameworkCore;
using RfidAccess.Web.DataAccess.Context;
using RfidAccess.Web.DataAccess.Repositories.Base;
using RfidAccess.Web.Models;

namespace RfidAccess.Web.DataAccess.Repositories.TimeSlots
{
    public sealed class WeekTimeSlotsRepository : BaseRepository<WeekTimeSlots>, IWeekTimeSlotsRepository
    {
        public WeekTimeSlotsRepository(RfidContext context) : base(context)
        {
        }

        public async Task<WeekTimeSlots?> GetFirst()
        {
            return await context.WeekTimeSlots.FirstOrDefaultAsync();
        }
    }
}
