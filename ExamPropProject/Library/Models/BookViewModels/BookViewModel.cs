namespace Library.Models.BookViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string? Description { get; set; }

        public string Category { get; set; } = null!;

        public string? Rating { get; set; }

        public string Img { get; set; } = null!;
    }
}
