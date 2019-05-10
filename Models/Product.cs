using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FacturaOnline.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Denumire")]
        public string Name { get; set; }

        [Display(Name = "Descriere")]
        public string Description { get; set; }

        [Display(Name = "Moneda")]
        public string Currency { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Pret")]
        public int Price { get; set; }

        [Display(Name = " Pretul Contine TVA?")]
        public bool HasVAT { get; set; }

        [Display(Name = "Unitate de Masura")]
        public string MeasureUnit { get; set; }

        [Display(Name = "Categorie")]
        public string Categ { get; set; }

        public string IdPartner { get; set; }

    }
}
