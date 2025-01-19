using RfidAccess.Web.ViewModels.Base;
using RfidAccess.Web.ViewModels.People;

namespace RfidAccess.Web.Services.People
{
    public interface IPersonService
    {
        Result CreatePerson(PersonCreateViewModel viewModel);

        Task<Result<PersonCombinedViewModel>> GetAllPeople();

        Task<Result<PersonCombinedViewModel>> GetPaginated(int skip, int take, string? firstName, string? lastName, string? code);

        Task<Result> DeletePerson(int id);

        Task<Result> ToggleWhiteListPerson(int id);

        void DeleteFromBuffer(string? firstName, string? lastName);
    }
}
