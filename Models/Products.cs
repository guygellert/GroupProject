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
        [Display(Name = "שם מוצר")]
        public String productName { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "product must have a price bigger than 0")]
        [Display(Name = "מחיר")]
        public double price { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "תיאור")]
        public String description { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "כתובת תמונה")]
        public String imgUrl { get; set; }

        //[Required(ErrorMessage = "Every Product Have A Category Choose One")]
        [Display(Name = "קטגוריה")]
        //public int catagoryId { get; set; }
        public List<Catagories> Catagories { get; set; }

        public List<Order> orders { get; set; }

    }
}
