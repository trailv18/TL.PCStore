using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TL.PCStore.Constants;
using TL.PCStore.Models;
using TL.PCStore.Repositories;

namespace TL.PCStore.Controllers.Admin
{
    public class RoleManagerController : Controller
    {
        private readonly IRoleRepository roleRepository;
        private readonly IUserRoleRepository userRoleRepository;

        public RoleManagerController()
        {
            roleRepository = new RoleRepository();
            userRoleRepository = new UserRoleRepository();
        }
        // GET: Admin/Role
        public ActionResult Index()
        {
            var roles = roleRepository.GetAllRoles();
            return View(roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Role role)
        {
            if (role == null)
            {
                return HttpNotFound();
            }

            var roleByName = await roleRepository.GetRoleByName(role.Name);
            bool valid = ValidateRole(role.Name);
            if (!valid)
            {
                return View();
            }
            else
            if (roleByName != null)
            {
                TempData["error-message"] = MessageConstants.ROLE_DUPLICATE_NAME;
                return View();
            }

            role.CreateDate = DateTime.Now;
            role.DeleteFlag = false;

            bool result = await roleRepository.AddRole(role);
            if (result)
            {
                TempData["message"] = MessageConstants.ROLE_SUCCESS_CREATE;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error-message"] = MessageConstants.ROLE_ERROR_CREATE;
                return View();
            }
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            int roleId = (int)id;
            var role = await roleRepository.Find(roleId);

            if (role == null)
            {
                return HttpNotFound();
            }

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Role role)
        {
            if (role == null)
            {
                return HttpNotFound();
            }

            bool valid = ValidateRole(role.Name);
            if (!valid)
            {
                return View();
            }

            role.UpdateDate = DateTime.Now;
            bool result = await roleRepository.UpdateRole(role);

            if (result)
            {
                TempData["message"] = MessageConstants.ROLE_SUCCESS_UPDATE;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error-message"] = MessageConstants.ROLE_ERROR_UPDATE;
                return View();
            }
        }

        public async Task<ActionResult> DeleteRole(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }

            var countRoleInUserRole = userRoleRepository.GetAllUserRoles().Where(r => r.RoleId == id).Count();
            if (countRoleInUserRole > 0)
            {
                TempData["error-message"] = MessageConstants.ROLE_INVALID_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }

            int roleId = (int)id;
            var role = await roleRepository.Find(roleId);
            if (role == null)
            {
                return HttpNotFound();
            }

            bool result = await roleRepository.DeleteRole(role);
            if (result)
            {
                TempData["message"] = MessageConstants.ROLE_SUCCESS_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                TempData["error-message"] = MessageConstants.ROLE_ERROR_DELETE;
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        private bool ValidateRole(string name)
        {
            if (name.Length < 2)
            {
                TempData["error-message"] = MessageConstants.ROLE_ERROR_NAME_LENTH;
                return false;
            }
            return true;
        }
    }
}