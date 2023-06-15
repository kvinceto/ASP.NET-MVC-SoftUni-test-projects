namespace Library.Services
{
    using Library.Data;
    using Library.Data.Models;
    using Library.Models.BookViewModels;
    using Library.Models.CategoriesViewModels;
    using Library.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class BookServices : IBookService
    {
        private readonly LibraryDbContext dbContext;

        public BookServices(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<BookViewModel>> GetAllBooksAsync()
        {
            return await dbContext.Books
                .Include(b => b.Category)
                .AsNoTracking()
                .Select(b => new BookViewModel()
                {
                    Id = b.Id,
                    Img = b.ImageUrl,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    Rating = b.Rating.ToString(),
                    Category = b.Category.Name
                })
                .ToListAsync();
        }

        public async Task AddBookToCollectionAsync(int id, string userId)
        {
            IdentityUserBook? userBook = await dbContext.IdentityUsersBooks
                .FirstOrDefaultAsync(iu => iu.BookId == id && iu.CollectorId == userId);

            if (userBook == null)
            {
                IdentityUserBook identityUserBook = new IdentityUserBook()
                {
                    CollectorId = userId,
                    BookId = id
                };

                await dbContext.IdentityUsersBooks.AddAsync(identityUserBook);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<ICollection<BookViewModel>> GetAllMyBooksAsync(string userId)
        {
            return await dbContext.IdentityUsersBooks
                .Include(b => b.Book)
                .AsNoTracking()
                .Where(iub => iub.CollectorId == userId)
                .Select(b => new BookViewModel()
                {
                    Id = b.BookId,
                    Title = b.Book.Title,
                    Author = b.Book.Author,
                    Description = b.Book.Description,
                    Rating = b.Book.Rating.ToString(),
                    Category = b.Book.Category.Name
                })
                .ToListAsync();
        }

        public async Task RemoveFromCollectionAsync(int id, string userId)
        {
            IdentityUserBook userBook = await dbContext.IdentityUsersBooks
                 .FirstAsync(iu => iu.BookId == id && iu.CollectorId == userId);

            dbContext.IdentityUsersBooks.Remove(userBook);
            await dbContext.SaveChangesAsync();
        }

        public async Task<BookAddViewModel> GetAllCategories()
        {
            var categories = await dbContext.Categories
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            return new BookAddViewModel()
            {
                Categories = categories
            };
        }

        public async Task AddBook(BookAddViewModel model)
        {
            Book book = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                Rating = model.Rating,
                CategoryId = model.CategoryId,
                ImageUrl = model.Url
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
        }


    }
}
