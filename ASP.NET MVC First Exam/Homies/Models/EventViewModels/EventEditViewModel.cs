using Homies.Models.TypeViewModels;

using System.ComponentModel.DataAnnotations;

using static Homies.Common.EntityValidationConstants.Event;

namespace Homies.Models.EventViewModels
{
    public class EventEditViewModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string Start { get; set; } = null!;

        [Required]
        public string End { get; set; } = null!;

        [Required]
        public int TypeId { get; set; }

        public ICollection<TypeViewModel> Types { get; set; } = new HashSet<TypeViewModel>();
    }
}



