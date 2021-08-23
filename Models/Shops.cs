using Caveret.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class Shops
{
    public int Id { get; set; }

    [DataType(DataType.Text)]
    [Required]
    [Display(Name = "Address")]
    public string Address { get; set; }

    [MinLength(2)]
    [MaxLength(200)]
    [Display(Name = "Description")]
    public string Description { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
    [DataType(DataType.Time)]
    [Display(Name = "Opening time")]
    public DateTime OpeningTime { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "{0:hh\\:mm tt}")]
    [DataType(DataType.Time)]
    [Display(Name = "Closing time")]
    public DateTime ClosingTime { get; set; }
}
