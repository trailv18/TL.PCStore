using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL.PCStore.Models;

namespace TL.PCStore.Repositories
{
    public interface IBrandRepository
    {
        Task<Brand> Find(int brandId);
        Task<bool> AddBrand(Brand brand);
        Task<bool> UpdateBrand(Brand brand);
        Task<bool> DeleteBrand(Brand brand);
        IList<Brand> GetAllBrands();
        Task<Brand> FindByName(string name);
    }
}
