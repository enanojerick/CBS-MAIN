using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CBS.Dto.ViewModel;
using CBS.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;


namespace CBS.Main.Controllers
{
    public class LocationController : BaseController
    {
        private readonly IDepartmentService _DepartmentService;
        private readonly IUserService _UserService;
        private readonly IApplicationsService _ApplicationsService;
        private readonly RoleManager<IdentityRole> _RoleManager;
        public static IConfigurationRoot Configuration { get; set; }

        public LocationController(IDepartmentService DepartmentService, IUserService UserService, IApplicationsService ApplicationsService, RoleManager<IdentityRole> RoleManager)
        {
            _DepartmentService = DepartmentService;
            _UserService = UserService;
            _ApplicationsService = ApplicationsService;
            _RoleManager = RoleManager;
        }

        public async Task<ActionResult> Index()
        {
            var org = new DepartmentDto();
            IList<DepartmentDto> branch = new List<DepartmentDto>();
            IList<SelectListItem> branchDD = new List<SelectListItem>();
            try
            {
                var deptId = GetDepartmentID().Value;
                org = await _DepartmentService.GetDepartmentByID(deptId);
                branch = await _DepartmentService.GetDepartmentBranchByDeptID(deptId);
                branchDD = _DepartmentService.GetDepartmentBranchDDLByDeptID(deptId);

            }
            catch (Exception ex)
            {

            }
            // roles
            ViewBag.superAdmin = SuperAdmin;
            ViewBag.admin = Admin;

            ViewBag.branch = branch;
            ViewBag.branchDD = branchDD;
            ViewBag.roles = _RoleManager.Roles.Select(s => new SelectListItem { Value = s.Name, Text = s.Name }).ToList();
            return View(org);
        }

        [HttpGet]
        public async Task<ActionResult> AddBranch(int deptid, int? r, string origin = null)
        {
            //if (!CheckAccess("Organization_AddBranch")) return Redirect(AccessDeniedUrl);

            var dept = await _DepartmentService.GetDepartmentByID(deptid);

            ViewBag.Origin = r;
            ViewBag.Tab = string.IsNullOrEmpty(origin) ? "orgchart" : "branches";
            ViewBag.DeptId = deptid;

            DepartmentDto model = new DepartmentDto();
            model.ParentID = deptid;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddBranch(DepartmentDto dept, int? r, string origin = null)
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

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();

                var deptid = await _DepartmentService.AddDepartment(dept, GetLoggedInUserID().Value);
         
                return RedirectToAction("Index", controllerName, new { tab = origin });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", controllerName, new { tab = origin });
            }
        }

        public async Task<ActionResult> AddUpdate(DepartmentDto dept, string returnUrl = "")
        {
            try
            {

                if (dept.DepartmentID != null)
                {
                    await _DepartmentService.UpdateDepartmentAsync(dept, GetLoggedInUserID().Value);
                }
                else
                {
                    await _DepartmentService.AddDepartment(dept, GetLoggedInUserID().Value);
                }


                return returnUrl != "" ? RedirectToAction(returnUrl) : RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditBranch(int deptid, int? r, string origin = null)
        {
            //if (!CheckAccess("Organization_EditBranch")) return Redirect(AccessDeniedUrl);

            DepartmentDto model = new DepartmentDto();
            model = await _DepartmentService.GetDepartmentByID(deptid);
            model.Branches = await _DepartmentService.GetDepartmentBranchByDeptID(deptid);
            ViewBag.DeptID = model.ParentID;
            ViewBag.Origin = r;
            ViewBag.Tab = origin;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditBranch(DepartmentDto dept, IFormFile File, int? r, string origin = null)
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

                await _DepartmentService.UpdateDepartmentAsync(dept, GetLoggedInUserID().Value);

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();

                var deptid = await _DepartmentService.AddDepartment(dept, GetLoggedInUserID().Value);

                return RedirectToAction("Index", controllerName, new { tab = origin });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", controllerName, new { tab = origin });
            }
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
    }
}