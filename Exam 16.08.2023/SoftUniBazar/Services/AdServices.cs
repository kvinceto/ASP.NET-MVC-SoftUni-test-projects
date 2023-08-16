namespace SoftUniBazar.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using SoftUniBazar.Data;
    using SoftUniBazar.Data.Models;
    using SoftUniBazar.Services.Interfaces;
    using SoftUniBazar.ViewModels.AdViewModels;
    using SoftUniBazar.ViewModels.CategoriseViewModels;

    using static SoftUniBazar.Common.EntityValidationConstants;

    using Ad = SoftUniBazar.Data.Models.Ad;

    public class AdServices : IAdServices
    {
        private readonly BazarDbContext dbContext;

        public AdServices(BazarDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Добавя новя обява към базата
        /// </summary>
        /// <param name="viewModel">Модела с данни за добавяне</param>
        /// <param name="myId">Id на собственика на обявата</param>
        public async Task AddNewAsync(AdAddNewViewModel viewModel, string myId)
        {
            string date = DateTime.Now.ToString(DateFormat);

            Ad newAd = new Ad()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                CategoryId = viewModel.CategoryId,
                CreatedOn = DateTime.Parse(date),
                ImageUrl = viewModel.ImageUrl,
                OwnerId = myId
            };

            await dbContext.Ads.AddAsync(newAd);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Добавя дадена обявя към колекцията на User
        /// </summary>
        /// <param name="id">Id на обявата</param>
        /// <param name="myId">Id на User</param>
        /// <exception cref="InvalidOperationException">Хвърля грешка ако е вече добавена тази обява към колекцията на този User</exception>
        public async Task AdToCollectionAsync(int id, string myId)
        {
            bool isCreated = await dbContext.AdsBuyers.AnyAsync(a => a.AdId == id && a.BuyerId == myId);

            if (isCreated)
            {
                throw new InvalidOperationException("already exists");
            }

            await dbContext.AdsBuyers.AddAsync(new AdBuyer()
            {
                AdId = id,
                BuyerId = myId
            });

            await dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Променя данните на дадена обява в базата
        /// </summary>
        /// <param name="viewModel">Новите данни</param>
        /// <param name="myId">Id на User</param>
        /// <exception cref="NullReferenceException">Хвърля грешка ако няма такава обява</exception>
        /// <exception cref="InvalidOperationException">Хвърля грешка ако User не е собственик на обявата</exception>
        public async Task EditAsync(AdEditViewModel viewModel, string myId)
        {
            Ad? adToEdit = await dbContext.Ads
               .FirstOrDefaultAsync(a => a.Id == viewModel.Id);

            if (adToEdit == null)
            {
                throw new NullReferenceException(nameof(adToEdit));
            }

            if (adToEdit.OwnerId != myId)
            {
                throw new InvalidOperationException("now my ad");
            }

            string date = DateTime.Now.ToString(DateFormat);

            adToEdit.Name = viewModel.Name;
            adToEdit.Description = viewModel.Description;
            adToEdit.Price = viewModel.Price;
            adToEdit.OwnerId = myId;
            adToEdit.CategoryId = viewModel.CategoryId;
            adToEdit.ImageUrl = viewModel.ImageUrl;
            adToEdit.CreatedOn = DateTime.Parse(date);

            await dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Извиква всички обяви в базата
        /// </summary>
        /// <returns>Калекция от всички обяви в базата</returns>
        public async Task<ICollection<AdAllViewModel>> GetAllAdsAsync()
        {
            return await dbContext.Ads
                .Include(a => a.Category)
                .Include(a => a.Owner)
                 .Select(a => new AdAllViewModel
                 {
                     Id = a.Id,
                     Name = a.Name,
                     Description = a.Description,
                     Category = a.Category.Name,
                     Owner = a.Owner.UserName,
                     CreatedOn = a.CreatedOn.ToString(DateFormat),
                     ImageUrl = a.ImageUrl,
                     Price = $"{a.Price:f2}"
                 })
                 .ToArrayAsync();
        }


        /// <summary>
        /// Извиква всички обяви в базата на даден User
        /// </summary>
        /// <param name="myId">Id ня User</param>
        /// <returns>Калекция от всички обяви в базата за даден User</returns>
        public async Task<ICollection<AdAllViewModel>> GetAllMine(string myId)
        {
            return await dbContext.AdsBuyers
                 .Include(a => a.Ad)
                 .ThenInclude(a => a.Category)
                 .Include(a => a.Buyer)
                 .Where(a => a.BuyerId == myId)
                 .Select(a => new AdAllViewModel()
                 {
                     Id = a.Ad.Id,
                     Name = a.Ad.Name,
                     Description = a.Ad.Description,
                     Category = a.Ad.Category.Name,
                     Owner = a.Ad.Owner.UserName,
                     CreatedOn = a.Ad.CreatedOn.ToString(DateFormat),
                     ImageUrl = a.Ad.ImageUrl,
                     Price = $"{a.Ad.Price:f2}"
                 })
                 .ToArrayAsync();
        }

        /// <summary>
        /// Генерира модел за нова обява
        /// </summary>
        /// <returns>Модел за нова обява и всички категории в базата</returns>
        public async Task<AdAddNewViewModel> GetModelForAdd()
        {
            AdAddNewViewModel model = new AdAddNewViewModel();
            model.Categories = await dbContext.Categories
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToArrayAsync();

            return model;
        }

        /// <summary>
        /// Генерира модел за промяна на обява
        /// </summary>
        /// <param name="id">Id на обявата</param>
        /// <returns>Модел за промяна обява и всички категории в базата</returns>
        /// <exception cref="NullReferenceException">Хвърля грешка ако няма такава обява</exception>
        public async Task<AdEditViewModel> GetModelForEdit(int id)
        {
            Ad? adToEdit = await dbContext.Ads
                .FirstOrDefaultAsync(a => a.Id == id);

            if (adToEdit == null)
            {
                throw new NullReferenceException(nameof(adToEdit));
            }

            var model = new AdEditViewModel()
            {
                Id = adToEdit.Id,
                Name = adToEdit.Name,
                Description = adToEdit.Description,
                Price = adToEdit.Price,
                ImageUrl = adToEdit.ImageUrl,
                CategoryId = adToEdit.CategoryId,
                Categories = await dbContext.Categories
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToArrayAsync()
            };

            return model;
        }

        /// <summary>
        /// Премахва дадена обява от колекцията на User
        /// </summary>
        /// <param name="myId">Id на User</param>
        /// <param name="id">Id на обявата</param>
        /// <exception cref="InvalidOperationException">Хвърля грешка ако няма такава обява в колекцията</exception>
        public async Task RemoveFromCartAsync(string myId, int id)
        {
            bool isCreated = await dbContext.AdsBuyers.AnyAsync(a => a.AdId == id && a.BuyerId == myId);

            if (!isCreated)
            {
                throw new InvalidOperationException("not created");
            }

            AdBuyer? modelToRemove = await dbContext.AdsBuyers
                .FirstOrDefaultAsync(a => a.AdId == id && a.BuyerId == myId);

            if(modelToRemove == null)
            {
                throw new InvalidOperationException("now created");
            }

            dbContext.AdsBuyers.Remove(modelToRemove);
            await dbContext.SaveChangesAsync();
        }
    }
}
