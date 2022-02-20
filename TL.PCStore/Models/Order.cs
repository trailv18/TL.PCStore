using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TL.PCStore.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string Payment { get; set; }
        public DateTime DateOrder { get; set; }
        public DateTime? DateDelivery { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}