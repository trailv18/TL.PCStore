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
    [CustomAuthenticationFilter]
    public class BrandAdminController : Controller
    {
        private readonly IBrandRepository brandRepository;
        private readonly IProductRepository productRepository;
        private readonly PcStoreDbContext db;

        public BrandAdminController()
        {
            brandRepository = new BrandRepository();
            productRepository = new ProductRepository();
            db = new PcStoreDbContext();
        }


        /// <summary>
        /// Get: get all brand
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

            var brands = from b in brandRepository.GetAllBrands()
                         select b;
            if (!String.IsNullOrEmpty(search))
            {
                brands = brands.Where(b => b.Name.ToUpper().Contains(search.ToUpper()));
            }

            int defaSize = (pageSize ?? 6);
            int pageNumber = (page ?? 1);
            ViewBag.total = brands.Count();
            int itemPage = 0;

            itemPage = pageNumber * defaSize < brands.Count() ? pageNumber * defaSize : brands.Count();

            ViewBag.itemPage = itemPage;
            return View(brands.ToPagedList(pageNumber, defaSize));
        }

        /// <summary>
        /// Get: create brand
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize("Admin")]
        public ActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Post: create brand
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]

        public async Task<ActionResult> Create(Brand brand)
        {
            if (brand == null)
            {
                TempData["error-message"] = MessageConstants.BRAND_NOT_EXIST;
                return RedirectToAction("index");
            }

            var brandByName = await brandRepository.FindByName(brand.Name);
            bool valid = ValidateBrand(brand.Name);
            if (!valid)
            {
                return View();
            }

            if (brandByName != null)
            {
                TempData["error-message"] = MessageConstants.BRAND_DUPLICATE_NAME;
                return View();
            }

            brand.CreateDate = DateTime.Now;
            brand.DeleteFlag = false;
            brand.Url = brand.Url + "-" + DateTime.Now.ToString("ddHHmmss");
            bool result = await brandRepository.AddBrand(brand);
            if (result)
            {
                TempData["message"] = MessageConstants.BRAND_SUCCESS_CREATE;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error-message"] = MessageConstants.BRAND_ERROR_CREATE;
                return View();
            }
        }

       /// <summary>
       /// Get: edit brand
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["error-message"] = MessageConstants.BRAND_NOT_EXIST;
                return RedirectToAction("index");
            }

            int brandId = (int)id;
            var brand = await brandRepository.Find(brandId);

            if (brand == null)
            {
                TempData["error-message"] = MessageConstants.BRAND_NOT_EXIST;
                return RedirectToAction("index");
            }

            return View(brand);
        }

       /// <summary>
       /// Post: edit brand
       /// </summary>
       /// <param name="brand"></param>
       /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> Edit(Brand brand)
        {
            bool valid = ValidateBrand(brand.Name);

            if (!valid)
            {
                return View(brand);
            }

            if (db.Brands.Any(b => b.Name.Equals(brand.Name) && b.Id != brand.Id))
            {
                TempData["error-message"] = MessageConstants.BRAND_DUPLICATE_NAME;
                return View(brand);
            }

            brand.UpdateDate = DateTime.Now;
            bool result = await brandRepository.UpdateBrand(brand);
            if (result)
            {
                TempData["message"] = MessageConstants.BRAND_SUCCESS_UPDATE;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error-message"] = MessageConstants.BRAND_ERROR_UPDATE;
                return View(brand);
            }
        }

        /// <summary>
        /// Find brand by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> FindById(string id)
        {
            var brand = await brandRepository.Find(Int32.Parse(id));
            return Json(brand, MediaTypeNames.Text.Plain);
        }


        /// <summary>
        /// Delete flag brand
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> DeleteFlag(int id, bool status)
        {
            var brand = await brandRepository.Find(id);
            if(status == true)
            {
                brand.DeleteFlag = status;
                await brandRepository.UpdateBrand(brand);
                return Json("Đã ẩn thương hiệu.", MediaTypeNames.Text.Plain);
            }
            else
            {
                brand.DeleteFlag = status;
                await brandRepository.UpdateBrand(brand);
                return Json("Đã hiển thị thương hiệu.", MediaTypeNames.Text.Plain);
            }
        }

        /// <summary>
        /// Delete brand or list brand by id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> DeleteSelectedBrand(string[] ids)
        {
            bool result = false;
            List<int> listId = ids.Select(x => Int32.Parse(x)).ToList();

            for (int i = 0; i < listId.Count(); i++)
            {
                var br = await brandRepository.Find(listId[i]);
                if (br == null)
                {
                    TempData["error-message"] = MessageConstants.BRAND_NOT_EXIST;
                    return RedirectToAction("index");
                }
                else
                {
                    var countCategoryInProduct = productRepository.GetAllProducts().Where(p => p.CategoryId == br.Id).Count();
                    if (countCategoryInProduct > 0)
                    {
                        TempData["error-message"] = MessageConstants.BRAND_INVALID_DELETE;
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    result = await brandRepository.DeleteBrand(br);
                }

            }

            if (result)
            {
                TempData["message"] = MessageConstants.BRAND_SUCCESS_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {

                TempData["error-message"] = MessageConstants.BRAND_ERROR_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        /// <summary>
        /// Validate brand
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool ValidateBrand(string name)
        {
            if (name.Length < 2 || name.Length > 255)
            {
                TempData["error-message"] = MessageConstants.BRAND_ERROR_NAME_LENTH;
                return false;
            }
            return true;
        }
    }
}