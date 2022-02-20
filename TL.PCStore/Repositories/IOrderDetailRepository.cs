using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<bool> AddOrderDetail(OrderDetail orderDetail);

        IList<OrderDetail> GetAlls();
    }
}