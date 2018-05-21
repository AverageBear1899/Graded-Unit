using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Staff")]
    
    public class DashboardController : Controller
    {
        /// <summary>
        /// Returns view for admin dashboard
        /// </summary>
        /// <returns>Admin dashboard view</returns>
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}