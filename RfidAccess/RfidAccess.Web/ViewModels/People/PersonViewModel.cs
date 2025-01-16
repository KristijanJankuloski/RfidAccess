namespace RfidAccess.Web.ViewModels.People
{
    public class PersonViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Code { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsWhitelisted { get; set; }
    }
}
