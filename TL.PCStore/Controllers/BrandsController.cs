using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TL.PCStore.Repositories;

namespace TL.PCStore.Web.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IBrandRepository brandRepository;

        public BrandsController()
        {
            brandRepository = new BrandRepository();
        }
        // GET: Category name
        public ActionResult GetNameBrand()
        {
            var brands = brandRepository.GetAllBrands().Where(b=>b.DeleteFlag == false);
            if (brands == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Brand", brands);
        }
    }
}