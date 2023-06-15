namespace Library.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Library.Common.ValidationConstants.Category;

    [Comment("Category of the books")]
    public class Category
    {
        public Category()
        {
            this.Books = new HashSet<Book>();
        }

        [Comment("Category identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of the category")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}