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

        [DataType(DataType.Date)]
        public DateTime whenToDeliever { get; set; }


        //public AspNetUser MyProperty { get; set; }
        public List<Products> products { get; set; }
    }
}
