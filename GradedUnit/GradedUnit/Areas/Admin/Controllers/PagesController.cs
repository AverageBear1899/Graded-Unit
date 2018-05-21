﻿using GradedUnit.Models.Data;
using GradedUnit.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Staff")]

    public class PagesController : Controller
    {
        /// <summary>
        /// Returns a list of pages on the website
        /// </summary>
        /// <returns>Pages list view</returns>
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare a list of pageVM
            List<PageVM> pagesList;
            
            using (Db db = new Db())
            {
                //Init the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            //Return view
            return View(pagesList);
        }
        /// <summary>
        /// Allows an admin to create a new page on the website
        /// </summary>
        /// <returns>Takes user to the add page view</returns>
        //GET : Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        /// <summary>
        /// Post method for a user to create new pages on the website
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns user to the add page view or displays error that the page exists</returns>
        //POST  : Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                //declare slug
                string slug;
                //init the pagedto
                PageDTO dto = new PageDTO();
                //dto title
                dto.Title = model.Title; 
                //check for and set slug 
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                //make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x=> x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                }
                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;
                //Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //Set TempData messege
            TempData["SM"] = "You have added a new page!";

            //Redirect
            return RedirectToAction("AddPage");
        }
        /// <summary>
        /// Allows an admin to edit a page on the website
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Takes user to the edit page view or displays that the page already exists</returns>
        //GET : Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //declare pageVM
            PageVM model;
            using (Db db = new Db())
            {
                //get the page
                PageDTO dto = db.Pages.Find(id);
            //confirm page exists
            if (dto == null)
                {
                    return Content("The page doesnt exist.");
                }

                //init pageVM
                model = new PageVM(dto);

            }
            //return view with model
            return View(model);
        }
        /// <summary>
        /// Post method for editing a page
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Takes user to the edit page view or displays error that page cant be edited</returns>
        //POST : Admin/Pages/EditPage/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                //get page id
                int id = model.Id;
                //declare slug
                string slug = "home";
                //get page
                PageDTO dto = db.Pages.Find(id);
                //dto the title
                dto.Title = model.Title;
                //check for slug and set
                if (model.Slug != "home")
                {
                    if(string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                //make sure title and slug are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                    {
                    ModelState.AddModelError("", "That title or slug already exists.");
                    return View(model);
                    }
                //dto the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                //save the dto
                db.SaveChanges();
            }
            //set tempdata message
            TempData["SM"] = "You have edited the page!";
            //redirect
            return RedirectToAction("EditPage");
        }
        /// <summary>
        /// Allows user to view the details of a page
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Takes user to the page details view</returns>
        //GET : Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            //declare pagevm
            PageVM model;

            using (Db db = new Db())
            {
                //get the page
                PageDTO dto = db.Pages.Find(id);

                //confirm the page exists
                if(dto == null)
                {
                    return Content("The Page Doesn't Exist.");
                }

                //init pagevm
                model = new PageVM(dto);
            }
            //return view with model
            return View(model);
        }
        /// <summary>
        /// Allows an admin to delete a page from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns user to the index page once a page has been deleted</returns>
        //GET : Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                //get the page
                PageDTO dto = db.Pages.Find(id);

                //remove the page
                db.Pages.Remove(dto);

                //save
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Allows an admin to change the order in which the pages will be displayed
        /// </summary>
        /// <param name="id"></param>
        //POST : Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                //set initial count
                int count = 1;
                //declare page dto
                PageDTO dto;
                //set sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;

                }
            }

        }
        /// <summary>
        /// Allows admin to edit the content of the sidebar on a page
        /// </summary>
        /// <returns>Takes user to the edit sidebar view</returns>
        //GET : Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            //declare model
            SidebarVM model;

            using (Db db = new Db())
            {
                //get the dto
                SidebarDTO dto = db.Sidebar.Find(1);
                //init model
                model = new SidebarVM(dto);
            }
            //return view with model
            return View(model);
        }
        /// <summary>
        /// Post method for editing the sidebar on a page
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Redirects to the edit sidebar page once the sidebar has been edited</returns>
        //POST : Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                //get dto
                SidebarDTO dto = db.Sidebar.Find(1);
                //dto body
                dto.Body = model.Body;
                //save
                db.SaveChanges();
            }
            //set tempdata message
            TempData["SM"] = "You have edited the sidebar!";

            //redirect
            return RedirectToAction("EditSidebar");
        }



    }

}