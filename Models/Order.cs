using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Caveret.Models
{
    public class Order
    {


        public int id { get; set; }


        public int userId { get; set; }
        public IdentityUser user { get; set; }

        [DataType(DataType.Date)]
        public DateTime whenToDeliever { get; set; }

        public int quentity { get; set; }

        public int productsId { get; set; }
        //public AspNetUser MyProperty { get; set; }
        public List<Products> products { get; set; }

        public int price { get; set; }
    }
}
