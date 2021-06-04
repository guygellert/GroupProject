using Caveret.Models;
using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Products
{


    public int Id { get; set; }
    public String productName { get; set; }
    [DataType(DataType.Currency)]
    public double price { get; set; }
    public String description { get; set; }
    public String imgUrl { get; set; }

    public int catagoryId { get; set; }
    public Catagories catagory { get; set; }


}
