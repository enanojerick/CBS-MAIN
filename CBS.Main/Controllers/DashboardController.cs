using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

using CBS.Data.Identity;
using CBS.Dto.ViewModel;
using CBS.Service.Interfaces;

namespace CBS.Main.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _UserService;
        private readonly IDepartmentService _DepartmentService;
        private readonly IPlatformService _PlatformService;
        private readonly IApplicationsService _ApplicationsService;
        public static IConfigurationRoot Configuration { get; set; }
        private readonly SignInManager<ApplicationUser> _signInManager;

        public DashboardController(UserManager<ApplicationUser> userManager, 
                                   IUserService UserService,
                                   IDepartmentService DepartmentService,
                                   IPlatformService PlatformService,
                                   IApplicationsService ApplicationsService,
                                   SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _UserService = UserService;
            _DepartmentService = DepartmentService;
            _PlatformService = PlatformService;
            _ApplicationsService = ApplicationsService;
            _signInManager = signInManager;

            SetRolesStringName();
        }


        public async Task<ActionResult> Index()
        {
            var email = User.Identity.Name;
            var loggeduser = _UserService.GetProfileByEmailOAuth(email);

            var platform = await _PlatformService.GetUserPlatform(loggeduser.UserID.Value);

            var userapps = _ApplicationsService.GetUserAppsByUserID(loggeduser.UserID.Value);

            var deptplatforms = await _PlatformService.GetDepartmentPlatformByDepID(loggeduser.DepartmentID.Value);

            ViewBag.UserApps = userapps;
            ViewBag.DeptPlatforms = deptplatforms;
            ViewBag.platform = platform;

            return View();
        }

        public async Task<ActionResult> AddPlatform(UserPlatformDto platform, IFormFile File)
        {
            var email = User.Identity.Name;
            var user = _UserService.GetProfileByEmail(email);

            platform.UserId = user.UserID;

            var result = await _PlatformService.AddUserPlatform(platform, 1);
            if (result > 0)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }


        public async Task<ActionResult> EditPlatform(UserPlatformDto platform, IFormFile File)
        {
            var email = User.Identity.Name;
            var user = _UserService.GetProfileByEmail(email);

            platform.UserId = user.UserID;

            var result = await _PlatformService.UpdateUserPlatform(platform, user.UserID.Value);
            if (result == "Success")
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<ActionResult> DeletePlatform(int platformid)
        {
            try
            {
                var email = User.Identity.Name;
                var user = _UserService.GetProfileByEmail(email);

                var success = await _PlatformService.DeleteUserPlatform(platformid, user.UserID.Value);

                if (success == "Success")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        #region Organisation Platform
        public async Task<ActionResult> AddDeploymentPlatform(DepartmentPlatformsDto deptplatform, IFormFile File, string redirectUrl = "")
        {
            var email = User.Identity.Name;
            var user = _UserService.GetProfileByEmail(email);

            deptplatform.DepartmentID = user.DepartmentID;

            var result = await _PlatformService.AddDepartmentPlatform(deptplatform, user.UserID.Value);
            if (result > 0)
            {
                if (redirectUrl != "")
                {
                    return Redirect(redirectUrl);
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        public async Task<ActionResult> EditDeploymentPlatform(DepartmentPlatformsDto deptplatform, IFormFile File, string redirectUrl = "")
        {
            var email = User.Identity.Name;
            var user = _UserService.GetProfileByEmail(email);

            deptplatform.DepartmentID = user.DepartmentID;

            var result = await _PlatformService.UpdateDepartmentPlatform(deptplatform, user.UserID.Value);
            if (result == "Success")
            {
                if(redirectUrl != "")
                {
                    return Redirect(redirectUrl);
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<ActionResult> DeleteDepartmentPlatform(int deptplatformid, string redirectUrl = "")
        {
            try
            {
                var email = User.Identity.Name;
                var user = _UserService.GetProfileByEmail(email);

                var success = await _PlatformService.DeleteDepartmentPlatform(deptplatformid, user.UserID.Value);

                if (success == "Success")
                {
                    if (redirectUrl != "")
                    {
                        return Redirect(redirectUrl);
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        #endregion


        public ActionResult SavePlatformSort(string data)
        {
            var email = User.Identity.Name;
            var user = _UserService.GetProfileByEmail(email);

            var userId = user.UserID;

            var update = _UserService.UpdateUserPlatformSorting(data, userId);
            return new JsonResult(data);
        }

        public ActionResult GetPlatformSort()
        {
            var email = User.Identity.Name;
            var user = _UserService.GetProfileByEmail(email);

            var userId = user.UserID;
            var data = _UserService.GetUserPlatformSorting(userId);
            return new JsonResult(data);
        }

        [Route("/AccessDenied")]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/Eula")]
        public ActionResult Eula()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/PrivacyPolicy")]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}

