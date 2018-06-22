using GradedUnit.Models.Data;
using GradedUnit.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Controllers
{
    /// <summary>
    /// Controller that deals with the viewing of pages
    /// </summary>
    public class PagesController : Controller
    {
        // GET: Index/{page}
        /// <summary>
        /// Displays the page with a number of different additions depending on what was set up when an admin created the page
        /// </summary>
        /// <param name="page"></param>
        /// <returns>Returns the page with or without the sidebar depending on what the admin has setup</returns>
        public ActionResult Index(string page = "")
        {
                //get/set page slug
                if (page == "")
                    page = "home";
                //declare model and dto
                PageVM model;
                PageDTO dto;
                //check if page exists
                using (Db db = new Db())
            {
                if (! db.Pages.Any(x => x.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }
                //get page dto
                using (Db db = new Db())
            {
                dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }
                //set page title
                    ViewBag.PageTitle = dto.Title;
                //check for sidebar
                if (dto.HasSidebar == true)
            {
                    ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "No";
            }
                //init model
                model = new PageVM(dto);
                //return view with model
                return View(model);
        }
        /// <summary>
        /// Partial view for the pages menu
        /// </summary>
        /// <returns>Returns the pages menu with the list of pages</returns>
        public ActionResult PagesMenuPartial()
        {
            //declare a list of pagevm
            List<PageVM> pageVMList;
            //get all pages except home page
            using (Db db = new Db()) 
            {
                pageVMList = db.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "home").Select(x => new PageVM(x)).ToList();
            }
            //return partial view with list
            return PartialView(pageVMList);
        }
        /// <summary>
        /// Partial view for the sidebar that is displayed on some pages
        /// </summary>
        /// <returns>Returns the sidebar partial view</returns>
        public ActionResult SidebarPartial()
        {
            //declare model
            SidebarVM model;
            //init model
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebar.Find(1);
                model = new SidebarVM(dto);
            }
                //return partial view with model
                return PartialView(model);
        }
    }
}