using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


public class Skill
{
    public long Id { get; set; }
    [ForeignKey("SkillNameModel")]
    public String Name { get; set; }
    [ForeignKey("SkillLevelModel")]
    public String Level { get; set; }
}
public class ContactModel
{
    public long Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }
    public virtual ICollection<Skill> Skills { get; set; }   
}