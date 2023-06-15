namespace Library.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    [Comment("Mapping entity between book and user")]
    public class IdentityUserBook
    {
        [Comment("User id")]
        [ForeignKey(nameof(Collector))]
        public string CollectorId { get; set; } = null!;

        public virtual IdentityUser Collector { get; set; } = null!;

        [Comment("Book id")]
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        public virtual Book Book { get; set; } = null!;
    }
}