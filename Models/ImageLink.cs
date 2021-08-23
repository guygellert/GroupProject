using Caveret.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Caveret.Models
{
    public class ImageLink
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }
    }
}
