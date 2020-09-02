using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Models
{
    public class ContactsContext : DbContext
    {
        public ContactsContext(DbContextOptions<ContactsContext> options)
            : base(options)
        {
        }

        public DbSet<ContactsItem> ContactsItems { get; set; }
    }
}