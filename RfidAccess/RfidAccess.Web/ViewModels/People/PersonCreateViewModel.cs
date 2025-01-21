using System.ComponentModel.DataAnnotations;

namespace RfidAccess.Web.ViewModels.People
{
    public class PersonCreateViewModel
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Code { get; set; }
    }
}
