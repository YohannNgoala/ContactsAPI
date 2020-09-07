using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

public class SkillModel
{
    public long Id { get; set; }
    [Required]
    public String Name { get; set; }
    [Required]
    public String Level { get; set; }
    //ForeignKey
    [Required]
    public long? ContactModelId { get; set; }
    public virtual ContactModel ContactModel { get; set; }
}

