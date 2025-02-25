﻿using Newtonsoft.Json;
using RfidAccess.Web.DataAccess.Repositories.TimeSlots;
using RfidAccess.Web.Helpers;
using RfidAccess.Web.Models;
using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.Schedule;
using System.Net.NetworkInformation;

namespace RfidAccess.Web.Services.Schedules
{
    public class ScheduleService(IWeekTimeSlotsRepository weekTimeSlotsRepository) : IScheduleService
    {
        private readonly IWeekTimeSlotsRepository weekTimeSlotsRepository = weekTimeSlotsRepository;

        public async Task<Result<ActiveTimeSlotViewModel>> GetActiveAndNext()
        {
            Result<TimeSlotViewModel> allTimeSlots = await GetTimeSlots();
            if (allTimeSlots.IsFailed || allTimeSlots.Value == null)
            {
                return new Result<ActiveTimeSlotViewModel>("NO_SLOT");
            }

            ConvertedTimeSlot? convertedTimeSlot = TimeSlotHelper.GetActiveTimeSlot(allTimeSlots.Value, DateTime.Now);
            ConvertedTimeSlot? nextSlot = TimeSlotHelper.GetDaySlots(allTimeSlots.Value, DateTime.Now)
                .Where(x => x.Start > convertedTimeSlot?.End)
                .FirstOrDefault();

            return new Result<ActiveTimeSlotViewModel>(new ActiveTimeSlotViewModel
            {
                Active = convertedTimeSlot,
                Next = nextSlot
            });
        }

        public async Task<Result<ConvertedTimeSlot>> GetActiveTimeSlot()
        {
            Result<TimeSlotViewModel> allTimeSlots = await GetTimeSlots();
            if (allTimeSlots.IsFailed || allTimeSlots.Value == null)
            {
                return new Result<ConvertedTimeSlot>("NO_SLOT");
            }

            ConvertedTimeSlot? convertedTimeSlot = TimeSlotHelper.GetActiveTimeSlot(allTimeSlots.Value, DateTime.Now);
            if (convertedTimeSlot == null)
            {
                return new Result<ConvertedTimeSlot>("NO_SLOT");
            }

            return new Result<ConvertedTimeSlot>(convertedTimeSlot);
        }

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
                Sunday = ParseTimeSlots(weekTimeSlots.Sunday),
                LastModified = weekTimeSlots.LastModified
            };
            return new Result<TimeSlotViewModel>(timeSlotViewModel);
        }

        public async Task<Result> UpdateTimeSlots(TimeSlotViewModel viewModel)
        {
            if (ValidateSlotsInDay(viewModel.Monday)
                || ValidateSlotsInDay(viewModel.Tuesday)
                || ValidateSlotsInDay(viewModel.Wednesday)
                || ValidateSlotsInDay(viewModel.Thursday)
                || ValidateSlotsInDay(viewModel.Friday)
                || ValidateSlotsInDay(viewModel.Saturday)
                || ValidateSlotsInDay(viewModel.Sunday))
            {
                return Result.Failure("Невалидно време");
            }

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
                existingSlots.LastModified = DateTime.Now;

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
                Sunday = sunday,
                LastModified = DateTime.Now
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

        private bool InvalidTimeSpan(string start, string end)
        {
            TimeSpan startTime = TimeSpan.Parse(start);
            TimeSpan endTime = TimeSpan.Parse(end);

            return endTime <= startTime;
        }

        private bool ValidateSlotsInDay(List<TimeSlot> slots)
        {
            foreach (var slot in slots)
            {
                if (InvalidTimeSpan(slot.Start, slot.End))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
