using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly PcStoreDbContext db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(BrandRepository));

        public BrandRepository()
        {
            db = new PcStoreDbContext();
        }
        public async Task<bool> AddBrand(Brand brand)
        {
            try
            {
                db.Brands.Add(brand);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> DeleteBrand(Brand brand)
        {
            try
            {
                var item = db.Brands.Find(brand.Id);
                db.Brands.Remove(item);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<Brand> Find(int brandId)
        {
            try
            {
                return await db.Set<Brand>().FirstOrDefaultAsync(i => i.Id == brandId);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<Brand> FindByName(string name)
        {
            try
            {
                return await db.Set<Brand>().FirstOrDefaultAsync(i => i.Name == name);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public IList<Brand> GetAllBrands()
        {
            try
            {
                return db.Brands.OrderByDescending(b => b.CreateDate).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> UpdateBrand(Brand brand)
        {
            try
            {
                db.Entry(brand).State = EntityState.Modified;
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
