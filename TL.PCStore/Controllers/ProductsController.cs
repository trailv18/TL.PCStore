using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TL.PCStore.Repositories;

namespace TL.PCStore.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly INewsRepository newsRepository;

        public ProductsController()
        {
            productRepository = new ProductRepository();
            newsRepository = new NewsRepository();
        }
        // GET: Products
        public ActionResult Index(string search, string category, string brand,  string sortOrder, int? page)
        {
            ViewBag.SortOrder = new List<SelectListItem>() {              
                new SelectListItem() { Value="price_desc", Text= "Giá giảm dần" },
                new SelectListItem() { Value="price_asc", Text= "Giá tăng dần" },
                new SelectListItem() { Value="name_desc", Text= "Tên từ Z - A" },
                new SelectListItem() { Value="name_asc", Text= "Tên từ A - Z" },
            };

            var products = from p in productRepository.GetAllProducts()
                           where p.DeleteFlag == false
                           select p;

            if (!String.IsNullOrEmpty(category) || !String.IsNullOrEmpty(brand))
            {
                products = products.Where(p => p.Category.Url == category || p.Brand.Url == brand);

            }

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.ToUpper().Contains(search.ToUpper()));

            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "name_asc":
                    products = products.OrderBy(p => p.Name);
                    break;                
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;

            }
            if (products == null)
            {
                return HttpNotFound();
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View("_ListProducts",products.ToPagedList(pageNumber, pageSize));
        }

        //GET: Products/GetProductDetail/url=?
        public async Task<ActionResult> GetProductDetail(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return HttpNotFound();
            }
            var product = await productRepository.GetProductDetail(url);
            if(product != null)
            {
                return View("GetProductDetail", product);

            }
            else
            {
                return HttpNotFound();
            }
        }


        public ActionResult GetNews()
        {
            ViewBag.Title = "BÀI VIẾT MỚI";
            var news = newsRepository.GetAllNews();
            return PartialView("_GetNews", news.Take(2));
        }

        public ActionResult GetNewsViewCount()
        {
            ViewBag.Title = "BÀI VIẾT NHIỀU LƯỢT XEM";
            var news = newsRepository.GetAllNews().OrderByDescending(n => n.ViewCount);
            return PartialView("_GetNews", news.Take(3));
        }

    }
}