using Newtonsoft.Json;
using RfidAccess.Web.DataAccess.Repositories.TimeSlots;
using RfidAccess.Web.Models;
using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.Schedule;

namespace RfidAccess.Web.Services.Schedules
{
    public class ScheduleService(IWeekTimeSlotsRepository weekTimeSlotsRepository) : IScheduleService
    {
        private readonly IWeekTimeSlotsRepository weekTimeSlotsRepository = weekTimeSlotsRepository;

        public async Task<Result<TimeSlotViewModel>> GetTimeSlots()
        {
            WeekTimeSlots? weekTimeSlots = await weekTimeSlotsRepository.GetFirst();
            if (weekTimeSlots == null)
            {
                return new Result<TimeSlotViewModel>("Not found");
            }

            TimeSlotViewModel timeSlotViewModel = new TimeSlotViewModel
            {
                Monday = ParseTimeSlots(weekTimeSlots.Monday),
                Tuesday = ParseTimeSlots(weekTimeSlots.Tuesday),
                Wednesday = ParseTimeSlots(weekTimeSlots.Wednesday),
                Thursday = ParseTimeSlots(weekTimeSlots.Thursday),
                Friday = ParseTimeSlots(weekTimeSlots.Friday),
                Saturday = ParseTimeSlots(weekTimeSlots.Saturday),
                Sunday = ParseTimeSlots(weekTimeSlots.Sunday)
            };
            return new Result<TimeSlotViewModel>(timeSlotViewModel);
        }

        public async Task<Result> UpdateTimeSlots(TimeSlotViewModel viewModel)
        {
            string monday = JsonConvert.SerializeObject(viewModel.Monday);
            string tuesday = JsonConvert.SerializeObject(viewModel.Tuesday);
            string wednesday = JsonConvert.SerializeObject(viewModel.Wednesday);
            string thursday = JsonConvert.SerializeObject(viewModel.Thursday);
            string friday = JsonConvert.SerializeObject(viewModel.Friday);
            string saturday = JsonConvert.SerializeObject(viewModel.Saturday);
            string sunday = JsonConvert.SerializeObject(viewModel.Sunday);

            WeekTimeSlots? existingSlots = await weekTimeSlotsRepository.GetFirst();
            if (existingSlots != null)
            {
                existingSlots.Monday = monday;
                existingSlots.Tuesday = tuesday;
                existingSlots.Wednesday = wednesday;
                existingSlots.Thursday = thursday;
                existingSlots.Friday = friday;
                existingSlots.Saturday = saturday;
                existingSlots.Sunday = sunday;

                weekTimeSlotsRepository.Update(existingSlots);
                await weekTimeSlotsRepository.SaveChanges();
                return Result.Success;
            }

            WeekTimeSlots newSlots = new WeekTimeSlots
            {
                Monday = monday,
                Tuesday = tuesday,
                Wednesday = wednesday,
                Thursday = thursday,
                Friday = friday,
                Saturday = saturday,
                Sunday = sunday
            };
            weekTimeSlotsRepository.Create(newSlots);
            await weekTimeSlotsRepository.SaveChanges();
            return Result.Success;
        }

        private List<TimeSlot> ParseTimeSlots(string daySlots)
        {
            if (string.IsNullOrWhiteSpace(daySlots))
            {
                return new List<TimeSlot>();
            }

            try
            {
                List<TimeSlot>? timeSlots = JsonConvert.DeserializeObject<List<TimeSlot>>(daySlots);
                if (timeSlots == null)
                {
                    return new List<TimeSlot>();
                }

                return timeSlots;
            }
            catch (Exception ex)
            {
                return new List<TimeSlot>();
            }
        }
    }
}
