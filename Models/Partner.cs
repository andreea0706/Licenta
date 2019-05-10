using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FacturaOnline.Models
{

    public class Partner : IdentityUser
    {
       
        [Display(Name = "CUI")]
        public string cui { get; set; }
       
        [Display(Name = "Adresa")]
        public string Address { get; set; }

        [Display(Name = "Nume Firma")]
        public string FirmName { get; set; }

    }
}
