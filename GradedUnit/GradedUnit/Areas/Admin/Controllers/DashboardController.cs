//Mark Riley
//30/05/18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Areas.Admin.Controllers
{
    /// <summary>
    /// Dashboard Controller which will return the actions available to a logged in staff member or admin
    /// </summary>
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