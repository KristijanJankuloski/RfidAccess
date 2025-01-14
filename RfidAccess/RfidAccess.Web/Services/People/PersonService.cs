using RfidAccess.Web.DataAccess.Repositories.People;
using RfidAccess.Web.Models;
using RfidAccess.Web.Services.Buffer;
using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.People;

namespace RfidAccess.Web.Services.People
{
    public class PersonService(
        PersonBufferService personBuffer,
        IPersonRepository personRepository) : IPersonService
    {
        private readonly PersonBufferService personBuffer = personBuffer;
        private readonly IPersonRepository personRepository = personRepository;

        public Result CreatePerson(PersonCreateViewModel viewModel)
        {
            Person person = new Person()
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                CreatedOn = DateTime.Now
            };

            personBuffer.People.Add(person);
            return Result.Success;
        }

        public async Task<Result<PersonCombinedViewModel>> GetAllPeople()
        {
            List<Person> people = await personRepository.GetAll();

            List<PersonViewModel> response = people.Select(x => new PersonViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CreatedOn = x.CreatedOn,
                Code = x.Code
            }).ToList();

            List<PersonViewModel> buffer = personBuffer.People.Select(x => new PersonViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CreatedOn = x.CreatedOn,
                Code = x.Code
            }).ToList();

            PersonCombinedViewModel combined = new PersonCombinedViewModel
            {
                Buffer = buffer,
                People = response
            };

            return new Result<PersonCombinedViewModel>(combined);
        }

        public async Task<Result<PersonCombinedViewModel>> GetPaginated(int skip, int take)
        {
            int count = await personRepository.Count();
            List<Person> people = await personRepository.GetRange(skip, take);

            List<PersonViewModel> response = people.Select(x => new PersonViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CreatedOn = x.CreatedOn,
                Code = x.Code
            }).ToList();

            List<PersonViewModel> buffer = personBuffer.People.Select(x => new PersonViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CreatedOn = x.CreatedOn,
                Code = x.Code
            }).ToList();

            PersonCombinedViewModel combined = new PersonCombinedViewModel
            {
                Buffer = buffer,
                People = response,
                Total = count,
                Skip = skip,
                Take = take
            };

            return new Result<PersonCombinedViewModel>(combined);
        }
    }
}
