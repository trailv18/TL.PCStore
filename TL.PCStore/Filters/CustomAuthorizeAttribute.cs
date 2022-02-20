using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TL.PCStore.Models;
using TL.PCStore.Repositories;

namespace TL.PCStore.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
           
            var email = Convert.ToString(httpContext.Session["UserName"]);
            if (!string.IsNullOrEmpty(email))
            {
                using (var context = new PcStoreDbContext())
                {
                    var userRoles = (from u in context.Users
                                     join roleMapping in context.UserRoles on u.Id equals roleMapping.UserId
                                     join r in context.Roles on roleMapping.RoleId equals r.Id
                                     where u.Email == email
                                     select r.Name).ToList();


                    foreach (var role in allowedroles)
                    {
                        foreach (var userRole in userRoles)
                        {
                            if (role == userRole)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "home" },
                    { "action", "unauthorized" }
               });
        }
    }
}