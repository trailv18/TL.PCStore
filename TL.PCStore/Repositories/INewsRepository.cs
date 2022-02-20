using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
   public interface INewsRepository
    {
        Task<News> Find(int id);
        Task<bool> AddNews(News news);
        Task<bool> UpdateNews(News news);
        Task<bool> DeleteNews(News news);
        IList<News> GetAllNews();
        Task<News> FindByUrl(string url);
    }
}
