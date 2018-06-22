using GradedUnit.Models.Data;
using GradedUnit.Models.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Controllers
{
    /// <summary>
    /// Controller that deals with adding/removing items to the cart
    /// </summary>
    public class CartController : Controller
    {
        /// <summary>
        /// Shows when an item is or isnt in the cart
        /// </summary>
        /// <returns>returns message that the cart is empty or it returns the cart with the items in it</returns>
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
        /// <summary>
        /// Partial view for the cart
        /// </summary>
        /// <returns>Returns a partial view with the items in the cart and their prices</returns>
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
                model.QuantityOrdered = qty;
                model.Price = price;
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
        /// <summary>
        /// Partial view for the add to cart action
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns an error message if the product is out of stock or returns the cart view</returns>
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

                if (product.Quantity == 0)
                {
                    ModelState.AddModelError("", "Out of stock");
                    
                }
                else
                {
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

                    //send email once quantity reaches restock level
                    if (product.Quantity == 5)
                    {
                        var senderClient = new SmtpClient("smtp.gmail.com", 587)
                        {
                            Credentials = new NetworkCredential("markriact@gmail.com", "eprbdxwogucwqdic"),
                            EnableSsl = true
                        };
                        senderClient.Send("markiact@gmail.com", "markjriley1899@gmail.com", "Stock Alert", "Hello admin " + product.Name + " is almost out of stock, please log in to your account and submit a re-order");
                    }
                }
                //get total, qty, price
                int qty = 0;
                decimal price = 0m;

                foreach (var item in cart)
                {
                    qty += item.QuantityOrdered;
                    price += item.QuantityOrdered * item.Price;
                }
                model.QuantityOrdered = qty;
                model.Price = price;


                //save cart to session
                Session["cart"] = cart;

                //return partial view with model
                return PartialView(model);
            }
        }
        /// <summary>
        /// Method for the increment product button
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Returns a behavior request or shows an error if the item is out of stock</returns>
        // GET: /Cart/IncrementProduct
        public ActionResult IncrementProduct(int productId)
        {
            // Init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (Db db = new Db())
            {
                //instance of product class
                ProductDTO product = db.Products.Find(productId);

                // Get cartVM from list
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);
                if (product.Quantity == 0)
                {
                    ModelState.AddModelError("", "Out of stock");
                    return Json(JsonRequestBehavior.AllowGet);
                }
                else
                {


                    // Increment qty
                    model.QuantityOrdered++;

                    // Store needed data
                    var result = new { qty = model.QuantityOrdered, price = model.Price };

                    //decrease quantity available
                    product.Quantity = product.Quantity - 1;
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                    //send email once quantity reaches restock level
                    if (product.Quantity == 5)
                    {
                        var senderClient = new SmtpClient("smtp.gmail.com", 587)
                        {
                            Credentials = new NetworkCredential("markriact@gmail.com", "eprbdxwogucwqdic"),
                            EnableSsl = true
                        };
                        senderClient.Send("markiact@gmail.com", "markjriley1899@gmail.com", "Stock Alert", "Hello admin " + product.Name + " is almost out of stock, please log in to your account and submit a re-order");
                    }
                    // Return json with data
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

        }
        /// <summary>
        /// Method for the decrement product button
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Returns a json behavior request</returns>
        // GET: /Cart/DecrementProduct
        public ActionResult DecrementProduct(int productId)
        {
            //init cart
            List<CartVM> cart = Session["cart"] as List<CartVM>;
            using (Db db = new Db())
            {
                //instance of product class
                ProductDTO product = db.Products.Find(productId);

                //get model from list
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                //decrement quantity
                if (model.QuantityOrdered > 1)
                {
                    model.QuantityOrdered--;
                }
                else
                {
                    model.QuantityOrdered = 0;
                    cart.Remove(model);
                }
                //store data
                var result = new { qty = model.QuantityOrdered, price = model.Price };

                //increase quantity available
                product.Quantity = product.Quantity + 1;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                //return json
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }
        /// <summary>
        /// Method for the remove product button
        /// </summary>
        /// <param name="productId"></param>
        // GET: /Cart/RemoveProduct
        public void RemoveProduct(int productId)
        {
            //init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;
            using (Db db = new Db()) {
                //instance of product class
                ProductDTO product = db.Products.Find(productId);

                //get model from list
                CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

                //update quantity available
                product.Quantity = product.Quantity + model.QuantityOrdered;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                //remove model from list
                cart.Remove(model);

                

            }
        }
        /// <summary>
        /// Partial view for the paypal functionality 
        /// </summary>
        /// <returns>Returns a partial view for papal</returns>
        public ActionResult PaypalPartial()
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            return PartialView(cart);
        }
        /// <summary>
        /// Allows the user to place their order
        /// </summary>
        // POST: /Cart/PlaceOrder
        public void PlaceOrder()
        {
            // Get cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            // Get username
            string username = User.Identity.Name;

            int orderId = 0;

            using (Db db = new Db())
            {
                // Init OrderDTO
                OrderDTO orderDTO = new OrderDTO();

                // Get user id
                var q = db.Users.FirstOrDefault(x => x.Username == username);
                int userId = q.Id;

                //Add to OrderDTO and save
                orderDTO.UserId = userId;
                orderDTO.CreatedAt = DateTime.Now;

                db.Orders.Add(orderDTO);

                db.SaveChanges();

                // Get inserted id
                orderId = orderDTO.OrderId;

                // Init OrderDetailsDTO
                OrderDetailsDTO orderDetailsDTO = new OrderDetailsDTO();

                // Add to OrderDetailsDTO
                foreach (var item in cart)
                {
                    orderDetailsDTO.OrderId = orderId;
                    //orderDetailsDTO.UserId = userId;
                    orderDetailsDTO.ProductId = item.ProductId;
                    orderDetailsDTO.Quantity = item.QuantityOrdered;

                    db.OrderDetails.Add(orderDetailsDTO);

                    db.SaveChanges();
                }
            }

            

            // Reset session
            Session["cart"] = null;
        }

    }


}