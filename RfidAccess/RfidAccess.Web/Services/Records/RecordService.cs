using RfidAccess.Web.DataAccess.Repositories.People;
using RfidAccess.Web.DataAccess.Repositories.Records;
using RfidAccess.Web.Models;
using RfidAccess.Web.Services.Buffer;
using RfidAccess.Web.Services.Schedules;
using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.Schedule;
using RfidAccess.Web.Helpers;
using RfidAccess.Web.ViewModels.Records;

namespace RfidAccess.Web.Services.Records
{
    public class RecordService(
        PersonBufferService personBuffer,
        IRecordRepository recordRepository,
        IPersonRepository personRepository,
        IScheduleService scheduleService) : IRecordService
    {
        private readonly PersonBufferService personBuffer = personBuffer;
        private readonly IRecordRepository recordRepository = recordRepository;
        private readonly IPersonRepository personRepository = personRepository;
        private readonly IScheduleService scheduleService = scheduleService;

        public async Task<Result<RecordsListViewModel>> GetPaginatedRecords(int skip, int take)
        {
            int count = await recordRepository.Count();
            List<Record> records = await recordRepository.GetRange(skip, take);

            RecordsListViewModel model = new RecordsListViewModel
            {
                Total = count,
                Skip = skip,
                Take = take,
                Records = records.Select(x => new RecordViewModel
                {
                    Id = x.Id,
                    FirstName = x.Person?.FirstName ?? string.Empty,
                    LastName = x.Person?.LastName ?? string.Empty,
                    Code = x.Code,
                    Time = x.Time
                }).ToList()
            };

            return new Result<RecordsListViewModel>(model);
        }

        public async Task<Result> InsertCode(string code)
        {
            DateTime now = DateTime.Now;
            Person? person = (await personRepository.Filter(query => query.Where(x => x.Code == code))).FirstOrDefault();
            if (person == null)
            {
                Person? personToInsert = personBuffer.People.FirstOrDefault();
                if (personToInsert == null)
                    return Result.Failure("CODE_NOT_FOUND");

                personToInsert.Code = code;
                personRepository.Create(personToInsert);
                await personRepository.SaveChanges();
                personBuffer.People.Remove(personToInsert);
                return Result.Failure("PERSON_INSERTED");
            }

            Result<TimeSlotViewModel> timeSlotvm = await scheduleService.GetTimeSlots();
            if (timeSlotvm.IsFailed || timeSlotvm.Value == null)
            {
                return Result.Failure("NOT_FOUND");
            }

            ConvertedTimeSlot? activeTimeSlot = TimeSlotHelper.GetActiveTimeSlot(timeSlotvm.Value, now);
            if (activeTimeSlot == null)
            {
                return Result.Failure("NO_ACTIVE_TIME_SLOT");
            }

            List<Record> personRecords = await recordRepository.Filter(q => q
                .Where(x => x.Code == code &&
                    x.Time >= activeTimeSlot.Start
                    && x.Time <= activeTimeSlot.End));

            if (personRecords.Count >= activeTimeSlot.Allowed)
            {
                return Result.Failure("DENIED");
            }

            Record record = new Record
            {
                PersonId = person.Id,
                Code = code,
                Time = now
            };

            recordRepository.Create(record);
            await recordRepository.SaveChanges();
            return Result.Success;
        }
    }
}
