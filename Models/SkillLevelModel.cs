using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

public class SkillLevelModel
{
    public long Id { get; set; }
    [Required]
    public String Level { get; set; }
}
