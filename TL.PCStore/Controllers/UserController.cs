using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TL.PCStore.Constants;
using TL.PCStore.Models;
using TL.PCStore.Repositories;

namespace TL.PCStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;


        public UserController()
        {
            userRepository = new UserRepository();
            orderRepository = new OrderRepository();
            orderDetailRepository = new OrderDetailRepository();
        }
        // GET: User

        public async Task<ActionResult> GetUserInformation()
        {
            var user = await userRepository.Find((int)Session["UserId"]);
            return View(user);
        }
        public async Task<ActionResult> GetOrder()
        {
            var user = await userRepository.Find((int)Session["UserId"]);
            ViewBag.UserName = user.FullName;
            return View();
        }

        public ActionResult OrderProcessing()
        {
            var orderProcessing = orderRepository.GetAllOrder().Where(o => o.OrderStatus.Equals(OrderStatus.STATUS_PROCESSING) && o.UserId == (int)Session["UserId"]);
            ViewBag.Status = "Processing";
            return PartialView("_OrderStatus", orderProcessing);
        }

        public ActionResult OrderDelivary()
        {
            var orderDelivary = orderRepository.GetAllOrder().Where(o => o.OrderStatus.Equals(OrderStatus.STATUS_DELIVERY) && o.UserId == (int)Session["UserId"]);
            ViewBag.Status = "Delivary";
            return PartialView("_OrderStatus", orderDelivary);
        }
        public ActionResult OrderSuccess()
        {
            var orderSuccess = orderRepository.GetAllOrder().Where(o => o.OrderStatus.Equals(OrderStatus.STATUS_SUCCESS) && o.UserId == (int)Session["UserId"]);
            ViewBag.Status = "Success";
            return PartialView("_OrderStatus", orderSuccess);
        }

        public ActionResult OrderCancel()
        {
            var orderCancel = orderRepository.GetAllOrder().Where(o => o.OrderStatus.Equals(OrderStatus.STATUS_CANCEL) && o.UserId == (int)Session["UserId"]);
            ViewBag.Status = "Cancel";
            return PartialView("_OrderStatus", orderCancel);
        }

        public async Task<ActionResult> EditStatus(string id, string status)
        {
            var idOrder = new Guid(id);
            var order = orderRepository.GetAllOrder().Where(x=>x.Id == idOrder && x.UserId == (int)Session["UserId"]).FirstOrDefault();
            
            if(status == "Đã hủy")
            {
                order.OrderStatus = status;
                await orderRepository.UpdateOrder(order);
                return Json("Hủy đơn hàng thành công!", MediaTypeNames.Text.Plain);
            }
            else
            {
                order.OrderStatus = status;
                order.PaymentStatus = OrderStatus.STATUS_PAID;
                order.DateDelivery = DateTime.Now;
                await orderRepository.UpdateOrder(order);
                return Json("Xác nhận đơn hàng thành công!", MediaTypeNames.Text.Plain);
            }
        }
    }
}