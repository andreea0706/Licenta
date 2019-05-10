using FacturaOnline.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/* Model that helps with providing info to the Customer Views */
namespace FacturaOnline.ViewModels
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public int cui { get; set; }
        public string data { get; set; }
    }
}
