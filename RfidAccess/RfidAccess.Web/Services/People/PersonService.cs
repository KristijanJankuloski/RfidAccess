using RfidAccess.Web.DataAccess.Repositories.People;
using RfidAccess.Web.Models;
using RfidAccess.Web.Services.Buffer;
using RfidAccess.Web.ViewModels.People;

namespace RfidAccess.Web.Services.People
{
    public class PersonService(
        PersonBufferService personBuffer,
        IPersonRepository personRepository) : IPersonService
    {
        private readonly PersonBufferService personBuffer = personBuffer;
        private readonly IPersonRepository personRepository = personRepository;

        public void CreatePerson(PersonCreateViewModel viewModel)
        {
            Person person = new Person()
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                CreatedOn = DateTime.Now
            };

            personBuffer.People.Add(person);
        }
    }
}
