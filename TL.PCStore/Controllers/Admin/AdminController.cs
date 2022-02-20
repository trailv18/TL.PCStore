using System.Web.Mvc;
using TL.PCStore.Filters;

namespace TL.PCStore.Controllers.Admin
{
    [CustomAuthenticationFilter]
    public class AdminController : Controller
    {
        // GET: Admin
        [CustomAuthorize("Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}