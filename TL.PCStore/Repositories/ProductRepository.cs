using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PcStoreDbContext db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProductRepository));
        public ProductRepository()
        {
            db = new PcStoreDbContext();
        }

        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            try
            {
                var item = db.Products.Find(product.Id);
                db.Products.Remove(item);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }

        }

        public async Task<Product> Find(int productId)
        {
            try
            {
                return await db.Set<Product>().FirstOrDefaultAsync(i => i.Id == productId);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<Product> FindByName(string name)
        {
            try
            {
                return await db.Set<Product>().FirstOrDefaultAsync(p => p.Name.ToUpper() == name.ToUpper());
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public IList<Product> GetAllProducts()
        {
            try
            {
                return db.Products.Include(p => p.Category).Include(p => p.Brand).OrderByDescending(p => p.CreateDate).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<Product> GetProductByPicture(string picture)
        {
            try
            {
                return await db.Set<Product>().Include(p => p.Category).Include(p => p.Brand).FirstOrDefaultAsync(p => p.Picture == picture);

            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<Product> GetProductDetail(string url)
        {
            try
            {
                return await db.Set<Product>().Include(p => p.Category).Include(p => p.Brand).FirstOrDefaultAsync(p => p.Url == url);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            try
            {
                db.Entry(product).State = EntityState.Modified;
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
