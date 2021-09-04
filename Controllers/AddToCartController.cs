﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Web;
using Caveret.Data;
using Caveret.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Caveret.Controllers
{
    public class AddToCartController : Controller
    {
        //DataTable dt;
        private readonly CaveretContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        // GET: AddToCart

        public AddToCartController(CaveretContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        public ActionResult Add(Products prod,int quantity)
        {
            var binFormatter = new BinaryFormatter();
            string jsonCart;
            var mStream = new MemoryStream();
            byte[] arr;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            List<Products> li = new List<Products>();
            Encoding u8 = Encoding.UTF8;
            if (this.HttpContext.Session.Get("cart") == null)
            {




                //This gives you the byte array.

                for (int i = 0; i < quantity; i++)
                {
                    li.Add(prod);
                }
                InsertProductsSession(li);
                //jsonCart = JsonSerializer.Serialize(li);
                //HttpContext.Session.SetString("cart", jsonCart);
                //arr = li.SelectMany(x => u8.GetBytes(x.productName)).ToArray();
                //binFormatter.Serialize(mStream, li);
                //HttpContext.Session.Set("cart", li);
                //arr = mStream.ToArray();
                //HttpContext.Session.Set("cart", arr);
                ViewBag.cart = li.Count();

                HttpContext.Session.SetInt32("count", quantity);
                //Session["count"] = 1;


            }
            else
            {
                //mStream.Write(HttpContext.Session.Get("cart"), 
                //              0,
                //              HttpContext.Session.Get("cart").Length);

                //mStream.Position = 0;
               li =  GetProductsSession();
                //li = JsonSerializer.Deserialize<List<Products>>(HttpContext.Session.GetString("cart"));
                //List<Products> li = binFormatter.Deserialize(mStream) as List<Products>;
                for (int i = 0; i < quantity; i++)
                {
                    li.Add(prod);
                }
                //binFormatter.Serialize(mStream, li);
                //jsonCart = JsonSerializer.Serialize(li);
                InsertProductsSession(li);
                //arr = li.SelectMany(x => u8.GetBytes(x.productName)).ToArray();
                //HttpContext.Session.SetString("cart", jsonCart);
                //Session["cart"] = li;
                ViewBag.cart = li.Count();
                HttpContext.Session.SetInt32("count",
                    (int)(HttpContext.Session.GetInt32("count") + quantity));
                //Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                
            }
            return RedirectToAction("Index", "Products");


        }
        public void InsertProductsSession(List <Products> ls)
        {
            string jsonCart = JsonSerializer.Serialize(ls);
            HttpContext.Session.SetString("cart", jsonCart);
        }
        public List<Products> GetProductsSession()
        {
            string d = HttpContext.Session.GetString("cart");
            List<Products> ls = JsonSerializer.Deserialize<List<Products>>(d);
            return ls;
        }

        
        public ActionResult Myorder()
        {
            if (this.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account");
            }
            List<Products> li = new List<Products>();
            List<Products> MyOrder = new List<Products>();
            if (HttpContext.Session != null && HttpContext.Session.Get("cart") != null)
            {

                li = GetProductsSession();
                InsertProductsSession(li);
                var listItems = li.GroupBy(prod => prod.Id).ToList();
                
                listItems.ForEach(prod =>
                {
                    Products p = new Products();
                    p.Id = prod.Key;
                    p.productName = _context.Products.FirstOrDefault(item => item.Id == prod.Key).productName + " X " +
                                    li.Where(item => item.Id == prod.Key).Count();

                    p.price = _context.Products.FirstOrDefault(item => item.Id == prod.Key).price *
                              li.Where(item => item.Id == prod.Key).Count();

                    MyOrder.Add(p);
                });
            }


            ViewData["isDisabled"] = false;
            if (MyOrder.Count == 0)
            {
                ViewData["isDisabled"] = true;
            }
            return View(MyOrder);

        }
        


        public ActionResult orderHistory()
        {
            if(this.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account");
            }
            var userid = _userManager.Users.FirstOrDefault(user => user.Email == HttpContext.User.Identity.Name).Id.ToString();
            var orders = _context.ShopCartItem.Where(cart => cart.userId == userid);
            //var myOrders = orders.GroupBy(cart => cart.orderId);
            
            List <CartItem> orderList = orders.ToList();
            List<Order> MyOrders = new List<Order>();
            orderList.ForEach(order =>
            {
                Order o = new Order();
                o.id = order.orderId;
                if(MyOrders.Exists(myOrd => myOrd.id == order.orderId))
                {
                    o = MyOrders.FirstOrDefault(myOrd => myOrd.id == order.orderId);
                    MyOrders.Remove(o);
                    o.price = o.price + ((int)((int)order.Quantity * _context.Products.FirstOrDefault(prod => prod.Id == order.ProductsId).price));
                    MyOrders.Add(o);
                }
                else
                {
                    o.price = ((int)((int) order.Quantity * _context.Products.FirstOrDefault(prod => prod.Id == order.ProductsId).price));
                    MyOrders.Add(o);
                }
                
            });

            return View(MyOrders);
        }

        public ActionResult OrderDetails(int? id)
        {
            if (this.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account");
            }
            var products = _context.ShopCartItem.Where(cart => cart.orderId == id)
                        .Select(item => new Products
                        {
                            Id = item.ProductsId,
                            
                            price = (int)_context.Products.FirstOrDefault(prod => prod.Id == item.ProductsId).price * item.Quantity,
                            productName = _context.Products.FirstOrDefault(prod => prod.Id == item.ProductsId).productName + " X " +
                            item.Quantity,
                            
                            
                        });
            return View(products);
        }
        
        public ActionResult CreateOrder(List<Products> lip)
        {
            if (this.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account");
            }

            
            if (this.HttpContext.Session.Get("cart") == null)
            {
                return RedirectToAction("Index", "Home");
            }
                List<CartItem> order = new List<CartItem>();
            
            if (ModelState.IsValid)
            {
                //Twitter t = new Twitter();
                //t.SendText("First Test Yay");
                //products.Catagories = new List<Catagories>();
                //products.Catagories.AddRange(_context.Catagories.Where(x => Catagories.Contains(x.Id)));

                List<Products> lp = GetProductsSession();

                var e = lp.GroupBy(l => l.Id);


                //e.ToList()
                var res = lp.GroupBy(l => l.Id)
                          .Select(l => new CartItem
                          {
                              ProductsId = l.Key,
                              Quantity = l.Count(i => i.Id == l.Key),
                              userId = _userManager.FindByNameAsync(HttpContext.User.Identity.Name).Id.ToString()
                          });

                
                e.ToList().ForEach(prod =>
                {
                    CartItem c = new CartItem();

                    
                    c.ProductsId = prod.Key;
                    c.Quantity = prod.Count(cou => cou.Id == prod.Key);
                    c.userId = _userManager.Users.FirstOrDefault(user => user.Email == HttpContext.User.Identity.Name).Id.ToString();
                    c.orderId = _context.ShopCartItem.Where(cart => cart.userId == c.userId).Max(cart => cart.orderId);
                    c.orderId++;


                    order.Add(c);

                    //c.user = HttpContext.;
                });

                //HttpContext.Use
                order.ForEach(item =>
               {
                  
                  Stock s = _context.Stock.FirstOrDefault(prod => prod.productId == item.ProductsId);
                   if(item.Quantity > s.quantity)
                   {
                       item.Quantity = s.quantity;
                   }
                   s.quantity = s.quantity - item.Quantity;

                   _context.ShopCartItem.Add(item);
                   _context.Stock.Update(s);

               });
                
                _context.SaveChanges();


                InsertProductsSession(new List<Products>());
                return View("../Home/Success");

            }
            //else
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //}
            return View();
        }
        public ActionResult Remove(Products prod)
        {
            if (this.User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account");
            }

            ////var binFormatter = new BinaryFormatter();
            ////byte[] arr;
            ////var mStream = new MemoryStream();
            ////mStream.Write(HttpContext.Session.Get("cart"),
            ////  0,
            ////  HttpContext.Session.Get("cart").Length);

            ////mStream.Position = 0;
            ////List<Products> li = binFormatter.Deserialize(mStream) as List<Products>;
            ///
            List<Products> lp = GetProductsSession();


           int HowManyToRemove = lp.Where(x => x.Id == prod.Id).Count();
            lp.RemoveAll(x => x.Id == prod.Id);

            InsertProductsSession(lp);
            HttpContext.Session.SetInt32("count",
    (int)(HttpContext.Session.GetInt32("count") - HowManyToRemove));
            return RedirectToAction("Myorder", "AddToCart");
            //return View();
        }
    }
}