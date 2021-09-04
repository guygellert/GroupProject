using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Caveret.Models
{
    public class Stock
    {
        public int id { get; set; }

        public int productId { get; set; }

        [Display(Name = "Product")]
        public Products product { get; set; }

        [Display(Name = "Quantity")]
        public int quantity { get; set; }
    }
}
