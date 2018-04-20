using System.Web.Mvc;

namespace GradedUnit.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        /// <summary>
        /// Returns the Area Name
        /// </summary>
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }
        /// <summary>
        /// Defines the Route and Default Action
        /// </summary>
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}