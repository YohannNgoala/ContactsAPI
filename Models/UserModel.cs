using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


public class UserModel
{
    [Key]
    public String UserName { get; set; }
    public string Password { get; set; }

    public virtual ICollection<ContactModel> ContactModels { get; set; }
}

