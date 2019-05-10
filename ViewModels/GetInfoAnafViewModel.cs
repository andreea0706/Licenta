using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//This view model is used to retrieve data from ANAF
namespace FacturaOnline.ViewModels
{
    public class GetInfoAnafViewModel
    {
        [Display(Name = "CUI")]
        public int cui { get; set; }

        [Display(Name = "Data")]
        public string data { get; set; }
    }
}
