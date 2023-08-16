namespace SoftUniBazar.Services.Interfaces
{
    using SoftUniBazar.ViewModels.AdViewModels;

    public interface IAdServices
    {
        Task AddNewAsync(AdAddNewViewModel viewModel, string myId);

        Task AdToCollectionAsync(int id, string myId);

        Task EditAsync(AdEditViewModel viewModel, string myId);

        Task<ICollection<AdAllViewModel>> GetAllAdsAsync();

        Task<ICollection<AdAllViewModel>> GetAllMine(string myId);

        Task<AdAddNewViewModel> GetModelForAdd();

        Task<AdEditViewModel> GetModelForEdit(int id);

        Task RemoveFromCartAsync(string myId, int id);
    }
}
