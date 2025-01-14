using RfidAccess.Web.ViewModels.Schedule;
using System.Linq;

namespace RfidAccess.Web.Helpers
{
    public static class TimeSlotHelper
    {
        public static ConvertedTimeSlot? GetActiveTimeSlot(TimeSlotViewModel vm, DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return ExtractTimeSlot(vm.Monday, date);
                case DayOfWeek.Tuesday:
                    return ExtractTimeSlot(vm.Tuesday, date);
                case DayOfWeek.Wednesday:
                    return ExtractTimeSlot(vm.Wednesday, date);
                case DayOfWeek.Thursday:
                    return ExtractTimeSlot(vm.Thursday, date);
                case DayOfWeek.Friday:
                    return ExtractTimeSlot(vm.Friday, date);
                case DayOfWeek.Saturday:
                    return ExtractTimeSlot(vm.Saturday, date);
                case DayOfWeek.Sunday:
                    return ExtractTimeSlot(vm.Sunday, date);
                default:
                    return null;
            }
        }

        public static List<ConvertedTimeSlot> GetDaySlots(TimeSlotViewModel vm, DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return ExtractTimeSlots(vm.Monday, date);
                case DayOfWeek.Tuesday:
                    return ExtractTimeSlots(vm.Tuesday, date);
                case DayOfWeek.Wednesday:
                    return ExtractTimeSlots(vm.Wednesday, date);
                case DayOfWeek.Thursday:
                    return ExtractTimeSlots(vm.Thursday, date);
                case DayOfWeek.Friday:
                    return ExtractTimeSlots(vm.Friday, date);
                case DayOfWeek.Saturday:
                    return ExtractTimeSlots(vm.Saturday, date);
                case DayOfWeek.Sunday:
                    return ExtractTimeSlots(vm.Sunday, date);
                default:
                    return [];
            }
        }

        private static ConvertedTimeSlot? ExtractTimeSlot(List<TimeSlot> slots, DateTime date)
        {
            return slots.Select(x => new ConvertedTimeSlot(x.Start, x.End, date, x.Allow))
                .FirstOrDefault(x => x.Start <= date && x.End >= date);
        }

        private static List<ConvertedTimeSlot> ExtractTimeSlots(List<TimeSlot> slots, DateTime date)
        {
            return slots.Select(x => new ConvertedTimeSlot(x.Start, x.End, date, x.Allow)).ToList();
        }
    }

    public class ConvertedTimeSlot
    {
        public ConvertedTimeSlot(string startTime, string endTime, DateTime date, int allowed)
        {
            var startTimes = startTime.Split(':').Select(x => int.TryParse(x, out int result) ? result : 0);
            Start = new DateTime(date.Year, date.Month, date.Day, startTimes.FirstOrDefault(), startTimes.LastOrDefault(), 0);

            var endTimes = endTime.Split(':').Select(x => int.TryParse(x, out int result) ? result : 0);
            End = new DateTime(date.Year, date.Month, date.Day, endTimes.FirstOrDefault(), endTimes.LastOrDefault(), 0);

            Allowed = allowed;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int Allowed { get; set; }
    }
}
