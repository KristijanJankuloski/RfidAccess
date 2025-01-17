namespace RfidAccess.Web.ViewModels.Records
{
    public class RecordsListViewModel
    {
        public int? Total { get; set; } = 0;

        public int? Skip { get; set; } = 0;

        public int? Take { get; set; } = 10;

        public List<RecordViewModel> Records { get; set; } = [];

        public string? Code { get; set; }
    }

    public class RecordViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public DateTime Time { get; set; }

        public int PersonId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }
}
