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
                FirstName = viewModel.FirstName.Trim(),
                LastName = viewModel.LastName.Trim(),
                CreatedOn = DateTime.Now
            };

            personBuffer.People.Add(person);
            return Result.Success;
        }

        public void DeleteFromBuffer(string? firstName, string? lastName)
        {
            Person? person = personBuffer.People.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            if (person != null)
            {
                personBuffer.People.Remove(person);
            }
        }

        public async Task<Result> DeletePerson(int id)
        {
            Person? person = await personRepository.GetById(id);
            if (person == null)
            {
                return Result.Failure("Нема пронајден корисник.");
            }

            personRepository.Delete(person);
            await personRepository.SaveChanges();
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
                Code = x.Code,
                IsWhitelisted = x.IsWhitelisted
            }).ToList();

            List<PersonViewModel> buffer = personBuffer.People.Select(x => new PersonViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CreatedOn = x.CreatedOn,
                Code = x.Code,
                IsWhitelisted = x.IsWhitelisted
            }).ToList();

            PersonCombinedViewModel combined = new PersonCombinedViewModel
            {
                Buffer = buffer,
                People = response
            };

            return new Result<PersonCombinedViewModel>(combined);
        }

        public async Task<Result<PersonCombinedViewModel>> GetPaginated(int skip, int take, string? firstName, string? lastName, string? code)
        {
            int count = 0;
            List<Person> people = [];

            if (!string.IsNullOrWhiteSpace(firstName)
                || !string.IsNullOrWhiteSpace(lastName)
                || !string.IsNullOrWhiteSpace(code))
            {
                count = await personRepository.CountFilter(firstName, lastName, code);
                people = await personRepository.GetFiltered(firstName, lastName, code, skip, take);
            }
            else
            {
                count = await personRepository.Count();
                people = await personRepository.GetRange(skip, take);
            }

            List<PersonViewModel> response = people.Select(x => new PersonViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CreatedOn = x.CreatedOn,
                Code = x.Code,
                IsWhitelisted = x.IsWhitelisted
            }).ToList();

            List<PersonViewModel> buffer = personBuffer.People.Select(x => new PersonViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CreatedOn = x.CreatedOn,
                Code = x.Code,
                IsWhitelisted = x.IsWhitelisted
            }).ToList();

            PersonCombinedViewModel combined = new PersonCombinedViewModel
            {
                Buffer = buffer,
                People = response,
                Total = count,
                Skip = skip,
                Take = take,
                FirstName = firstName,
                LastName = lastName,
                Code = code
            };

            return new Result<PersonCombinedViewModel>(combined);
        }

        public async Task<Result> ToggleWhiteListPerson(int id)
        {
            Person? person = await personRepository.GetById(id);
            if (person == null)
            {
                return Result.Failure("Нема пронајден корисник.");
            }

            person.IsWhitelisted = !person.IsWhitelisted;
            personRepository.Update(person);
            await personRepository.SaveChanges();
            return Result.Success;
        }
    }
}
