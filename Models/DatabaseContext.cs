using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ContactsApi.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<SkillModel> Skills { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
    }
}