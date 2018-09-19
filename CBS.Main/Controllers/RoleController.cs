using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using CBS.Data.Identity;
using CBS.Dto.ViewModel;
using CBS.Service.Interfaces;


namespace CBS.Main.Controllers
{
    public class RoleController : BaseController
    {
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IUserService _UserService;

        public RoleController(RoleManager<IdentityRole> RoleManager, IUserService UserService, UserManager<ApplicationUser> UserManager)
        {
            _RoleManager = RoleManager;
            _UserService = UserService;
            _UserManager = UserManager;
            SetRolesStringName();
        }

        public ActionResult Index()
        {
            if (!CheckAccess("Role_Index")) return Redirect(AccessDeniedUrl);

            var roles = _RoleManager.Roles.ToList();
            return View(roles);
        }

        public ActionResult Add()
        {
            if (!CheckAccess("Role_Add")) return Redirect(AccessDeniedUrl);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(string role)
        {
            if (!await _RoleManager.RoleExistsAsync(role))
            {
                var result = await _RoleManager.CreateAsync(new IdentityRole(role));

                if (result.Succeeded)
                {

                }
                else
                {

                }
            }
            return View();
        }

        public ActionResult Edit(string roleid)
        {
            if (!CheckAccess("Role_Edit")) return Redirect(AccessDeniedUrl);

            var role = _RoleManager.Roles.Where(m => m.Id == roleid).FirstOrDefault();
            return View(role);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(IdentityRole role)
        {
            IdentityRole theRole = await _RoleManager.FindByIdAsync(role.Id);
            theRole.Name = role.Name;

            var result = await _RoleManager.UpdateAsync(theRole);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public async Task<ActionResult> AssignRole(string identityid)
        {
            if (!CheckAccess("Role_AssignRole")) return Redirect(AccessDeniedUrl);

            var userData = await _UserManager.FindByIdAsync(identityid);

            var userRole = await _UserManager.GetRolesAsync(userData);

            var Roles = _RoleManager.Roles.ToList();

            ViewBag.userRoles = userRole;
            ViewBag.roles = Roles;
            return View(userData);
        }

        [HttpPost]
        public async Task<ActionResult> AssignRole(ApplicationUser user, string[] role)
        {
            ApplicationUser targetUser = await _UserManager.FindByEmailAsync(user.UserName);

            var userOldRoles = await _UserManager.GetRolesAsync(targetUser);

            await _UserManager.RemoveFromRolesAsync(targetUser, userOldRoles.ToArray());

            foreach (var item in role)
            {
                var result = await _UserManager.AddToRoleAsync(targetUser, item);
            }
          
           
            return RedirectToAction("Index", "Organization", new { tab = "user-permission" });
        }

        public async Task<ActionResult> AssignUsersToRole(string identityid, int orgid)
        {
            if (!CheckAccess("Role_AssignRole")) return Redirect(AccessDeniedUrl);

            var admindetails = _UserService.GetProfileByEmail(User.Identity.Name);

            var users = _UserService.GetUserProfileListByDepartmentId(admindetails.DepartmentID);
                
                //_UserManager.Users.ToList();

            List<UserRoleDto> userr = new List<UserRoleDto>();

            foreach (var iuser in users)
            {
                ApplicationUser itargetuser = await _UserManager.FindByEmailAsync(iuser.Email);
                var targetUserRole = await _UserManager.GetRolesAsync(itargetuser);
                userr.Add(new UserRoleDto { user = iuser.Email, role = targetUserRole.FirstOrDefault(), selected = targetUserRole.FirstOrDefault() == identityid ? true : false, selectedrole = identityid });
            }

            ViewBag.userList = userr.Select(m => new UserRoleDto { user = m.user, role = m.role, selectedrole = m.selectedrole, selected = m.selected }).Where(m => m.selected = m.selected == false);
            ViewBag.userInRoleList = userr.Select(m => new UserRoleDto { user = m.user, role = m.role, selectedrole = m.selectedrole, selected = m.selected }).Where(m => m.selected = m.selected == true);
            ViewBag.superAdmin = SuperAdmin;
            ViewBag.Role = identityid;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AssignUsersToRole(string Userid, string role)
        {
            ApplicationUser targetUser = await _UserManager.FindByEmailAsync(Userid);

            bool userRoleExists = false;

            var userOldRoles = await _UserManager.GetRolesAsync(targetUser);

            if (userOldRoles != null)
            {
                if (userOldRoles.Count > 0)
                {
                    if (userOldRoles.Contains(role))
                    {
                        userRoleExists = true;
                    }
                }
            }

            var result = await _UserManager.RemoveFromRolesAsync(targetUser, userOldRoles.ToArray());

            if (!userRoleExists) result = await _UserManager.AddToRoleAsync(targetUser, role);

            if (result.Succeeded)
            {
                return Json(Userid);
            }
            else
            {
                return Json("failed");
            }
        }
    }
}