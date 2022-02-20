using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PcStoreDbContext db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryRepository));
        public CategoryRepository()
        {
            db = new PcStoreDbContext();
        }
        public async Task<bool> AddCategory(Category category)
        {
            try
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> DeleteCategory(Category category)
        {
            try
            {
                var item = db.Categories.Find(category.Id);
                db.Categories.Remove(item);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<Category> Find(int categoryId)
        {
            try
            {
                return await db.Set<Category>().FirstOrDefaultAsync(i => i.Id == categoryId);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<Category> FindByName(string name)
        {
            try
            {
                return await db.Set<Category>().FirstOrDefaultAsync(c => c.Name.ToUpper() == name.ToUpper());
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public IList<Category> GetAllCategories()
        {
            try
            {
                return db.Categories.OrderByDescending(c => c.CreateDate).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            try
            {
                db.Entry(category).State = EntityState.Modified;
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
