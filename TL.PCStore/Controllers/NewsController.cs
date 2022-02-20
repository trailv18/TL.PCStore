using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TL.PCStore.Models;
using TL.PCStore.Repositories;

namespace TL.PCStore.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository newsRepository;
        private readonly IProductRepository productRepository;
        private readonly IOrderDetailRepository orderDetailRepository;

        public NewsController()
        {
            newsRepository = new NewsRepository();
            productRepository = new ProductRepository();
            orderDetailRepository = new OrderDetailRepository();
        }

        public ActionResult ListNews(int? page)
        {
            var news = from n in newsRepository.GetAllNews()
                       select n;

            if (news == null)
            {
                return HttpNotFound();
            }

            int pageSize = 7;
            int pageNumber = (page ?? 1);

            return View(news.ToPagedList(pageNumber, pageSize));
        }

        public async Task<ActionResult> GetNews(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return HttpNotFound();
            }
            var news = await newsRepository.FindByUrl(url);

            if (news != null)
            {
                if (news.ViewCount == null)
                {
                    news.ViewCount = 1;
                    await newsRepository.UpdateNews(news);
                }
                else
                {
                    news.ViewCount = news.ViewCount + 1;
                    await newsRepository.UpdateNews(news);
                }
                return View("GetNews", news);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult GetProductBestSale()
        {
            ViewBag.Title = "SẢN BÁN CHẠY";
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
                                Qty = o != null ? o.Qty : 0
                            }).ToList()
                            .GroupBy(x => new { x.Id, x.Name, x.Price, x.Pic, x.Url, x.Stock })
                            .Select(x => new ProductBestSaleViewModel
                            {
                                Id = x.Key.Id,
                                Name = x.Key.Name,
                                Picture = x.Key.Pic,
                                Url = x.Key.Url,
                                Stock = x.Key.Stock,
                                Price = x.Key.Price,
                                Qt = x.Sum(p => p.Qty)
                            }).OrderByDescending(x => x.Qt); return PartialView("_NewProduct", products.Take(15));
        }
    }
}