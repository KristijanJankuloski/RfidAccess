using RfidAccess.Web.DataAccess.Repositories.People;
using RfidAccess.Web.DataAccess.Repositories.Records;
using RfidAccess.Web.Models;
using RfidAccess.Web.Services.Buffer;
using RfidAccess.Web.ViewModels.Base;

namespace RfidAccess.Web.Services.Records
{
    public class RecordService(
        PersonBufferService personBuffer,
        IRecordRepository recordRepository,
        IPersonRepository personRepository) : IRecordService
    {
        private readonly PersonBufferService personBuffer = personBuffer;
        private readonly IRecordRepository recordRepository = recordRepository;
        private readonly IPersonRepository personRepository = personRepository;

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
                return Result.Failure("PERSON_INSERTED");
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
