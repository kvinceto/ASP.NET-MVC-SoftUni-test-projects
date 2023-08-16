namespace SoftUniBazar.ViewModels.AdViewModels
{
    using System.ComponentModel.DataAnnotations;

    using SoftUniBazar.ViewModels.CategoriseViewModels;

    using static SoftUniBazar.Common.EntityValidationConstants.Ad;


    public class AdAddNewViewModel
    {

        public AdAddNewViewModel()
        {
            this.Categories = new HashSet<CategoryViewModel>();
        }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; } = null!;
    }
}
