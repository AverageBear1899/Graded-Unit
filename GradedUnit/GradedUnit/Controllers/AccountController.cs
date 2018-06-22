using GradedUnit.Models.Data;
using GradedUnit.Models.ViewModels.Account;
using GradedUnit.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GradedUnit.Controllers
{
    /// <summary>
    /// Controller that deals with the creation and editing of accounts
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Index
        /// </summary>
        /// <returns>Returns index view</returns>
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        
        /// <summary>
        /// Returns view that allows user to enter login details
        /// </summary>
        /// <returns>Returns the login view</returns>
        [HttpGet]
        public ActionResult Login()
        {
            //confirm user is not logged in
            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
                return RedirectToAction("user-profile");

            //return view
            return View();
        }

        
        /// <summary>
        /// Processes the information given by the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns the login view or displays an error message that the login was invalid</returns>
        [HttpPost]
        public ActionResult Login(LoginUserVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //check if the user is 
            bool isValid = false;

            using (Db db = new Db())
            {
                if (db.Users.Any(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password)))
                {
                    isValid = true;
                }
            }
            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid username or password!");
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, model.RememberMe));
            }

        }

        
        /// <summary>
        /// Allows user to enter desired account details
        /// </summary>
        /// <returns>Returns the create account view</returns>
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        
        /// <summary>
        /// Processes the information given by the user to create the account
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Redirects user to the login page or displays error message that the account creation failed</returns>
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", model);
            }
            //check passwords match
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Passwords don't match!");
                return View("CreateAccount", model);
            }
            using (Db db = new Db())
            {
                //check username is unique
                if (db.Users.Any(x => x.Username.Equals(model.Username)))
                {
                    ModelState.AddModelError("", "Username " + model.Username + " is taken!");
                    model.Username = "";
                    return View("CreateAccount", model);
                }
                //create dto
                UserDTO userDTO = new UserDTO()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    Username = model.Username,
                    Password = model.Password,
                    Address = model.Address
                };
                //add the dto
                db.Users.Add(userDTO);
                //save
                db.SaveChanges();

                //add to userrolesdto
                int id = userDTO.Id;
                UserRoleDTO userRoleDTO = new UserRoleDTO()
                {
                    UserId = id,
                    RoleId = 2
                };
                db.UserRoles.Add(userRoleDTO);
                db.SaveChanges();

            }
            //create a  message
            TempData["SM"] = "You are now registered and can now log in.";

            //Email user
            var senderClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("markriact@gmail.com", "eprbdxwogucwqdic"),
                EnableSsl = true
            };
            senderClient.Send("markiact@gmail.com", model.EmailAddress, "You have created a new Account", "Hello " + model.FirstName + " " + model.LastName + " You have created an account with the TShirt Company, you can now place an order with us. Happy shopping!");
            //redirect
            return Redirect("~/account/login");
        }

        /// <summary>
        /// Provides user the ability to logout of their account
        /// </summary>
        /// <returns>Redirects the user to the login page</returns>
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/account/login");
        }

        /// <summary>
        /// Takes the users details to display a link to their account details using their first and last name
        /// </summary>
        /// <returns>Returns a partial view for the user navigation bar</returns>
        [Authorize]
        public ActionResult UserNavPartial()
        {
            //get username
            string username = User.Identity.Name;
            //declare model
            UserNavPartialVM model;

            using (Db db = new Db())
            {
                //get user
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);
                //build the model
                model = new UserNavPartialVM()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };
            }
            //return partial view with model
            return PartialView(model);
        }

        /// <summary>
        /// Takes the users details from the database to build a view with their details filled in
        /// </summary>
        /// <returns>Takes the user to their own profile</returns>
        [HttpGet]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile()
        {
            // Get username
            string username = User.Identity.Name;

            // Declare model
            UserProfileVM model;

            using (Db db = new Db())
            {
                // Get user
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);

                // Build model
                model = new UserProfileVM(dto);
            }

            // Return view with model
            return View("UserProfile", model);
        }

        /// <summary>
        /// Allows the user to update the fields of their account and saves the changes to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns the user to their profile or displays an error stating their desired changes haven't been made</returns>
        [HttpPost]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile(UserProfileVM model)
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            // Check if passwords match if need be
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                    return View("UserProfile", model);
                }
            }

            using (Db db = new Db())
            {
                // Get username
                string username = User.Identity.Name;

                // Make sure username is unique
                //if (db.Users.Where(x => x.Id != model.Id).Any(x => x.Username == username))
                //{
                //    ModelState.AddModelError("", "Username " + model.Username + " already exists.");
                //    model.Username = "";
                //    return View("UserProfile", model);
                //}

                // Edit DTO
                UserDTO dto = db.Users.Find(model.Id);

                dto.FirstName = model.FirstName;
                dto.LastName = model.LastName;
                dto.EmailAddress = model.EmailAddress;
                //dto.Username = model.Username;
                dto.Address = model.Address;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    dto.Password = model.Password;
                }

                // Save
                db.SaveChanges();
            }

            // Set TempData message
            TempData["SM"] = "You have edited your profile!";

            // Redirect
            return Redirect("~/account/user-profile");
        }

        /// <summary>
        /// Alllows the user to view their orders
        /// </summary>
        /// <returns>Takes the user to their orders page</returns>
        [Authorize(Roles = "User")]
        public ActionResult Orders()
        {
            //init list of ordersforuservm
            List<OrdersForUserVM> ordersForUser = new List<OrdersForUserVM>();

            using (Db db = new Db())
            {
                //get userid
                UserDTO user = db.Users.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
                int userId = user.Id;
                //init list of orderVM
                List<OrderVM> orders = db.Orders.Where(x => x.UserId == userId).ToArray().Select(x => new OrderVM(x)).ToList();

                //loop through list of orderVM
                foreach(var order in orders)
                {
                    //init products dict
                    Dictionary<string, int> productsAndQty = new Dictionary<string, int>();
                    //declare total
                    decimal total = 0m;
                    //init list of orderdetailsdto
                    List<OrderDetailsDTO> orderDetailsDTO = db.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                    //loop through
                    foreach (var orderDetails in orderDetailsDTO)
                    {
                        //get product
                        ProductDTO product = db.Products.Where(x => x.Id == orderDetails.ProductId).FirstOrDefault();
                        //get product price
                        decimal price = product.Price;
                        //get product name
                        string productName = product.Name;
                        //add to products dict
                        productsAndQty.Add(productName, orderDetails.Quantity);
                        //get total
                        total += orderDetails.Quantity * price;
                    }
                    //add to OrdersForUserVM list
                    ordersForUser.Add(new OrdersForUserVM()
                    {
                        OrderNumber = order.OrderId,
                        Total = total,
                        ProductsAndQty = productsAndQty,
                        CreatedAt = order.CreatedAt
                    });
                }
            }
                return View(ordersForUser);
        }
        /// <summary>
        /// Allows the user to cancel their order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the user to their orders page or displays an error that they cannot cancel their order</returns>
        [Authorize(Roles="User")]
        public ActionResult DeleteOrder(int id)
        {
            //delete product from db
            using (Db db = new Db())
            {
                OrderDTO dto = db.Orders.Find(id);
                //list of orderdetails
                List<OrderDetailsDTO> orderDetails = db.OrderDetails.Where(x => x.OrderId == id).ToList();
                //adds the quantity back to the store
                foreach (var line in orderDetails)
                {
                    line.Products.Quantity = (line.Products.Quantity + line.Quantity);
                    db.Entry(line.Products).State = EntityState.Modified;
                }

                if (dto.CreatedAt > DateTime.Now.AddHours(-24))
                    {
                    db.Orders.Remove(dto);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "You can only cancel your orders on the day they are placed");
                }
                //redirect
                return RedirectToAction("Orders");
            }
        }

        /// <summary>
        /// Allows user to enter desired account details
        /// </summary>
        /// <returns>Returns the create account view</returns>
        //[ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateStaffAccount()
        {
            return View("CreateStaffAccount");
        }

        /// <summary>
        /// Processes the information given by the user to create the staff account
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Redirects user to the login page or displays error message that the account creation failed</returns>
        //[ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateStaffAccount(UserVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View("CreateStaffAccount", model);
            }
            //check passwords match
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Passwords don't match!");
                return View("CreateStaffAccount", model);
            }
            using (Db db = new Db())
            {
                //check username is unique
                if (db.Users.Any(x => x.Username.Equals(model.Username)))
                {
                    ModelState.AddModelError("", "Username " + model.Username + " is taken!");
                    model.Username = "";
                    return View("CreateStaffAccount", model);
                }
                //create dto
                UserDTO userDTO = new UserDTO()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    Username = model.Username,
                    Password = model.Password,
                    Address = model.Address
                };
                //add the dto
                db.Users.Add(userDTO);
                //save
                db.SaveChanges();

                //add to userrolesdto
                int id = userDTO.Id;
                UserRoleDTO userRoleDTO = new UserRoleDTO()
                {
                    UserId = id,
                    RoleId = 3
                };
                db.UserRoles.Add(userRoleDTO);
                db.SaveChanges();

            }
            //create a  message
            TempData["SM"] = "The staff member is now registered and can now log in.";

            //Email user
            var senderClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("markriact@gmail.com", "eprbdxwogucwqdic"),
                EnableSsl = true
            };
            senderClient.Send("markiact@gmail.com", model.EmailAddress, "You have had a new Account created for you", "Hello " + model.FirstName + " " + model.LastName + " Welcome to the TShirt Company!");
            //redirect
            return Redirect("~/admin/dashboard");
        }


    }

    }
