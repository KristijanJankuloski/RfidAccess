﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RfidAccess.Web.Models
{
    [Index(nameof(Code), nameof(Time))]
    public class Record : BaseEntity
    {
        [MaxLength(150)]
        public string Code { get; set; } = string.Empty;

        public int? PersonId { get; set; }

        [ForeignKey(nameof(PersonId))]
        public virtual Person? Person { get; set; }

        public DateTime Time { get; set; }
    }
}
