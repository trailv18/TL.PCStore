using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using TL.PCStore.Constants;
using TL.PCStore.Filters;
using TL.PCStore.Models;
using TL.PCStore.Repositories;

namespace TL.PCStore.Controllers.Admin
{
    [CustomAuthenticationFilter]
    public class ProductAdminController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IBrandRepository brandRepository;
        private readonly PcStoreDbContext db;
        private readonly IOrderDetailRepository orderDetailRepository;
        public ProductAdminController()
        {
            productRepository = new ProductRepository();
            categoryRepository = new CategoryRepository();
            brandRepository = new BrandRepository();
            db = new PcStoreDbContext();
            orderDetailRepository = new OrderDetailRepository();
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

            var products = from p in productRepository.GetAllProducts()
                           select p;

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(s => s.Name.ToUpper().Contains(search.ToUpper()));
            }

            int defaSize = (pageSize ?? 6);
            int pageNumber = (page ?? 1);
            ViewBag.total = products.Count();
            int itemPage = 0;

            itemPage = pageNumber * defaSize < products.Count() ? pageNumber * defaSize : products.Count();

            ViewBag.itemPage = itemPage;
            return View(products.ToPagedList(pageNumber, defaSize));
        }

        /// <summary>
        /// Get product detail
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ActionResult> Detail(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                TempData["error-message"] = MessageConstants.PRODUCT_NOT_EXIST;
                return View();
            }
            var product = await productRepository.GetProductDetail(url);
            if (product != null)
            {
                return View(product);
            }
            else
            {
                TempData["error-message"] = MessageConstants.PRODUCT_NOT_EXIST;
                return View();
            }
        }

        /// <summary>
        /// Get: create product
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize("Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryRepository.GetAllCategories().Where(c => c.DeleteFlag == false), "Id", "Name");
            ViewBag.BrandId = new SelectList(brandRepository.GetAllBrands().Where(b => b.DeleteFlag == false), "Id", "Name");
            return View();
        }

        /// <summary>
        /// Post:create product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> Create(Product product, HttpPostedFileBase file)
        {
            ViewBag.CategoryId = new SelectList(categoryRepository.GetAllCategories().Where(c => c.DeleteFlag == false), "Id", "Name", product.CategoryId);
            ViewBag.BrandId = new SelectList(brandRepository.GetAllBrands().Where(b => b.DeleteFlag == false), "Id", "Name", product.BrandId);

            if (product == null)
            {
                TempData["error-message"] = MessageConstants.PRODUCT_NOT_EXIST;
                return View();
            }

            var productByName = await productRepository.FindByName(product.Name);
            bool valid = ValidateProduct(product.Name, product.Price, product.Stock);
            if (!valid)
            {
                return View();
            }

            if (productByName != null)
            {
                TempData["error-message"] = MessageConstants.PRODUCT_DUPLICATE_NAME;
                return View();
            }

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var urlFile = "~/Uploads/" + fileName;

                var checkLink = await productRepository.GetProductByPicture(urlFile);
                if (checkLink != null)
                {
                    urlFile = "~/Uploads/" + "pcs" + fileName;
                }
                product.Picture = urlFile;
                file.SaveAs(Server.MapPath(urlFile));
            }
            else
            {
                TempData["error-message"] = MessageConstants.INVALID_FILE;
                return View();
            }

            product.CreateDate = DateTime.Now;
            product.DeleteFlag = false;
            product.Url = product.Url + "-" + DateTime.Now.ToString("ddHHmmss");
            var result = await productRepository.AddProduct(product);

            if (result)
            {
                TempData["message"] = MessageConstants.PRODUCT_SUCCESS_CREATE;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error-message"] = MessageConstants.PRODUCT_ERROR_CREATE;
                return View();
            }
        }

        /// <summary>
        /// Get: create product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["error-message"] = MessageConstants.PRODUCT_NOT_EXIST;
                return RedirectToAction("index");
            }

            int productId = (int)id;
            Product product = await productRepository.Find(productId);

            if (product == null)
            {
                TempData["error-message"] = MessageConstants.PRODUCT_NOT_EXIST;
                return RedirectToAction("index");
            }
            ViewBag.CategoryId = new SelectList(categoryRepository.GetAllCategories().Where(c => c.DeleteFlag == false), "Id", "Name");
            ViewBag.BrandId = new SelectList(brandRepository.GetAllBrands().Where(b => b.DeleteFlag == false), "Id", "Name");
            return View(product);
        }

        /// <summary>
        /// Post: create product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> Edit(Product product, HttpPostedFileBase file)
        {
            ViewBag.CategoryId = new SelectList(categoryRepository.GetAllCategories().Where(c => c.DeleteFlag == false), "Id", "Name", product.CategoryId);
            ViewBag.BrandId = new SelectList(brandRepository.GetAllBrands().Where(b => b.DeleteFlag == false), "Id", "Name", product.BrandId);

            if (file != null)
            {
                string filePath = Server.MapPath(product.Picture);
                System.IO.File.Delete(filePath);

                var fileName = Path.GetFileName(file.FileName);
                var urlFile = "~/Uploads/" + fileName;

                var checkLink = await productRepository.GetProductByPicture(urlFile);
                if (checkLink != null)
                {
                    urlFile = "~/Uploads/" + "pcs" + fileName;
                }

                product.Picture = urlFile;
                file.SaveAs(Server.MapPath(urlFile));
            }
            else
            {
                product.Picture = product.Picture;
            }

            if (db.Products.Any(p => p.Name.Equals(product.Name) && p.Id != product.Id))
            {
                TempData["error-message"] = MessageConstants.PRODUCT_DUPLICATE_NAME;
                return View(product);
            }

            bool valid = ValidateProduct(product.Name, product.Price, product.Stock);
            if (!valid)
            {
                return View(product);
            }

            product.UpdateDate = DateTime.Now;
            bool result = await productRepository.UpdateProduct(product);
            if (result)
            {
                TempData["message"] = MessageConstants.PRODUCT_SUCCESS_UPDATE;
                return RedirectToAction("index");
            }
            else
            {
                TempData["error-message"] = MessageConstants.PRODUCT_ERROR_UPDATE;
                return View(product);
            }
        }

       
        /// <summary>
        /// Find prooduct by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> FindById(string id)
        {
            var product = await productRepository.Find(Int32.Parse(id));
            return Json(product, MediaTypeNames.Text.Plain);
        }

        public async Task<ActionResult> DeleteFlag(int id, bool status)
        {
            var product = await productRepository.Find(id);
            if (status == true)
            {
                product.DeleteFlag = status;
                await productRepository.UpdateProduct(product);
                return Json("Đã ẩn sản phẩm.", MediaTypeNames.Text.Plain);
            }
            else
            {
                product.DeleteFlag = status;
                await productRepository.UpdateProduct(product);
                return Json("Đã hiển thị sản phẩm.", MediaTypeNames.Text.Plain);
            }
        }

        /// <summary>
        /// delete product or list product by id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteSelectedProduct(string[] ids)
        {
            bool result = false;
            List<int> listId = ids.Select(x => Int32.Parse(x)).ToList();

            for (int i = 0; i < listId.Count(); i++)
            {
                var prd = await productRepository.Find(listId[i]);
                if (prd == null)
                {
                    TempData["error-message"] = MessageConstants.PRODUCT_NOT_EXIST;
                    return RedirectToAction("index");
                }
                else
                {
                    var countProductInOrder = orderDetailRepository.GetAlls().Where(p => p.ProductId == prd.Id).Count();
                    if (countProductInOrder > 0)
                    {
                        TempData["error-message"] = MessageConstants.PRODUCT_INVALID_DELETE;
                        return Redirect(Request.UrlReferrer.ToString());
                    }
                    result = await productRepository.DeleteProduct(prd);
                }

            }

            if (result)
            {
                TempData["message"] = MessageConstants.PRODUCT_SUCCESS_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {

                TempData["error-message"] = MessageConstants.PRODUCT_ERROR_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        /// <summary>
        /// validate product
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        private bool ValidateProduct(string name, decimal price, int stock)
        {
            if (name.Length < 5 || name.Length > 255)
            {
                TempData["error-message"] = MessageConstants.PRODUCT_ERROR_NAME_LENTH;
                return false;
            }
            else if (price < 10000 || price > 100000000)
            {
                TempData["error-message"] = MessageConstants.PRODUCT_ERROR_PRICE;
                return false;
            }
            else if (stock < 1 || stock > 1000)
            {
                TempData["error-message"] = MessageConstants.PRODUCT_ERROR_STOCK;
                return false;
            }
            return true;
        }
    }
}