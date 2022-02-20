using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(Order order);
        Task<bool> UpdateOrder(Order order);

        IList<Order> GetAllOrder();

        Task<Order> Find(Guid id);
    }
}