using Homies.Extentions;
using Homies.Models.EventViewModels;
using Homies.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Homies.Controllers
{
    public class EventController : BaseController
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var models = await eventService.GetAllEventsAsync();
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = await eventService.GetNewEventAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventAddViewModel viewModel)
        {
            var myId = User.GetId();

            if (myId == null)
            {
                return View(viewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await eventService.AddNewEventAsync(viewModel, myId);

            return RedirectToAction("All", "Event");
        }

        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            var myId = User.GetId();

            if (myId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await eventService.JoinEventAsync(id, myId);
                return RedirectToAction("Joined", "Event");
            }
            catch (Exception)
            {
                return RedirectToAction("Joined", "Event");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Joined()
        {
            var myId = User.GetId();

            if (myId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var models = await eventService.GetAllJoinedEventsAsync(myId);

            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            var myId = User.GetId();

            if (myId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await eventService.LeaveEventAsync(id, myId);
                return RedirectToAction("All", "Event");
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Event");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var myId = User.GetId();

            if (myId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var viewModel = await eventService.GetEventForEditAsync(id, myId);

                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Event");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventEditViewModel viewModel)
        {
            var myId = User.GetId();

            if (myId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                await eventService.EditAsync(viewModel, myId);

                return RedirectToAction("All", "Event");

            }
            catch (Exception)
            {
                return RedirectToAction("All", "Event");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var viewModel = await eventService.GetEventAsync(id);
                return View(viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Event");
            }
        }
    }
}
