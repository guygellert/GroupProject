using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Caveret.Models
{
    public class CartItem
    {


        public int cartItemId { get; set; }
        //[Key]
        //public string ItemId { get; set; }

        //public string CartId { get; set; }

        public int orderId { get; set; }

        public string userId { get; set; }
        public int Quantity { get; set; }

        //public System.DateTime DateCreated { get; set; }

        public int ProductsId { get; set; }

        public Products Product { get; set; }
    }
}
