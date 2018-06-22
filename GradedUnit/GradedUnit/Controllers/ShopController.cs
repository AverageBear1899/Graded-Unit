using GradedUnit.Models.Data;
using GradedUnit.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Controllers
{
    /// <summary>
    /// Controller that holds the actions primarily dealing with the categories and product details in the shop
    /// </summary>
    public class ShopController : Controller
    {
        // GET: Shop
        /// <summary>
        /// Index controller
        /// </summary>
        /// <returns>Redirects user to the website index</returns>
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }
        /// <summary>
        /// Displays a list of the categories of items available
        /// </summary>
        /// <returns>Returns a partial view of the categories on the left side of the page</returns>
        public ActionResult CategoryMenuPartial()
        {
            //declare list of categoryVM
            List<CategoryVM> categoryVMList;
            //init list
            using (Db db = new Db())
            {
                categoryVMList = db.Catagories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }

            //return partial with list
            return PartialView(categoryVMList);
        }
        /// <summary>
        /// Takes the user to the product page based on the category they have chosen
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Returns the product list the user has chosen</returns>
        //GET: Shop/category/name
        public ActionResult Category(string name)
        {
            //declare list of productvm
            List<ProductVM> productVMList;

            using (Db db = new Db())
            {
                //get category id
                CategoryDTO categoryDTO = db.Catagories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDTO.Id;
                //init list
                productVMList = db.Products.ToArray().Where(x => x.CategoryId == catId).Select(x => new ProductVM(x)).ToList();
               // get category name
                var productCat = db.Products.Where(x => x.CategoryId == catId).FirstOrDefault();
                if (productCat == null)
                {
                    return View(productVMList);
                }
                else
                {
                    ViewBag.CategoryName = productCat.CategoryName;
                }

            }
            //return view with list
            return View(productVMList);
        }
        /// <summary>
        /// Displays the product details of a selected products
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Takes the user to the product details page depending on the choice</returns>
        // GET: Shop/product-details/name
        [ActionName("product-details")]
    public ActionResult ProductDetails(string name)
    {
        //declare vm and dto
        ProductVM model;
        ProductDTO dto;

        //init product id
        int id = 0;

        using (Db db = new Db())
        {
            //check if product exists
            if (!db.Products.Any(x => x.Slug.Equals(name)))
            {
                return RedirectToAction("Index", "Shop");
            }
            //init product dto
            dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();
            //get id
            id = dto.Id;
            //init model
            model = new ProductVM(dto);
        }
        //get images
        model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                            .Select(fn => Path.GetFileName(fn));
        //return view with model
        return View("ProductDetails", model);
    }
}
}