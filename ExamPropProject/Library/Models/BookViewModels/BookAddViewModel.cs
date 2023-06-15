namespace Library.Models.BookViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Library.Models.CategoriesViewModels;

    using static Library.Common.ValidationConstants.Book;

    public class BookAddViewModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(AuthorMaxLength, MinimumLength = AuthorMinLength)]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!;

        [Range(RatingMinValue, RatingMaxValue)]
        public decimal Rating { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
