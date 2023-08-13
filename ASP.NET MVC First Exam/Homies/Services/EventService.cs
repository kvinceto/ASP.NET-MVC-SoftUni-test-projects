using Homies.Data;
using Homies.Data.Models;
using Homies.Models.EventViewModels;
using Homies.Models.TypeViewModels;
using Homies.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

using static Homies.Common.EntityValidationConstants;

using Event = Homies.Data.Models.Event;

namespace Homies.Services
{
    public class EventService : IEventService
    {
        private readonly HomiesDbContext dbContext;

        public EventService(HomiesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddNewEventAsync(EventAddViewModel viewModel, string userId)
        {
            Event newEvent = new Event()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                CreatedOn = DateTime.UtcNow,
                Start = DateTime.Parse(viewModel.Start),
                End = DateTime.Parse(viewModel.End),
                TypeId = viewModel.TypeId,
                OrganiserId = userId
            };

            await dbContext.Events.AddAsync(newEvent);
            await dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(EventEditViewModel viewModel, string myId)
        {
            Event? eventToEdit = await dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == viewModel.Id && e.OrganiserId == myId);

            if (eventToEdit == null)
            {
                throw new NullReferenceException(nameof(eventToEdit));
            }

            eventToEdit.Name = viewModel.Name;
            eventToEdit.Description = viewModel.Description;
            eventToEdit.Start = DateTime.Parse(viewModel.Start);
            eventToEdit.End = DateTime.Parse(viewModel.End);
            eventToEdit.TypeId = viewModel.TypeId;

            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<EventAllViewModel>> GetAllEventsAsync()
        {
            return await dbContext.Events
                .Include(e => e.Type)
                .Include(e => e.Organiser)
                .Select(e => new EventAllViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Start = e.Start.ToString(DateTimeFormat),
                    Type = e.Type.Name,
                    Organiser = e.Organiser.UserName
                })
                .ToArrayAsync();
        }

        public async Task<ICollection<EventAllViewModel>> GetAllJoinedEventsAsync(string userId)
        {
            return await dbContext.EventsParticipants
                .Include(e => e.Event)
                .Include(e => e.Helper)
                .Where(e => e.HelperId == userId)
                .Select(e => new EventAllViewModel()
                {
                    Id = e.Event.Id,
                    Name = e.Event.Name,
                    Organiser = e.Event.Organiser.UserName,
                    Start = e.Event.Start.ToString(DateTimeFormat),
                    Type = e.Event.Type.Name
                })
                .ToArrayAsync();
        }

        public async Task<EventDetailsViewModel> GetEventAsync(int id)
        {
            Event? eventToView = await dbContext.Events
                .Include(e => e.Organiser)
                .Include(e => e.Type)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventToView == null)
            {
                throw new NullReferenceException(nameof(eventToView));
            }

            return new EventDetailsViewModel()
            {
                Id = eventToView.Id,
                Name = eventToView.Name,
                Description = eventToView.Description,
                CreatedOn = eventToView.CreatedOn.ToString(DateTimeFormat),
                Start = eventToView.CreatedOn.ToString(DateTimeFormat),
                End = eventToView.CreatedOn.ToString(DateTimeFormat),
                Organiser = eventToView.Organiser.UserName,
                Type = eventToView.Type.Name
            };
        }

        public async Task<EventEditViewModel> GetEventForEditAsync(int id, string myId)
        {
           Event? eventToEdit = await dbContext.Events
                .Include(e => e.Organiser)
                .FirstOrDefaultAsync(e => e.Id == id && e.OrganiserId == myId);

            if (eventToEdit == null)
            {
                throw new NullReferenceException(nameof(eventToEdit));
            }

            return new EventEditViewModel()
            {
                Id = eventToEdit.Id,
                Name = eventToEdit.Name,
                Description = eventToEdit.Description,
                Start = eventToEdit.Start.ToString(DateTimeFormat),
                End = eventToEdit.End.ToString(DateTimeFormat),
                TypeId = eventToEdit.TypeId,
                Types = await dbContext.Types
                                       .Select(t => new TypeViewModel()
                                       {
                                           Id = t.Id,
                                           Name = t.Name
                                       })
                                       .ToArrayAsync()
            };
        }

        public async Task<EventAddViewModel> GetNewEventAsync()
        {
            var result = new EventAddViewModel();
            result.Types = await dbContext.Types
                .Select(t => new TypeViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToArrayAsync();

            return result;
        }

        public async Task JoinEventAsync(int id, string userId)
        {
            bool isValid = await dbContext.EventsParticipants
                .AnyAsync(e => e.EventId == id && e.HelperId == userId);

            if (isValid)
            {
                throw new ArgumentException(nameof(id));
            }

            EventParticipant ep = new EventParticipant()
            {
                EventId = id,
                HelperId = userId
            };

            await dbContext.EventsParticipants.AddAsync(ep);
            await dbContext.SaveChangesAsync();

        }

        public async Task LeaveEventAsync(int id, string myId)
        {
            bool isValid = await dbContext.EventsParticipants
                .AnyAsync(e => e.EventId == id && e.HelperId == myId);

            if (!isValid)
            {
                throw new ArgumentException(nameof(id));
            }

            var ep = await dbContext.EventsParticipants
                .FirstAsync(e => e.EventId == id && e.HelperId == myId);

            dbContext.EventsParticipants.Remove(ep);
            await dbContext.SaveChangesAsync();
        }
    }
}
