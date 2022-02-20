using System.Linq;
using System.Web.Mvc;
using TL.PCStore.Models;
using TL.PCStore.Repositories;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly INewsRepository newsRepository;
        private readonly IOrderDetailRepository orderDetailRepository;

        public HomeController()
        {
            productRepository = new ProductRepository();
            newsRepository = new NewsRepository();
            orderDetailRepository = new OrderDetailRepository();
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult UnAuthorized()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult GetNewProductLaptop()
        {
            ViewBag.Title = "LAPTOP, MÁY TÍNH XÁCH TAY MỚI";
            ViewBag.Name = "Mới";
            var products = productRepository.GetAllProducts().Where(p => p.DeleteFlag == false && p.Category.Name.Equals("Laptop"));
            return PartialView("_GetProduct", products.Take(5));
        }

        public ActionResult GetNewProductPC()
        {
            ViewBag.Title = "PC, MÁY TÍNH BÀN MỚI";
            ViewBag.Name = "Mới";
            var products = productRepository.GetAllProducts().Where(p => p.DeleteFlag == false && p.Category.Name.Equals("Máy tính bàn"));
            return PartialView("_GetProduct", products.Take(5));
        }

        public ActionResult GetProductBestSale()
        {
            ViewBag.Title = "SẢN PHẨM BÁN CHẠY";

            var products = (from p in productRepository.GetAllProducts().Where(p => p.DeleteFlag == false)
                            join o in orderDetailRepository.GetAlls() on p.Id equals o.ProductId
                            select new
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Pic = p.Picture,
                                Price = p.Price,
                                Stock = p.Stock,
                                Url = p.Url,
                                Dis = p.Discount,
                                Qty = o != null ? o.Qty : 0
                            }).ToList()
                            .GroupBy(x => new { x.Id, x.Name, x.Price, x.Pic, x.Url, x.Stock, x.Dis})
                            .Select(x => new ProductBestSaleViewModel
                            {
                                Id = x.Key.Id,
                                Name = x.Key.Name,
                                Picture = x.Key.Pic,
                                Url = x.Key.Url,
                                Stock = x.Key.Stock,
                                Price = x.Key.Price,
                                Discount = x.Key.Dis,
                                Qt = x.Sum(p=>p.Qty)
                            }).OrderByDescending(x => x.Qt);

            return PartialView("_GetProductBestSale", products.Take(10));
        }

        public ActionResult GetNews()
        {
            ViewBag.Title = "TIN CÔNG NGHỆ";
            var news = newsRepository.GetAllNews();
            return PartialView("_GetNewsNew", news.Take(4));
        }
    }
}
