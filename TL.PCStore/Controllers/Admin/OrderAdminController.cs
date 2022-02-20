using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Mvc;
using TL.PCStore.Repositories;

namespace TL.PCStore.Controllers.Admin
{
    public class OrderAdminController : Controller
    {
        private readonly IOrderRepository orderRepository;


        public OrderAdminController()
        {
            orderRepository = new OrderRepository();
        }
       
        /// <summary>
        /// Get all order
        /// </summary>
        /// <param name="search"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Index(string search, string status, int? page, int? pageSize)
        {

            ViewBag.Status = new List<SelectListItem>() {
                new SelectListItem() { Value="Đang xử lý", Text= "Đang xử lý" },
                new SelectListItem() { Value="Đang giao", Text= "Đang giao" },
                new SelectListItem() { Value="Đã giao", Text= "Đã giao" },
                new SelectListItem() { Value="Đã hủy", Text= "Đã hủy" },
            };

            ViewBag.PageSize = new List<SelectListItem>() {
                new SelectListItem() { Value="6", Text= "6" },
                new SelectListItem() { Value="9", Text= "9" },
                new SelectListItem() { Value="12", Text= "12" },
                new SelectListItem() { Value="15", Text= "15" },
                new SelectListItem() { Value="18", Text= "18" },
                new SelectListItem() { Value="21", Text= "21" }
            };

            var orders = from o in orderRepository.GetAllOrder()
                         select o;

            if (!String.IsNullOrEmpty(search))
            {
                orders = orders.Where(s => s.User.Mobile.Contains(search));
            }

            if (!String.IsNullOrEmpty(status))
            {
                orders = orders.Where(s => s.OrderStatus.Contains(status));
            }


            int defaSize = (pageSize ?? 6);
            int pageNumber = (page ?? 1);
            ViewBag.total = orders.Count();
            int itemPage = 0;

            itemPage = pageNumber * defaSize < orders.Count() ? pageNumber * defaSize : orders.Count();

            ViewBag.itemPage = itemPage;
            return View(orders.ToPagedList(pageNumber, defaSize));
        }


        /// <summary>
        /// Edit status order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<ActionResult> EditStatus(string id, string status)
        {
            var idOrder = new Guid(id);
            var order = await orderRepository.Find(idOrder);
            order.OrderStatus = status;
            await orderRepository.UpdateOrder(order);
            return Json("Cập nhật thông tin thành công!", MediaTypeNames.Text.Plain);
        }
    }
}