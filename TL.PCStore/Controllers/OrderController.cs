using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TL.PCStore.Constants;
using TL.PCStore.Filters;
using TL.PCStore.Models;
using TL.PCStore.Repositories;

namespace TL.PCStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;

        private readonly PcStoreDbContext db;

        public OrderController()
        {
            userRepository = new UserRepository();
            productRepository = new ProductRepository();
            db = new PcStoreDbContext();

        }
        // GET: Order
        public async Task<ActionResult> Cart()
        {
            User user = new User();
            if(Session["UserId"] != null)
            {
                int userId = (int)Session["UserId"];
                user = await userRepository.Find(userId);
                
            }
            ViewBag.Name = user.FullName ?? null;
            ViewBag.Mobile = user.Mobile ?? null;
            ViewBag.UserAddress = user.Address ?? null;
            return View();
        }

        [CustomAuthenticationFilter]
        public ActionResult LoginOrder()
        {
            return RedirectToAction("Cart");

        }

        [HttpPost]
        public async Task<JsonResult> CheckOut(OrderDetail[] products, string name, string mobile, string address, string payment)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                DateOrder = DateTime.Now
            };

            decimal total = 0;

            foreach (var item in products)
            {
                var orderDetail = new OrderDetail
                {
                    Qty = item.Qty,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    OrderId = order.Id,
                    SubTotal = item.Price * item.Qty
                };

                total += (item.Price * item.Qty);
                db.OrderDetails.Add(orderDetail);
                var product = await productRepository.Find(item.ProductId);
                product.Stock -= item.Qty;
                await productRepository.UpdateProduct(product);
            }

            int userId = (int)Session["UserId"];
            var user = await userRepository.Find(userId);

            user.Mobile = mobile;
            user.Address = address;
            user.FullName = name;

            await userRepository.UpdateUser(user);

            order.Total = total;
            order.OrderStatus = OrderStatus.STATUS_PROCESSING;
            if(payment == "online")
            {
                order.Payment = payment;
                order.PaymentStatus = OrderStatus.STATUS_PAID;
            }
            else {
                order.Payment = payment;
                order.PaymentStatus = OrderStatus.STATUS_UNPAID;
            }
            order.UserId = (int)Session["UserId"];
            db.Orders.Add(order);
            await db.SaveChangesAsync();

            Response.StatusCode = (int)HttpStatusCode.OK;

            return Json("success", MediaTypeNames.Text.Plain);
        }

    }
}