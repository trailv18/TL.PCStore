using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using TL.PCStore.Constants;
using TL.PCStore.Models;
using TL.PCStore.Repositories;
using TL.PCStore.Utilities;

namespace TL.PCStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IRoleRepository roleRepository;

        public AccountController()
        {
            userRepository = new UserRepository();
            userRoleRepository = new UserRoleRepository();
            roleRepository = new RoleRepository();
        }

        /// <summary>
        /// Get: login page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Post: login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(User model, string returnUrl)
        {
            var user = await userRepository.GetUserByEmail(model.Email);

            if (user != null)
            {
                var hashCode = user.PasswordSalt;
                var encodingPasswordString = PasswordUtilities.EncodePassword(model.Password, hashCode);
                var userLogin = await userRepository.GetUserByEmailAndPassword(model.Email, encodingPasswordString);

                var userRoles = (from u in userRepository.GetAllUsers()
                                 join roleMapping in userRoleRepository.GetAllUserRoles() on u.Id equals roleMapping.UserId
                                 join r in roleRepository.GetAllRoles() on roleMapping.RoleId equals r.Id
                                 where u.Email == model.Email
                                 select r.Name).ToList();

                if (userLogin != null)
                {
                    foreach (var item in userRoles)
                    {
                        if (item.Equals("Admin"))
                        {
                            Session["Role"] = item;
                        }
                    }
                    Session["UserName"] = user.Email;
                    Session["UserId"] = user.Id;
                    return RedirectToLocal(returnUrl);
                }
            }

            TempData["error-message"] = MessageConstants.USER_ERROR_LOGIN;
            return View();
        }

        /// <summary>
        /// Get: register
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Post: register
        /// </summary>
        /// <param name="model"></param>
        /// <param name="confPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(User model, string confPassword)
        {
            var user = await userRepository.GetUserByEmail(model.Email);
            bool valid = ValidateUser(model.FullName, model.Email, model.Password, confPassword);

            if (user != null)
            {
                TempData["error-message"] = MessageConstants.USER_DUPLICTE_EMAIL;
                return View();
            }
            else
            if (!valid)
            {
                return View();
            }

            var saltKey = PasswordUtilities.GeneratePassword(20);

            var userNew = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                IsActive = true,
                Password = PasswordUtilities.EncodePassword(model.Password, saltKey),
                PasswordSalt = saltKey,
                CreateDate = System.DateTime.Now
            };

            bool result = await userRepository.AddUser(userNew);
            if (result)
            {
                await userRoleRepository.AddUserRole(userNew.Id, RoleConstants.ROLE_USER);
                return RedirectToAction("login", "account");
            }
            else
            {
                TempData["error-message"] = MessageConstants.USER_ERROR_CREATE;
                return View();
            }

        }


        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            Session["UserName"] = null;
            Session["UserId"] = null;
            Session["Role"] = null;
            return RedirectToAction("index", "home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }

        /// <summary>
        /// validate user
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="confPassword"></param>
        /// <returns></returns>
        private bool ValidateUser(string name, string email, string password, string confPassword)
        {
            Regex emailRegex = new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");
            if (name.Length < 5 || name.Length > 255)
            {
                TempData["error-message"] = MessageConstants.USER_ERROR_NAME_LENTH;
                return false;
            }
            else if (!emailRegex.IsMatch(email))
            {
                TempData["error-message"] = MessageConstants.USER_ERROR_EMAIL;
                return false;
            }
            else if (password.Length < 6 || password.Length > 12)
            {
                TempData["error-message"] = MessageConstants.USER_ERROR_PASSWORD;
                return false;
            }
            else if (!password.Equals(confPassword))
            {
                TempData["error-message"] = MessageConstants.USER_PASSWORD_NOTMATCH;
                return false;
            }
            return true;
        }
    }
}