using RfidAccess.Web.DataAccess.Repositories.People;
using RfidAccess.Web.DataAccess.Repositories.Records;
using RfidAccess.Web.Models;
using RfidAccess.Web.Services.Buffer;
using RfidAccess.Web.Services.Schedules;
using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.Schedule;
using RfidAccess.Web.Helpers;
using RfidAccess.Web.ViewModels.Records;
using RfidAccess.Web.Services.HubService;
using RfidAccess.Web.Hub;
using RfidAccess.Web.DataAccess.Repositories.ErrorLogs;
using Microsoft.EntityFrameworkCore;

namespace RfidAccess.Web.Services.Records
{
    public class RecordService(
        PersonBufferService personBuffer,
        IRecordRepository recordRepository,
        IPersonRepository personRepository,
        IScheduleService scheduleService,
        IErrorLogRepository errorLogRepository,
        IHubService hubService) : IRecordService
    {
        private readonly PersonBufferService personBuffer = personBuffer;
        private readonly IRecordRepository recordRepository = recordRepository;
        private readonly IPersonRepository personRepository = personRepository;
        private readonly IScheduleService scheduleService = scheduleService;
        private readonly IErrorLogRepository errorLogRepository = errorLogRepository;
        private readonly IHubService hubService = hubService;

        public async Task<Result<RecordsListViewModel>> GetPaginatedRecords(int skip, int take, string? code)
        {
            int count = 0;
            List<Record> records = [];

            if (!string.IsNullOrWhiteSpace(code))
            {
                count = await recordRepository.CountFiltered(query => query.Where(x => x.Code == code));
                records = await recordRepository.Filter(query => query
                .Include(x => x.Person)
                .Where(x => x.Code == code)
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(take));
            }
            else
            {
                count = await recordRepository.Count();
                records = await recordRepository.GetRange(skip, take);
            }
            RecordsListViewModel model = new RecordsListViewModel
            {
                Total = count,
                Skip = skip,
                Take = take,
                Code = code,
                Records = records.Select(x => new RecordViewModel
                {
                    Id = x.Id,
                    FirstName = x.Person?.FirstName,
                    LastName = x.Person?.LastName,
                    Code = x.Code,
                    Time = x.Time
                }).ToList()
            };

            return new Result<RecordsListViewModel>(model);
        }

        public async Task<Result> InsertCode(string code)
        {
            DateTime now = DateTime.Now;
            Person? person = await personRepository.GetByCode(code);
            if (person == null)
            {
                Person? personToInsert = personBuffer.People.FirstOrDefault();
                if (personToInsert == null)
                {
                    NotificationDto dto = new NotificationDto
                    {
                        Message = "Непозната картичка",
                        Code = code,
                        Date = now.ToString("HH:mm dd.MM.yyyy")
                    };
                    await hubService.SendNotification(dto);
                    errorLogRepository.Create(new ErrorLog
                    {
                        Code = code,
                        Message = dto.Message,
                        CreatedOn = now
                    });
                    await errorLogRepository.SaveChanges();
                    return Result.Failure("CODE_NOT_FOUND");
                }

                personToInsert.Code = code;
                personRepository.Create(personToInsert);
                await personRepository.SaveChanges();
                personBuffer.People.Remove(personToInsert);
                await hubService.SendConfirmation(code);
                return Result.Failure("PERSON_INSERTED");
            }

            if (personBuffer.People.Count > 0)
            {
                await hubService.SendError(code, $"Корисник со картичката веќе постои: {person.FirstName} {person.LastName}");
                return Result.Failure("INSERT_FAILED");
            }

            if (person.IsWhitelisted)
            {
                Record r = new Record
                {
                    PersonId = person.Id,
                    Code = code,
                    Time = now
                };

                recordRepository.Create(r);
                await recordRepository.SaveChanges();
                return Result.Success;
            }

            Result<TimeSlotViewModel> timeSlotvm = await scheduleService.GetTimeSlots();
            if (timeSlotvm.IsFailed || timeSlotvm.Value == null)
            {
                NotificationDto dto = new NotificationDto
                {
                    Message = $"{person.FirstName} {person.LastName} - Нема пронајден термин",
                    Code = code,
                    Date = now.ToString("HH:mm dd.MM.yyyy")
                };
                await hubService.SendNotification(dto);
                errorLogRepository.Create(new ErrorLog
                {
                    Code = code,
                    Message = dto.Message,
                    CreatedOn = now,
                    PersonId = person.Id
                });
                await errorLogRepository.SaveChanges();
                return Result.Failure("NOT_FOUND");
            }

            ConvertedTimeSlot? activeTimeSlot = TimeSlotHelper.GetActiveTimeSlot(timeSlotvm.Value, now);
            if (activeTimeSlot == null)
            {
                NotificationDto dto = new NotificationDto
                {
                    Message = $"{person.FirstName} {person.LastName} - Погрешен термин",
                    Code = code,
                    Date = now.ToString("HH:mm dd.MM.yyyy")
                };
                await hubService.SendNotification(dto);
                errorLogRepository.Create(new ErrorLog
                {
                    Code = code,
                    Message = dto.Message,
                    CreatedOn = now,
                    PersonId = person.Id
                });
                await errorLogRepository.SaveChanges();
                return Result.Failure("NO_ACTIVE_TIME_SLOT");
            }

            List<Record> personRecords = await recordRepository.Filter(q => q
                .Where(x => x.Code == code &&
                    x.Time >= activeTimeSlot.Start
                    && x.Time <= activeTimeSlot.End));

            if (personRecords.Count >= activeTimeSlot.Allowed)
            {
                NotificationDto dto = new NotificationDto
                {
                    Message = $"{person.FirstName} {person.LastName} - Веќе искористен",
                    Code = code,
                    Date = now.ToString("HH:mm dd.MM.yyyy")
                };
                await hubService.SendNotification(dto);
                errorLogRepository.Create(new ErrorLog
                {
                    Code = code,
                    Message = dto.Message,
                    CreatedOn = now,
                    PersonId = person.Id
                });
                await errorLogRepository.SaveChanges();
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
