namespace Contacts.Data.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApplicationUserContact
    {
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser ApplicationUser { get; set; } = null!;

        [ForeignKey("Contact")]
        public int ContactId { get; set; }

        public Contact Contact { get; set; } = null!;
    }
}
