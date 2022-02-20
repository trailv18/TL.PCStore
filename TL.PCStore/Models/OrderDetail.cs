using System;
using System.ComponentModel.DataAnnotations;

namespace TL.PCStore.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}