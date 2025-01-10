using RfidAccess.Web.ViewModels.People;

namespace RfidAccess.Web.Services.People
{
    public interface IPersonService
    {
        void CreatePerson(PersonCreateViewModel viewModel);
    }
}
