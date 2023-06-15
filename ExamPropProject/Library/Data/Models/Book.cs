namespace Library.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using static Library.Common.ValidationConstants.Book;

    [Comment("Book in the library")]
    public class Book
    {
        public Book()
        {
            this.UsersBooks = new HashSet<IdentityUserBook>();
        }

        [Comment("Book Identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Title of the book")]
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Comment("Author of the book")]
        [Required]
        [MaxLength(AuthorMaxLength)]
        public string Author { get; set; } = null!;

        [Comment("Description of the book")]
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Comment("Link to image of the book")]
        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public decimal Rating { get; set; }

        [Comment("Id of the category of the book")]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<IdentityUserBook> UsersBooks { get; set; }
    }
}
