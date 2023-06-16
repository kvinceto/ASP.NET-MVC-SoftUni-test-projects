namespace Contacts.Data
{
    using Contacts.Data.Entities;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ContactsDbContext : IdentityDbContext<ApplicationUser>
    {

        public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
            : base(options)
        {
            //this.Database.Migrate();
        }

        public DbSet<Contact> Contacts { get; set; } = null!;

        public DbSet<ApplicationUserContact> ApplicationUsersContacts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUserContact>(e =>
            {
                e.HasKey(uc => new { uc.ApplicationUserId, uc.ContactId });
            });

            base.OnModelCreating(builder);
        }
    }
}