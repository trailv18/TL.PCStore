using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TL.PCStore.Repositories;

namespace TL.PCStore.Web.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly ICategoryRepository categoryRepository;

        public CategoriesController()
        {
            categoryRepository = new CategoryRepository();
        }
        // GET: Category name
        public ActionResult GetNameCategory()
        {
            var categories = categoryRepository.GetAllCategories().Where(c=>c.DeleteFlag == false);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Category", categories);
        }
    }
}