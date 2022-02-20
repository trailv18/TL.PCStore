using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Mvc;
using TL.PCStore.Constants;
using TL.PCStore.Filters;
using TL.PCStore.Models;
using TL.PCStore.Repositories;

namespace TL.PCStore.Controllers.Admin
{
    //[CustomAuthenticationFilter]
    public class CategoryAdminController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;
        private readonly PcStoreDbContext db;

        public CategoryAdminController()
        {
            categoryRepository = new CategoryRepository();
            productRepository = new ProductRepository();
            db = new PcStoreDbContext();

        }


        /// <summary>
        /// Get all category
        /// </summary>
        /// <param name="search"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [CustomAuthorize("Admin")]

        public ActionResult Index(string search, int? page, int? pageSize)
        {

            ViewBag.PageSize = new List<SelectListItem>() {
                new SelectListItem() { Value="6", Text= "6" },
                new SelectListItem() { Value="9", Text= "9" },
                new SelectListItem() { Value="12", Text= "12" },
                new SelectListItem() { Value="15", Text= "15" },
                new SelectListItem() { Value="18", Text= "18" },
                new SelectListItem() { Value="21", Text= "21" }
            };


            var categories = from c in categoryRepository.GetAllCategories()
                             select c;

            if (!String.IsNullOrEmpty(search))
            {
                categories = categories.Where(c => c.Name.ToUpper().Contains(search.ToUpper()));
            }

            int defaSize = (pageSize ?? 6);
            int pageNumber = (page ?? 1);
            ViewBag.total = categories.Count();
            int itemPage = 0;

            itemPage = pageNumber * defaSize < categories.Count() ? pageNumber * defaSize : categories.Count();

            ViewBag.itemPage = itemPage;

            return View(categories.ToPagedList(pageNumber, defaSize));
        }

        /// <summary>
        /// Get: create category
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Post: create category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> Create(Category category)
        {

            var categoryByName = await categoryRepository.FindByName(category.Name);
            bool valid = ValidateCategory(category.Name);
            if (categoryByName != null)
            {
                TempData["error-message"] = MessageConstants.CATEGORY_DUPLICATE_NAME;
                return View();
            }
            else if (!valid)
            {
                return View();
            }

            category.CreateDate = DateTime.Now;
            category.DeleteFlag = false;
            category.Url = category.Url + "-" + DateTime.Now.ToString("ddHHmmss");
            bool result = await categoryRepository.AddCategory(category);

            if (result)
            {
                TempData["message"] = MessageConstants.CATEGORY_SUCCESS_CREATE;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error-message"] = MessageConstants.CATEGORY_ERROR_CREATE;
                return View();
            }

        }

        /// <summary>
        /// Get: edit category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["error-message"] = MessageConstants.CATEGORY_NOT_EXIST;
                return RedirectToAction("index");
            }

            int categoryId = (int)id;
            Category category = await categoryRepository.Find(categoryId);

            if (category == null)
            {
                TempData["error-message"] = MessageConstants.CATEGORY_NOT_EXIST;
                return RedirectToAction("index");
            }

            return View(category);
        }

        /// <summary>
        /// Post: edit category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> Edit(Category category)
        {
            bool valid = ValidateCategory(category.Name);
            if (!valid)
            {
                return View();
            }

            if (db.Categories.Any(c => c.Name.Equals(category.Name) && c.Id != category.Id))
            {
                TempData["error-message"] = MessageConstants.CATEGORY_DUPLICATE_NAME;
                return View(category);
            }

            category.UpdateDate = DateTime.Now;
            bool result = await categoryRepository.UpdateCategory(category);
            if (result)
            {
                TempData["message"] = MessageConstants.CATEGORY_SUCCESS_UPDATE;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error-message"] = MessageConstants.CATEGORY_ERROR_UPDATE;
                return View();
            }
        }

        /// <summary>
        /// Get category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomAuthorize("Admin")]
        public async Task<JsonResult> FindById(string id)
        {
            var category = await categoryRepository.Find(Int32.Parse(id));
            return Json(category, MediaTypeNames.Text.Plain);
        }

        public async Task<ActionResult> DeleteFlag(int id, bool status)
        {
            var category = await categoryRepository.Find(id);
            if ( status==true)
            {
                category.DeleteFlag = status;
                await categoryRepository.UpdateCategory(category);
                return Json("Đã ẩn loại sản phẩm.", MediaTypeNames.Text.Plain);
            }
            else {
                category.DeleteFlag = status;
                await categoryRepository.UpdateCategory(category);
                return Json("Đã hiển thị loại sản phẩm.", MediaTypeNames.Text.Plain);
            }
        }

        /// <summary>
        /// Delete category or list category by id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> DeleteSelectedCategory(string[] ids)
        {
            bool result = false;
            List<int> listId = ids.Select(x => Int32.Parse(x)).ToList();

            for (int i = 0; i < listId.Count(); i++)
            {
                var ctg = await categoryRepository.Find(listId[i]);
                if (ctg == null)
                {
                    TempData["error-message"] = MessageConstants.CATEGORY_NOT_EXIST;
                    return RedirectToAction("index");
                }
                else
                {
                    var countCategoryInProduct = productRepository.GetAllProducts().Where(p => p.CategoryId == ctg.Id).Count();
                    if (countCategoryInProduct > 0)
                    {
                        TempData["error-message"] = MessageConstants.CATEGORY_INVALID_DELETE;
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    result = await categoryRepository.DeleteCategory(ctg);
                }

            }

            if (result)
            {
                TempData["message"] = MessageConstants.CATEGORY_SUCCESS_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {

                TempData["error-message"] = MessageConstants.CATEGORY_ERROR_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        /// <summary>
        /// Validate category
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool ValidateCategory(string name)
        {
            if (name.Length < 2 || name.Length > 255)
            {
                TempData["error-message"] = MessageConstants.CATEGORY_ERROR_NAME_LENTH;
                return false;
            }
            return true;
        }
    }
}