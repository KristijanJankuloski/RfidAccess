namespace RfidAccess.Web.ViewModels.People
{
    public class PersonCombinedViewModel
    {
        public List<PersonViewModel> Buffer { get; set; } = [];

        public List<PersonViewModel> People { get; set; } = [];

        public int? Skip { get; set; } = 0;

        public int? Total { get; set; } = 0;

        public int? Take { get; set; } = 10;
    }
}
