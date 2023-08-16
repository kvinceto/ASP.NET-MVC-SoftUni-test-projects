namespace SoftUniBazar.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using SoftUniBazar.Extensions;
    using SoftUniBazar.Services.Interfaces;
    using SoftUniBazar.ViewModels.AdViewModels;

    public class AdController : BaseController
    {
        private readonly IAdServices adServices;

        public AdController(IAdServices adServices)
        {
            this.adServices = adServices;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                var models = await adServices.GetAllAdsAsync();

                return View(models);

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                var model = await adServices.GetModelForAdd();
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Ad");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AdAddNewViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string? myId = User.GetId();

            if (myId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await adServices.AddNewAsync(viewModel, myId);
                return RedirectToAction("All", "Ad");
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Ad");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await adServices.GetModelForEdit(id);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Ad");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string? myId = User.GetId();

            if (myId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await adServices.EditAsync(viewModel, myId);
                return RedirectToAction("All", "Ad");
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Ad");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            string myId = User.GetId();

            if (myId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await adServices.AdToCollectionAsync(id, myId);
                return RedirectToAction("Cart", "Ad");
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Ad");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            string myId = User.GetId();

            if (myId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var models = await adServices.GetAllMine(myId);
                return View(models);

            }
            catch (Exception)
            {
                return RedirectToAction("All", "Ad");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            string myId = User.GetId();

            if (myId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await adServices.RemoveFromCartAsync(myId, id);
                return RedirectToAction("All", "Ad");
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Ad");
            }
        }
    }
}
