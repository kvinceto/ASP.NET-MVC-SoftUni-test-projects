namespace Library.Services.Interfaces
{
    using Library.Models.BookViewModels;

    public interface IBookService
    {
        Task<ICollection<BookViewModel>> GetAllBooksAsync();

        Task<ICollection<BookViewModel>> GetAllMyBooksAsync(string userId);

        Task AddBookToCollectionAsync(int id, string userId);

        Task RemoveFromCollectionAsync(int id, string userId);

        Task<BookAddViewModel> GetAllCategories();

        Task AddBook(BookAddViewModel model);
    }
}
