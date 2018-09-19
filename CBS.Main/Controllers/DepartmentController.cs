using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CBS.Dto.ViewModel;

using System.Net.Http;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Web;
using CBS.Service.Interfaces;

namespace CBS.Main.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _DepartmentService;
        private readonly IUserService _UserService;
        private readonly IApplicationsService _ApplicationsService;
        private readonly IPlatformService _PlatformService;
        private readonly RoleManager<IdentityRole> _RoleManager;
        public static IConfigurationRoot Configuration { get; set; }

        public DepartmentController(IDepartmentService DepartmentService, IUserService UserService, IApplicationsService ApplicationsService, IPlatformService PlatformService, RoleManager<IdentityRole> RoleManager)
        {
            _DepartmentService = DepartmentService;
            _UserService = UserService;
            _ApplicationsService = ApplicationsService;
            _PlatformService = PlatformService;
            _RoleManager = RoleManager;
            SetRolesStringName();
        }
        
        public async Task<ActionResult> Index()
        {
            if (!CheckAccess("Organization_Index")) return Redirect(AccessDeniedUrl);

            var maindept = new DepartmentDto();
            IList<DepartmentDto>  branch = new List<DepartmentDto>();
            try
            {
                var deptid = GetDepartmentID().Value;
                maindept =  await _DepartmentService.GetTopDepartment(deptid);
                branch = await _DepartmentService.GetDepartmentBranchByDeptID(maindept.DepartmentID.Value);

                ViewBag.DeptID = maindept.DepartmentID;
                ViewBag.Branch = branch;
                var branchDD = _DepartmentService.GetDepartmentBranchDDLByDeptID(deptid);

                // roles
                ViewBag.roles = _RoleManager.Roles.Select(s => new SelectListItem { Value = s.Name, Text = s.Name }).ToList();
                ViewBag.superAdmin = SuperAdmin;
                ViewBag.admin = Admin;
                ViewBag.branchDD = branchDD;

                if (deptid == maindept.DepartmentID)
                {
                    ViewBag.canModifyOrg = 1;
                }
                else
                {
                    ViewBag.canModifyOrg = 0;
                }
            }
            catch(Exception ex)
            {

            }
            return View("Index", maindept);
        }

        [HttpGet]
        public async Task<ActionResult> AddBranch(int deptid, int? r, string origin = null)
        {
            //if (!CheckAccess("Organization_AddBranch")) return Redirect(AccessDeniedUrl);

            ViewBag.Origin = r;
            ViewBag.Tab = string.IsNullOrEmpty(origin) ? "orgchart" : "branches";
            ViewBag.DeptID = deptid;

            DepartmentDto model = new DepartmentDto();
            model.ParentID = deptid;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddBranch(DepartmentDto dept, IFormFile File, int? r, string origin = null)
        {
            //if (!CheckAccess("Organization_AddBranch")) return Redirect(AccessDeniedUrl);
            string controllerName = string.Empty;

            try
            {
                if (r == 1)
                {
                    controllerName = "Department";
                }
                else
                {
                    controllerName = "Location";
                }

                var orggroupid = await _DepartmentService.GetDepartmentByID(dept.ParentID.Value);

                var orgid = await _DepartmentService.AddDepartment(dept, GetLoggedInUserID().Value);
                
                return RedirectToAction("Index", controllerName, new { tab = origin });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", controllerName, new { tab = origin });
            }
        }

      
        [HttpGet]
        public async Task<ActionResult> EditBranch(int deptid, int? r, string origin = null)
        {
            //if (!CheckAccess("Organization_EditBranch")) return Redirect(AccessDeniedUrl);

            ViewBag.Origin = r;
            ViewBag.Tab = string.IsNullOrEmpty(origin) ? "orgchart" : "branches";

            DepartmentDto model = new DepartmentDto();
            model = await _DepartmentService.GetDepartmentByID(deptid);
            model.Branches = await _DepartmentService.GetDepartmentBranchByDeptID(deptid);
            ViewBag.OrgID = model.ParentID;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditBranch(DepartmentDto dept, int? r, string origin = null)
        {
            //if (!CheckAccess("Organization_EditBranch")) return Redirect(AccessDeniedUrl);
            string controllerName = string.Empty;

            try
            {
                if (r == 1)
                {
                    controllerName = "Department";
                }
                else
                {
                    controllerName = "Location";
                }

                var orgid = await _DepartmentService.UpdateDepartmentAsync(dept, GetLoggedInUserID().Value);

                return RedirectToAction("Index", controllerName, new { tab = origin });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", controllerName, new { tab = origin });
            }
        }

        [HttpPost]
        public JsonResult EditBranchParent(DepartmentDto dept)
        {
            try
            {
                var result = _DepartmentService.UpdateDepartmentParent(dept, GetLoggedInUserID().Value);

                return Json("true");
            }
            catch (Exception ex)
            {
                return Json("false"); 
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddUpdate(DepartmentDto dept, string returnUrl = "")
        {
            try
            {

                var orgid = 0;
                var actionStr = "Add";
                if (dept.DepartmentID != null)
                {
                    orgid = await _DepartmentService.UpdateDepartmentAsync(dept, GetLoggedInUserID().Value);
                    actionStr = "Edit";
                }
                else
                {
                    orgid = await _DepartmentService.AddDepartment(dept, GetLoggedInUserID().Value);
                }

                return returnUrl != "" ? RedirectToAction(returnUrl)  : RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }   
        }

        public async Task<ActionResult> BranchList(int deptid)
        {
            IList<DepartmentDto> branches = new List<DepartmentDto>();
            try
            {

                branches = await _DepartmentService.GetDepartmentBranchByDeptID(deptid);

                ViewBag.DeptID = deptid;

                return View(branches);
            }
            catch (Exception)
            {
                return View(branches);
            }

        }

        public async Task<ActionResult> AssignApp()
        {
            if (!CheckAccess("Organization_AssignApp")) return Redirect(AccessDeniedUrl);

            var deptid = GetDepartmentID().Value;
            var enumIApp = _ApplicationsService.GetApplicationAll();
            var iApp = enumIApp.AsEnumerable();
            IList<SelectListItem> orgAppList = new List<SelectListItem>();

            foreach (var item in iApp)
            {
                var data = new SelectListItem()
                {
                    Text = item.ClientName,
                    Value = item.AppID.ToString(),
                };
                orgAppList.Add(data);
            }


            var iusers = _UserService.GetUserProfileListByDepartmentId(GetDepartmentID().Value);
            IList<SelectListItem> userList = new List<SelectListItem>();
            foreach (var item in iusers)
            {
                var data = new SelectListItem()
                {
                    Text = item.FirstName + " " + item.LastName,
                    Value = item.UserID.ToString(),
                };
                userList.Add(data);
            }

            ViewBag.users = userList;
            ViewBag.iApp = iApp;

            UsersAppsDto usersapp = new UsersAppsDto();
            return View(usersapp);
        }

        [HttpPost]
        public async Task<ActionResult> AssignApp(UsersAppsDto model)
        {
            //if (!CheckAccess("Organization_AssignApp")) return Redirect(AccessDeniedUrl);

            var success = 0;

            success = await _ApplicationsService.AddUserApps(model, GetLoggedInUserID().Value);


            if (success > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("AssignApp", "Organization");
            }
        }

        public ActionResult Chart()
        {
            if (!CheckAccess("Organization_Chart")) return Redirect(AccessDeniedUrl);
            return View();
        }

        public async Task<ActionResult> MyLocation()
        {
            if (!CheckAccess("Organization_MyLocation")) return Redirect(AccessDeniedUrl);

            var dept = new DepartmentDto();
            IList<SelectListItem> deptAndbranch = new List<SelectListItem>();
            var maindept = new DepartmentDto();
            try
            {
                var deptid = GetDepartmentID().Value;
                dept = await _DepartmentService.GetDepartmentByID(GetDepartmentID().Value);
                deptAndbranch = _DepartmentService.GetDepartmentBranchDDLByDeptID(deptid);

                ViewBag.branchUser = await _UserService.GetUserByDepartmentBranch(deptid);
                
            }
            catch (Exception ex)
            {

            }

            ViewBag.TopOrgId = maindept.DepartmentID;
            ViewBag.branch = deptAndbranch;
            return View(dept);
        }


        public async Task<JsonResult> GetChart(int deptid)
        {
            var dept = new DepartmentDto();
            IList<DepartmentDto> deptAndbranch = new List<DepartmentDto>();
            var id = deptid != 0 ? deptid : GetDepartmentID().Value;
            dept = await _DepartmentService.GetDepartmentByID(id);
            deptAndbranch = await _DepartmentService.GetDepartmentBranchByDeptID(id);

            return Json(new { parent = dept, branch = deptAndbranch });
        }

        public JsonResult GetOrganization()
        {
            var orgId = GetDepartmentID();
            if(orgId != null || orgId != 0)
            {
                var organization = _DepartmentService.GetDepartmentByID(GetDepartmentID().Value);

                return Json(organization);
            }

            return Json(null);
        }

        public async Task<JsonResult> GetDepartmentPlatform()
        {
            var DeptPlatforms = await _PlatformService.GetDepartmentPlatformByDepID(GetDepartmentID().Value);

            return Json(DeptPlatforms);
        }

        public int? GetLoggedInUserID()
        {
            var loggedUser = User.Identity.Name;
            return _UserService.GetProfileByEmail(loggedUser).UserID;
        }

        public int? GetDepartmentID()
        {
            var loggedUser = User.Identity.Name;
            return _UserService.GetProfileByEmail(loggedUser).DepartmentID;
        }

        public class HQModel
        {
            public int HQID { get; set; }
            public string Name { get; set; }
            public int Parentid { get; set; }
        }
    }
}
