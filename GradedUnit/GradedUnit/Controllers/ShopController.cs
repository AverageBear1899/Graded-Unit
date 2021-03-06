﻿using GradedUnit.Models.Data;
using GradedUnit.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

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
        // GET: Shop/category/name
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
                //get category name
                var productCat = db.Products.Where(x => x.CategoryId == catId).FirstOrDefault();
                ViewBag.CategoryName = productCat.CategoryName;

            }
            //return view with list
            return View(productVMList);
        }

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