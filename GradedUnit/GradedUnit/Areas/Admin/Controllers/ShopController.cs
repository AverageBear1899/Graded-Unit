using GradedUnit.Areas.Admin.Models.ViewModels.Shop;
using GradedUnit.Models.Data;
using GradedUnit.Models.ViewModels.Account;
using GradedUnit.Models.ViewModels.Shop;
using PagedList;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace GradedUnit.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Staff")]
    public class ShopController : Controller
    {
        /// <summary>
        /// Initialises list of categories and passes it to the view model
        /// </summary>
        /// <returns>Takes user to the categories view</returns>
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            //declare a list of models
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                //init the list
                categoryVMList = db.Catagories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }
            //return view with list
            return View(categoryVMList);
        }
        /// <summary>
        /// Allows a user to create a new category for items within the shop
        /// </summary>
        /// <param name="catName"></param>
        /// <returns>Returns the id of the category once it has been created</returns>
        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            //declare id
            string id;

            using (Db db = new Db())
            {
                //check that category is unique
                if (db.Catagories.Any(x => x.Name == catName))

                    return "titletaken";

                //init dto
                CategoryDTO dto = new CategoryDTO();
                //add to dto
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;
                //save 
                db.Catagories.Add(dto);
                db.SaveChanges();
                //get id
                id = dto.Id.ToString();
            }
            //return id
            return id;
        }
        /// <summary>
        /// Allows a user to reorder how the categories will appear to a customer
        /// </summary>
        /// <param name="id"></param>
        //POST : Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                //set initial count
                int count = 1;
                //declare page dto
                CategoryDTO dto;
                //set sorting for each category
                foreach (var catId in id)
                {
                    dto = db.Catagories.Find(catId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;

                }
            }

        }
        /// <summary>
        /// Allows a user to delete an existing category from the shop
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirects user to the categories view</returns>
        //GET : Admin/Shop/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                //get the page
                CategoryDTO dto = db.Catagories.Find(id);

                //remove the categories
                db.Catagories.Remove(dto);

                //save
                db.SaveChanges();
            }
            //redirect
            return RedirectToAction("Categories");
        }
        /// <summary>
        /// Allows a user to rename a category that exists and checks to see if the name is unique
        /// </summary>
        /// <param name="newCatName"></param>
        /// <param name="id"></param>
        /// <returns>return "titletaken" or returns "ok"</returns>
        [HttpPost]
        //GET : Admin/Shop/RenameCategory
        public string RenameCategory(string newCatName, int id)
        {
            using (Db db = new Db())
            {
                //check the cat name is unique
                if (db.Catagories.Any(x => x.Name == newCatName))
                    return "titletaken";
                //get dto
                CategoryDTO dto = db.Catagories.Find(id);
                //edit dto
                dto.Name = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();
                //save
                db.SaveChanges();

            }
            //return
            return "ok;";
        }
        /// <summary>
        /// Get method for adding a product to the shop
        /// </summary>
        /// <returns>Takes user to the add product view</returns>
        //GET : Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            //init model
            ProductVM model = new ProductVM();

            //add select list of categories
            using (Db db = new Db())
            {
                model.Categories = new SelectList(db.Catagories.ToList(), "Id", "Name");
            }

            //return view with model
            return View(model);
        }
        /// <summary>
        /// Post method for adding a product to the shop
        /// </summary>
        /// <param name="model"></param>
        /// <param name="file"></param>
        /// <returns>Returns error message if the product exists or confirmation if the product is created</returns>
        //POST : Admin/Shop/AddProduct
        [HttpPost]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            //check model state

            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.Categories = new SelectList(db.Catagories.ToList(), "Id", "Name");
                    return View(model);
                }
            }
            //make sure product is unique
            using (Db db = new Db())
            {
                if (db.Products.Any(x => x.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Catagories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That product name is taken");
                    return View(model);
                }
            }

            //declare product ID
            int id;

            //Init and save product
            using (Db db = new Db())
            {
                ProductDTO product = new ProductDTO();

                product.Name = model.Name;
                product.Slug = model.Name.Replace(" ", "-").ToLower();
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                CategoryDTO catDTO = db.Catagories.FirstOrDefault(x => x.Id == model.CategoryId);
                product.CategoryName = catDTO.Name;
                product.Quantity = model.Quantity;

                db.Products.Add(product);
                db.SaveChanges();


                //get inserted id
                id = product.Id;
            }
            //set tempdata 
            TempData["SM"] = "You have added a product!";

            #region Upload Image
            //create directories
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            //check if file was uploaded 
            if (file != null && file.ContentLength > 0)
            {
                //get file ext
                string ext = file.ContentType.ToLower();
                //verify ext
                if (ext != "image/jpg" &&
                        ext != "image/jpeg" &&
                        ext != "image/pjpeg" &&
                        ext != "image/gif" &&
                        ext != "image/x-png" &&
                        ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        {
                            model.Categories = new SelectList(db.Catagories.ToList(), "Id", "Name");
                            ModelState.AddModelError("", "The Image was not uploaded - wrong image extension.");
                            return View(model);
                        }
                    }
                }
                //init image name
                string imageName = file.FileName;
                //save image to dto
                using (Db db = new Db())
                {
                    ProductDTO dto = db.Products.Find(id);
                    dto.ImageName = imageName;

                    db.SaveChanges();

                }
                //set original and thumb img path
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);
                //save original
                file.SaveAs(path);
                //create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }
            #endregion

            //redirect
            return RedirectToAction("AddProduct");
        }
        /// <summary>
        ///  Shows all of the products that are in the database
        /// </summary>
        /// <param name="page"></param>
        /// <param name="catId"></param>
        /// <returns>Returns the products view with all the products visible</returns>
        //GET : Admin/Shop/Products
        public ActionResult Products(int? page, int? catId)
        {
            //declare list of productvm
            List<ProductVM> listOfProductVM;
            //set page number
            var pageNumber = page ?? 1;

            using (Db db = new Db())
            {
                //init list
                listOfProductVM = db.Products.ToArray()
                                    .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                                    .Select(x => new ProductVM(x))
                                    .ToList();

                //populate categories select list
                ViewBag.Categories = new SelectList(db.Catagories.ToList(), "Id", "Name");
                //set selected category
                ViewBag.SelectedCat = catId.ToString();
            }
            //set pagination
            var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 5);
            ViewBag.OnePageOfProducts = onePageOfProducts;
            //return view
            return View(listOfProductVM);
        }
        /// <summary>
        /// Get method for editing a products information in the store
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the edit products view</returns>
        //GET : Admin/Shop/EditProduct/id
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            //declare productvm
            ProductVM model;

            using (Db db = new Db())
            {
                //get product
                ProductDTO dto = db.Products.Find(id);
                //make sure product exists
                if (dto == null)
                {
                    return Content("That product doesnt exists");
                }
                //init model
                model = new ProductVM(dto);
                //make select list
                model.Categories = new SelectList(db.Catagories.ToList(), "Id", "Name");
                //get all gallery images
                model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                                .Select(fn => Path.GetFileName(fn));
            }
            //return view with model
            return View(model);
        }
        /// <summary>
        /// Post method for editing a product in the store
        /// </summary>
        /// <param name="model"></param>
        /// <param name="file"></param>
        /// <returns>Returns an error message if the product name exists or displays confirmation a product has been edited</returns>
        [HttpPost]
        //POST : Admin/Shop/EditProduct/id
        public ActionResult EditProduct(ProductVM model, HttpPostedFileBase file)
        {
            //get product id
            int id = model.Id;
            //populate categories select list and gallery images
            using (Db db = new Db())
            {
                model.Categories = new SelectList(db.Catagories.ToList(), "Id", "Name");
            }
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                                .Select(fn => Path.GetFileName(fn));
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //make sure product name is unique
            using (Db db = new Db())
            {
                if (db.Products.Where(x => x.Id != id).Any(x => x.Name == model.Name))
                {
                    ModelState.AddModelError("", "That product name is taken!");
                    return View(model);
                }
            }
            //update product
            using (Db db = new Db())
            {
                ProductDTO dto = db.Products.Find(id);

                dto.Name = model.Name;
                dto.Slug = model.Name.Replace(" ", "-").ToLower();
                dto.Price = model.Price;
                dto.Description = model.Description;
                dto.CategoryId = model.CategoryId;
                dto.ImageName = model.ImageName;
                dto.Quantity = model.Quantity;

                CategoryDTO catDTO = db.Catagories.FirstOrDefault(x => x.Id == model.CategoryId);
                dto.CategoryName = catDTO.Name;

                db.SaveChanges();
            }
            //set tempdata message
            TempData["SM"] = "You have edited the product!";

            #region image upload
            //check for file upload
            if (file != null && file.ContentLength > 0)
            {
                //get ext
                string ext = file.ContentType.ToLower();
                //verify ext
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        {
                            ModelState.AddModelError("", "The Image was not uploaded - wrong image extension.");
                            return View(model);
                        }
                    }
                }


                //set upload directory paths
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");

                //delete files from directories
                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file2 in di1.GetFiles())
                    file2.Delete();

                foreach (FileInfo file3 in di2.GetFiles())
                    file3.Delete();

                //save image name
                string imageName = file.FileName;

                using (Db db = new Db())
                {
                    ProductDTO dto = db.Products.Find(id);
                    dto.ImageName = imageName;
                    db.SaveChanges();
                }
                //save original and thumb images
                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);

                file.SaveAs(path);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            //redirect
            #endregion

            //redirect
            return RedirectToAction("EditProduct");
        }
        /// <summary>
        /// Allows a product to be deleted from the store and database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns user to the products page</returns>
        //GET : Admin/Shop/DeleteProduct/id
        public ActionResult DeleteProduct(int id)
        {
            //delete product from db
            using (Db db = new Db())
            {
                ProductDTO dto = db.Products.Find(id);
                db.Products.Remove(dto);

                db.SaveChanges();
            }
            //delete product folder
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());

            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);

            //redirect
            return RedirectToAction("Products");
        }
        /// <summary>
        /// Allows a user to upload a number of images to be viewed in a gallery when viewing a product
        /// </summary>
        /// <param name="id"></param>
        
        [HttpPost]
        //POST : Admin/Shop/SaveGalleryImages
        public void SaveGalleryImages(int id)
        {
            //loop through files
            foreach (string fileName in Request.Files)
            {
                //init file
                HttpPostedFileBase file = Request.Files[fileName];
                //check its not null
                if (file != null && file.ContentLength > 0)
                {
                    //set directory
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                    string pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");
                    //set image path
                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);
                    //save original and thumb
                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }
            }

        }
        /// <summary>
        /// Allows a user to delete an image from the gallery
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imageName"></param>
        [HttpPost]
        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);
        }
        /// <summary>
        /// Allows a user to view all of the orders that have been placed in the store
        /// </summary>
        /// <returns>Takes user to the ordersforadmin view</returns>
        //GET : Admin/Shop/Orders
        public ActionResult Orders()
        {

            //init list of Orders
            List<OrdersForAdminVM> ordersForAdmin = new List<OrdersForAdminVM>();

            using (Db db = new Db())
            {
                //init list of orderVMs
                List<OrderVM> orders = db.Orders.ToArray().Select(x => new OrderVM(x)).ToList();
                //loop through list of OrderVMS

                foreach (var order in orders)
                {
                    //init product dict
                    Dictionary<string, int> productsAndQty = new Dictionary<string, int>();
                    //declare total
                    decimal total = 0m;
                    //init list of OrderDetailsDTO
                    List<OrderDetailsDTO> orderDetailsList = db.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                    //get username
                    UserDTO user = db.Users.Where(x => x.Id == order.UserId).FirstOrDefault();
                    string username = user.Username;
                    //loop through list of OrderDetailsDTO
                    foreach (var orderDetails in orderDetailsList)
                    {
                        //get product
                        ProductDTO product = db.Products.Where(x => x.Id == orderDetails.ProductId).FirstOrDefault();
                        //get product price
                        decimal price = product.Price;
                        //get product name
                        string productName = product.Name;
                        //add to product dict
                        productsAndQty.Add(productName, orderDetails.Quantity);

                        //get total
                        total += orderDetails.Quantity * price;

                    }

                    //add to ordersForAdminVM list
                    ordersForAdmin.Add(new OrdersForAdminVM()
                    {
                        OrderNumber = order.OrderId,
                        Username = username,
                        Total = total,
                        ProductsAndQty = productsAndQty,
                        CreatedAt = order.CreatedAt
                    });


                }
            }
                return View(ordersForAdmin);
        }
        /// <summary>
        /// Allows a user to delete an order that has been placed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirects the user to the orders page or displays an error that the order cant be cancelled</returns>
        //GET : Admin/Shop/DeleteOrder/id
        public ActionResult DeleteOrder(int id)
        {
            
            //delete the order from db
            using (Db db = new Db())
            {
                OrderDTO dto = db.Orders.Find(id);

                //check to see when the order was placed, if it is a day after they cant cancel the order
                if (dto.CreatedAt > DateTime.Now.AddHours(-24))
                {
                    db.Orders.Remove(dto);
                    db.SaveChanges();

                    ////init user dto
                    //UserDTO user = new UserDTO();
                    ////send an email to the user to tell them the order is cancelled
                    //var senderClient = new SmtpClient("smtp.gmail.com", 587)
                    //{
                    //    Credentials = new NetworkCredential("markriact@gmail.com", "eprbdxwogucwqdic"),
                    //    EnableSsl = true
                    //};
                    //senderClient.Send("markiact@gmail.com", user.EmailAddress, "Order has been cancelled", "Hello " + user.FirstName + " " + user.LastName + "," + " " + " An order you have placed has been cancelled, please contact the store to find out why!");
                }

            }
            

            //redirect
            return RedirectToAction("Orders");
        }
        /// <summary>
        /// Method for generating a PDF file of the orders view
        /// </summary>
        /// <param name="orders"></param>
        /// <returns>Returns the pdf report</returns>
        public ActionResult GenerateReport(OrdersForAdminVM orders)
        {
            var report = new ActionAsPdf("Orders", new { ordersForAdmin = orders });
            return report;
        }
        /// <summary>
        /// Generates a view with all of the users that are in the database
        /// </summary>
        /// <returns>Returns the customers view</returns>
        //GET : Admin/Shop/Customers
        public ActionResult Customers()
        {
            //init list of Customers
            List<CustomersForAdminVM> customersForAdmin = new List<CustomersForAdminVM>();

            using (Db db = new Db())
            {
                //init list of customerVMs
                List<UserVM> users = db.Users.ToArray().Select(x => new UserVM(x)).ToList();
                //loop through list of customerVMS

                foreach (var customer in users)
                {
                   
                    //init list of CustomerDetails
                    List<UserDTO> customerDetailsList = db.Users.Where(x => x.Id == customer.Id).ToList();

                    UserDTO user = db.Users.Where(x => x.Id == customer.Id).FirstOrDefault();
                    string username = user.Username;
                    string firstname = user.FirstName;
                    string lastname = user.LastName;
                    string email = user.EmailAddress;

                    //add to CustomersForAdminVM list
                    customersForAdmin.Add(new CustomersForAdminVM()
                    {
                        UserId = user.Id,
                        Username = username,
                        FirstName = firstname,
                        LastName = lastname,
                        EmailAddress = email
                    });


                }
            }
            return View(customersForAdmin);
        }
        
        /// <summary>
        /// Method for generating a PDF file of the users view
        /// </summary>
        /// <param name="orders"></param>
        /// <returns>Returns the pdf report</returns>
        public ActionResult GenerateReportUsers(CustomersForAdminVM users)
        {
            var report = new ActionAsPdf("Customers", new { customersForAdmin = users });
            return report;
        }
    }
    }
    

