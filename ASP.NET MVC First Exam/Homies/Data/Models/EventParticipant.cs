﻿using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Homies.Data.Models
{
    public class EventParticipant
    {
        [ForeignKey(nameof(Helper))]
        public string HelperId { get; set; } = null!;

        public virtual IdentityUser Helper { get; set; } = null!;

        [ForeignKey(nameof(Event))]
        public int EventId { get; set; }

        public virtual Event Event { get; set; } = null!;
    }
}
