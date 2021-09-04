using Caveret.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Caveret.Models
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class Products
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [MaxLength(30)]
        [Required(ErrorMessage = "Think About The Customers! Product Must Have A Name")]
        [Display(Name = "Product Name")]
        public String productName { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "product must have a price bigger than 0")]
        [Display(Name = "Price")]
        [Range(1,999999)]
        public double price { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [MaxLength(50)]
        public String description { get; set; }

        [Display(Name ="Picture")]
        public ImageLink imgUrl { get; set; }

        //[Required(ErrorMessage = "Every Product Have A Category Choose One")]
        [Display(Name = "Categories")]
        //public int catagoryId { get; set; }
        public List<Catagories> Catagories { get; set; }

        public List<Order> orders { get; set; }

    }
}
