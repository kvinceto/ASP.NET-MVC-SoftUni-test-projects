﻿using System.ComponentModel.DataAnnotations;

using static Homies.Common.EntityValidationConstants.Type;

namespace Homies.Data.Models
{
    public class Type
    {
        public Type()
        {
            this.Events = new HashSet<Event>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Event> Events { get; set; }
    }
}
