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
    public class NewsRepository : INewsRepository
    {
        private readonly PcStoreDbContext db;
        private static readonly ILog Log = LogManager.GetLogger(typeof(NewsRepository));

        public NewsRepository()
        {
            db = new PcStoreDbContext();
        }
        public async Task<bool> AddNews(News news)
        {
            try
            {
                db.News.Add(news);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> DeleteNews(News news)
        {
            try
            {
                var item = db.News.Find(news.Id);
                db.News.Remove(item);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<News> Find(int id)
        {
            try
            {
                return await db.Set<News>().Include(n => n.User).FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public  async Task<News> FindByUrl(string url)
        {
            try
            {
                return await db.Set<News>().Include(n => n.User).FirstOrDefaultAsync(i => i.Url == url);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public IList<News> GetAllNews()
        {
            try
            {
                return db.News.Include(n => n.User).OrderByDescending(n => n.PostedOn).ToList();

            }
            catch (Exception ex)
            {
                Log.Error("Error", ex);
                throw;
            }
        }

        public async Task<bool> UpdateNews(News news)
        {
            try
            {
                db.Entry(news).State = EntityState.Modified;
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