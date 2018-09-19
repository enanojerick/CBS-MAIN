using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.IO;
using CBS.Data.Identity;
using CBS.Service.Interfaces;
using CBS.Data.Repository.Interface;
using CBS.Dto.ViewModel;

namespace CBS.Main.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _UserService;
        private readonly IApplicationsService _ApplicationsService;
        public static IConfigurationRoot Configuration { get; set; }
        private readonly IDepartmentService _DepartmentService;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public UserController(UserManager<ApplicationUser> userManager, IUserService UserService, IDepartmentService DepartmentService, IApplicationsService ApplicationsService, UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _UserService = UserService;
            _DepartmentService = DepartmentService;
            _ApplicationsService = ApplicationsService;
            _UserManager = UserManager;
            _RoleManager = roleManager;

            SetRolesStringName();
        }

        public ActionResult Index(int deptid)
        {
            var loggedUser = User.Identity.Name;
            var userDeptID = GetDepartmentID();
            var deptId = deptid != 0 ? deptid : userDeptID;

            var users = _UserService.GetUserProfileListByDepartmentId(deptId);

            ViewBag.Departments = _DepartmentService.GetDepartmentBranchDDLByDeptID(deptId.Value);
            ViewBag.orgId = deptId;
            ViewBag.users = users;

            return View();
        }

        public async Task<ActionResult> Edit(string identityID, int r)
        {
            var iusers = _UserService.GetProfileByIdentityID(identityID);
            var depId = GetDepartmentID().Value;
            var enumIApp = _ApplicationsService.GetApplicationAll();
            var iApp = enumIApp.AsEnumerable();
            var userApp = _ApplicationsService.GetUserAppsByUserID(iusers.UserID.Value);

            ViewBag.orgApp = iApp;

            var iuser = new UserDto()
            {
                IdentityID = iusers.IdentityID,
                Email = iusers.Email,
                FirstName = iusers.FirstName,
                LastName = iusers.LastName,
                Mobile = iusers.Mobile,
            };

            var userData = await _UserManager.FindByIdAsync(iusers.IdentityID);
            var userRole = await _UserManager.GetRolesAsync(userData);
            string selectedRole = userRole.Count > 0 ? userRole.First() : string.Empty;

            ViewBag.roles = _RoleManager.Roles.ToList().Select(m => new SelectListItem { Text = m.Name, Value = m.Name, Selected = selectedRole == m.Name ? true : false });

            ViewBag.userApp = userApp;
            ViewBag.returnUrl = null;

            if (r == 1)
            {
                ViewBag.returnUrl = "/Department/Index?tab=head-office-users";
            }
            else if (r == 2)
            {
                ViewBag.returnUrl = "/Location/Index?tab=location-users";
            }

            ViewBag.Origin = r;

            return View(iusers);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserDto user, string redirectUrl, string role)
        {
            var success = _UserService.UpdateUser(user, GetLoggedInUserID());

            ApplicationUser targetUser = await _UserManager.FindByEmailAsync(user.Email);
            var userOldRoles = await _UserManager.GetRolesAsync(targetUser);
            await _UserManager.RemoveFromRolesAsync(targetUser, userOldRoles.AsEnumerable());

            //foreach (var item in role)
            //{
            var result = await _UserManager.AddToRoleAsync(targetUser, role);
            //}

            if (success == "Success")
            {
                if (redirectUrl != null)
                {
                    return Redirect(redirectUrl);
                }
                return RedirectToAction("Index", "User");
            }
            else
            {
                return RedirectToAction("Edit", "User", user.IdentityID);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string identityID, int r)
        {
            try
            {
                var success = await _UserService.DeleteUser(identityID, GetLoggedInUserID());

                if (success == "Success")
                {
                    if (r == 1)
                    {
                        ViewBag.returnUrl = "/Organization/Index?tab=head-office-users";
                    }
                    else if (r == 2)
                    {
                        ViewBag.returnUrl = "/Organization/MyLocation?tab=location-users";
                    }

                    return RedirectToAction("Index", "Organization");
                }
                else
                {
                    return RedirectToAction("Index", "Organization");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Organization");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AssignApp(UsersAppsDto model)
        {
            var success = 0;

            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            success = await _ApplicationsService.AddUserApps(model, GetLoggedInUserID().Value);

            if (success > 0)
            {
                return Json(success);
            }
            else
            {
                return Json("failed");
            }
        }

        [HttpPost]
        public ActionResult RemoveUserApp(int? userappid, int? userid)
        {
            var success = _ApplicationsService.DeleteUsersApp(userappid, GetLoggedInUserID().Value);

            if (success == true)
            {
                return Json("success");
            }
            else
            {
                return Json("failed");
            }
        }

        public int? GetLoggedInUserID()
        {
            var loggedUser = User.Identity.Name;
            return _UserService.GetProfileByEmail(loggedUser).UserID.Value;
        }

        public int? GetDepartmentID()
        {
            var loggedUser = User.Identity.Name;
            return _UserService.GetProfileByEmail(loggedUser).DepartmentID.Value;
        }

        public async Task<ActionResult> GetAllUserPermissionView()
        {
            ViewBag.superAdmin = SuperAdmin;

            var userdetails = _UserService.GetProfileByEmail(User.Identity.Name);

            //Remove this when Extending is done
            var roles = _RoleManager.Roles.ToList();
            IList<RolesExtDto> roleext = new List<RolesExtDto>();
            foreach (var item in roles)
            {
                var iroleext = new RolesExtDto()
                {
                    RoleId = item.Id,
                    Role = item.Name
                };

                roleext.Add(iroleext);
            }

            //Remove this when Extending is done
            ViewBag.rolesext = roleext;

            var users = _UserService.GetUserProfileListByDepartmentId(userdetails.DepartmentID);

            foreach (var item in users)
            {
                ApplicationUser targetUser = await _userManager.FindByEmailAsync(item.Email);

                var userRoles = await _userManager.GetRolesAsync(targetUser);
                //item.Roles = userRoles.ToArray();
            }

            return View("_UserPermissionView", users);
        }

        public ActionResult GetUsersView(int orgid, int returnUrl)
        {
            var loggedUser = User.Identity.Name;
            var deptId = orgid != 0 ? orgid : GetDepartmentID();

            var users = _UserService.GetUserProfileListByDepartmentId(deptId);

            ViewBag.Organizations = _DepartmentService.GetDepartmentBranchDDLByDeptID(deptId.Value);
            ViewBag.DeptID = deptId;
            ViewBag.returnUrl = returnUrl;
            ViewBag.superAdmin = SuperAdmin;
            ViewBag.admin = Admin;

            return View("_UsersView", users);
        }
    }
}

