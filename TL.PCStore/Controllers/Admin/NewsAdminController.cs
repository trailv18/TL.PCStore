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
    public class NewsAdminController : Controller
    {
        private readonly INewsRepository newsRepository;

        public NewsAdminController()
        {
            newsRepository = new NewsRepository();
        }

        /// <summary>
        /// Get all news
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

            var news = from n in newsRepository.GetAllNews()
                       select n;
            if (!String.IsNullOrEmpty(search))
            {
                news = news.Where(n => n.Title.ToUpper().Contains(search.ToUpper()));
            }

            if (news == null)
            {
                return HttpNotFound();
            }

            int defaSize = (pageSize ?? 6);
            int pageNumber = (page ?? 1);
            ViewBag.total = news.Count();
            int itemPage = 0;

            itemPage = pageNumber * defaSize < news.Count() ? pageNumber * defaSize : news.Count();

            ViewBag.itemPage = itemPage;
            return View(news.ToPagedList(pageNumber, defaSize));
        }

        /// <summary>
        /// Get: create news
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Post: create news
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> Create(News news)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            news.UserId = userId;
            news.PostedOn = DateTime.Now;
            news.ViewCount = 0;
            news.Url = news.Url + "-" + DateTime.Now.ToString("ddHHmmss");
            await newsRepository.AddNews(news);
            return RedirectToAction("index");
        }

        
        [CustomAuthorize("Admin")]
        public async Task<ActionResult> FindById(string id)
        {
            var news = await newsRepository.Find(Int32.Parse(id));
            return Json(news, MediaTypeNames.Text.Plain);
        }

        [CustomAuthorize("Admin")]
        public async Task<ActionResult> DeleteFlag(int id, bool status)
        {
            var news = await newsRepository.Find(id);
            if(status == true)
            {
                news.Published = status;
                await newsRepository.UpdateNews(news);
                return Json("Đã hiển thị bài viết.", MediaTypeNames.Text.Plain);
            }
            else
            {
                news.Published = status;
                await newsRepository.UpdateNews(news);
                return Json("Đã ẩn bài viết.", MediaTypeNames.Text.Plain);
            }
        }

        [CustomAuthorize("Admin")]
        public async Task<ActionResult> DeleteSelectedNews(string[] ids)
        {
            bool result = false;
            List<int> listId = ids.Select(x => Int32.Parse(x)).ToList();

            for (int i = 0; i < listId.Count(); i++)
            {
                var ns = await newsRepository.Find(listId[i]);
                if (ns == null)
                {
                    TempData["error-message"] = MessageConstants.NEWS_NOT_EXIST;
                    return RedirectToAction("index");
                }
                else
                {           
                    result = await newsRepository.DeleteNews(ns);
                }

            }

            if (result)
            {
                TempData["message"] = MessageConstants.NEWS_SUCCESS_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                TempData["error-message"] = MessageConstants.NEWS_ERROR_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        private bool ValidateNews(string title, string url, string content)
        {
            if (title.Length < 10 || title.Length > 255)
            {
                TempData["error-message"] = MessageConstants.NEWS_ERROR_NAME_LENTH;
                return false;
            }
            return true;
        }
    }
}