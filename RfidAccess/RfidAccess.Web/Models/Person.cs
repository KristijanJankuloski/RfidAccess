using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RfidAccess.Web.Models
{
    [Index(nameof(Code))]
    public class Person : BaseEntity
    {
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Code { get; set; }

        public DateTime CreatedOn { get; set; }

        [DefaultValue(false)]
        public bool IsWhitelisted { get; set; }

        public ICollection<Record> Records { get; set; } = [];
    }
}
