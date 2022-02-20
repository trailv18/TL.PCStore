using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PcStoreDbContext db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(OrderRepository));

        public OrderRepository()
        {
            db = new PcStoreDbContext();
        }

        public async Task<bool> AddOrder(Order order)
        {
            try
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<Order> Find(Guid id)
        {
            try
            {
                return await db.Set<Order>().FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public IList<Order> GetAllOrder()
        {
            try
            {
                return db.Orders.OrderByDescending(b => b.DateOrder).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            try
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }
    }
}