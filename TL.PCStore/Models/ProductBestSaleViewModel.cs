using System.ComponentModel.DataAnnotations.Schema;

namespace TL.PCStore.Models
{
    [NotMapped]
    public class ProductBestSaleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Url { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int Qt { get; set; }
        public decimal? Discount { get; internal set; }
    }
}