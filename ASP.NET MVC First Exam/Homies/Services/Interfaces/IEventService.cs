using Homies.Models.EventViewModels;

namespace Homies.Services.Interfaces
{
    public interface IEventService
    {
        Task<ICollection<EventAllViewModel>> GetAllEventsAsync();

        Task<ICollection<EventAllViewModel>> GetAllJoinedEventsAsync(string userId);

        Task<EventAddViewModel> GetNewEventAsync();

        Task AddNewEventAsync(EventAddViewModel viewModel, string userId);

        Task JoinEventAsync(int id, string userId);

        Task LeaveEventAsync(int id, string myId);

        Task<EventEditViewModel> GetEventForEditAsync(int id, string myId);

        Task EditAsync(EventEditViewModel viewModel, string myId);

        Task<EventDetailsViewModel> GetEventAsync(int id);
    }
}
