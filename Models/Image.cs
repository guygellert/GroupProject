using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Caveret.Models
{
    public class Image
    {
        public int Id { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "כתובת תמונה")]
        public String imgUrl { get; set; }
    }
}
