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

//        public List<ContactModel> getAllContacts() => Contacts.ToList();
        //public List<SkillNameModel> getSkillNames() => SkillNames.ToList();
        //public List<SkillLevelModel> getSkillLevels() => SkillLevels.ToList();
        //public void PostContact(ContactModel contact)
        //{
        //    Contacts.Add(contact);
        //    this.SaveChanges();
        //    return;
        //}

        //public bool PostSkill(SkillNameModel skill)
        //{
        //    if (SkillNames.Any(s => s.Name == skill.Name)){ return false; }       
        //    SkillNames.Add(skill);
        //    this.SaveChanges();
        //    return true;
        //}

        //public bool PostLevel(SkillLevelModel level)
        //{
        //    if (SkillLevels.Any(s => s.Level == level.Level)) { return false; }
        //    SkillLevels.Add(level);
        //    this.SaveChanges();
        //    return true;
        //}

    }
}