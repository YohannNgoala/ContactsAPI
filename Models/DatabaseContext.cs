using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ContactsApi.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<SkillNameModel> SkillNames { get; set; }
        public DbSet<SkillLevelModel> SkillLevels { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
    }
}