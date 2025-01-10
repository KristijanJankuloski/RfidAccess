namespace RfidAccess.Web.DataAccess.Repositories.People
{
    using RfidAccess.Web.DataAccess.Context;
    using RfidAccess.Web.DataAccess.Repositories.Base;
    using RfidAccess.Web.Models;

    public sealed class PersonRepository(RfidContext context)
        : BaseRepository<Person>(context), IPersonRepository
    {
    }
}
