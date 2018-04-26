using GradedUnit.Models.Data;
using GradedUnit.Models.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //init cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            //check if cart is empty
            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty";
                return View();
            }
            //calculate total and save to view
            decimal total = 0m;

            foreach(var item in cart)
            {
                total += item.Total;
            }

            ViewBag.GrandTotal = total;
            //return view with list
            return View(cart);
        }

        public ActionResult CartPartial()
        {
            //init cartVM
            CartVM model = new CartVM();
            //init quantity
            int qty = 0;

            //init price
            decimal price = 0m;
            //check for cart session
            if (Session["cart"] != null)
            {
                //get total qty ordered and price
                var list = (List<CartVM>)Session["cart"];
                foreach (var item in list)
                {
                    qty += item.QuantityOrdered;
                    price += item.QuantityOrdered * item.Price;
                }
            }
            else
            {
                //or set qty and price to 0
                model.QuantityOrdered = 0;
                model.Price = 0m;
            }





            //return view with model
            return PartialView(model);
        }

        public ActionResult AddToCartPartial(int id)
        {
            //init cartVM list
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            //init cartVM
            CartVM model = new CartVM();
            
            
            using (Db db = new Db())
            {
                //get product
                ProductDTO product = db.Products.Find(id);
                //check if product is in the cart
                var productInCart = cart.FirstOrDefault(x => x.ProductId == id);
                //if not, add new
                if (productInCart == null)
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        QuantityOrdered = 1,
                        Price = product.Price,
                        Image = product.ImageName
                        
                    });
                    
                }
                else
                {
                    //if it is, increment
                    productInCart.QuantityOrdered++;
                }

                product.Quantity = product.Quantity - 1;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

            }
            //get total, qty, price
            int qty = 0;
            decimal price = 0m;

            foreach(var item in cart)
            {
                qty += item.QuantityOrdered;
                price += item.QuantityOrdered * item.Price;
            }
            model.QuantityOrdered = qty;
            model.Price = price;
            //change quantity available

            //save cart to session
            Session["cart"] = cart;

            //return partial view with model
            return PartialView(model);
        }
    }
}