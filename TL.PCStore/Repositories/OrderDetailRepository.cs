using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly PcStoreDbContext db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(OrderDetailRepository));

        public OrderDetailRepository()
        {
            db = new PcStoreDbContext();
        }

        public async Task<bool> AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                db.OrderDetails.Add(orderDetail);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public IList<OrderDetail> GetAlls()
        {
            try
            {
                return db.OrderDetails.Include("Product").Include("Order").OrderByDescending(od => od.Order.DateOrder).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }
    }
}