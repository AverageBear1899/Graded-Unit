using GradedUnit.Models.Data;
using GradedUnit.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GradedUnit.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        // GET: /account/login
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

        // POST: /account/login
        [HttpPost]
        public ActionResult Login(LoginUserVM model)
        {
            //check model state
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            //check if the user is 
            bool isValid = false;

            using (Db db = new Db())
            {
                if (db.Users.Any(x=> x.Username.Equals(model.Username) && x.Password.Equals(model.Password)))
                {
                    isValid = true;
                }
            }
            if (! isValid)
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

        // GET: /account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        // POST: /account/create-account
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
                if(db.Users.Any(x=> x.Username.Equals(model.Username)))
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
            //create a tempdata message
            TempData["SM"] = "You are now registered and can now log in.";

            //Email user
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("cb71214d698717", "de24b8953b0e53"),
                EnableSsl = true
            };
            client.Send("admin@example.com", model.EmailAddress, "New Account", "Hello " + model.FirstName + " You have created an account with the TShirt Company, you can now place an order with us.");
            //redirect
            return Redirect("~/account/login");
        }

        // GET: /account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/account/login");
        }

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

        // GET: /account/user-profile
        [HttpGet]
        [ActionName("user-profile")]
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

        // POST: /account/user-profile
        [HttpPost]
        [ActionName("user-profile")]
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
                if (db.Users.Where(x => x.Id != model.Id).Any(x => x.Username == username))
                {
                    ModelState.AddModelError("", "Username " + model.Username + " already exists.");
                    model.Username = "";
                    return View("UserProfile", model);
                }

                // Edit DTO
                UserDTO dto = db.Users.Find(model.Id);

                dto.FirstName = model.FirstName;
                dto.LastName = model.LastName;
                dto.EmailAddress = model.EmailAddress;
                dto.Username = model.Username;
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

    }
}