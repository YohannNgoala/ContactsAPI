using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


    public enum SkillLevel
{
    Noob,
    Begginner,
    Itermediate,
    Advanced,
    Expert
}

public enum SkillName
{
    Cs,
    Cpp,
    C,
    Js,
}
public class SkillsItem
{
    public long Id { get; set; }
    [Required]
    public SkillName Name { get; set; }
    [Required]
    public SkillLevel Level { get; set; }
}
