using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacturaOnline.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Display(Name = "CUI")]
        public string cui { get; set; }

        [Display(Name = "Adresa")]
        public string Address { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Nume Firma")]
        public string FirmName { get; set; }

        [Display(Name = "Nume Reprezentant")]
        public string RepName { get; set; }

        public string IdPartner { get; set; }

    }
}
