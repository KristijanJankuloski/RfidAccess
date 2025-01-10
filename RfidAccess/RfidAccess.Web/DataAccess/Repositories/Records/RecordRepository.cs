using RfidAccess.Web.DataAccess.Context;
using RfidAccess.Web.DataAccess.Repositories.Base;
using RfidAccess.Web.Models;

namespace RfidAccess.Web.DataAccess.Repositories.Records
{
    public sealed class RecordRepository(RfidContext context)
        : BaseRepository<Record>(context), IRecordRepository
    {
    }
}
