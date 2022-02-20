using System.Collections.Generic;
using System.Threading.Tasks;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public interface IProductRepository
    {
        Task<Product> Find(int productId);
        Task<bool> AddProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(Product product);
        IList<Product> GetAllProducts();
        Task<Product> FindByName(string name);
        Task<Product> GetProductDetail(string url);
        Task<Product> GetProductByPicture(string picture);

    }
}
