using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caveret.Models
{
    public class Stock
    {
        public int id { get; set; }

        public int productId { get; set; }
        public Products product { get; set; }

        public int quantity { get; set; }
    }
}
