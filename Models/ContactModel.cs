using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


//public class Skill
//{
//    public long Id { get; set; }
//    //Foreign Key for SkillNameModel
//    public String Name { get; set; }
//    //    public SkillNameModel SkillNameModel {get; set; }

//    public String Level { get; set; }
//    // public SkillLevelModel skillLevelModel { get; set; }
//}
public class ContactModel
{
    [Key]
    public long ContactModelId { get; set; }
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

    public virtual ICollection<SkillModel> SkillModel { get; set; }

}