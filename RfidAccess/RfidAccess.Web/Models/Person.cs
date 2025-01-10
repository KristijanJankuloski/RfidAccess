using System.ComponentModel.DataAnnotations;

namespace RfidAccess.Web.Models
{
    public class Person : BaseEntity
    {
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Code { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<Record> Records { get; set; } = [];
    }
}
