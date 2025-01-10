namespace RfidAccess.Web.ViewModels.People
{
    public class PersonCombinedViewModel
    {
        public List<PersonViewModel> Buffer { get; set; } = [];

        public List<PersonViewModel> People { get; set; } = [];
    }
}
