namespace Library.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Library.Services.Interfaces;
    using System.Security.Claims;
    using Library.Models.BookViewModels;

    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public async Task<IActionResult> All()
        {
            var books = await bookService.GetAllBooksAsync();
            return View("All", books);
        }

        public async Task<IActionResult> Mine()
        {
            var books = await bookService.GetAllMyBooksAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return View("Mine", books);
        }

        public async Task<IActionResult> AddToCollection(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await bookService.AddBookToCollectionAsync(id, userId);

            return RedirectToAction("All", "Book");
        }

        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await bookService.RemoveFromCollectionAsync(id, userId);

            return RedirectToAction("Mine", "Book");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = await bookService.GetAllCategories();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await bookService.AddBook(model);

            return RedirectToAction("All", "Book");
        }
    }
}
