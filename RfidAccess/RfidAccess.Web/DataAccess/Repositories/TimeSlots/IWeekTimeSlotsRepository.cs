using RfidAccess.Web.DataAccess.Repositories.Base;
using RfidAccess.Web.Models;

namespace RfidAccess.Web.DataAccess.Repositories.TimeSlots
{
    public interface IWeekTimeSlotsRepository : IRepository<WeekTimeSlots>
    {
        Task<WeekTimeSlots?> GetFirst();
    }
}
