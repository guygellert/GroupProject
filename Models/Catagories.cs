using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Caveret.Models
{
    public class Catagories
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Think about the customers! you must give a name")]
        public String catagorieName { get; set; }

        public List<Products> products { get; set; }
    }
}
