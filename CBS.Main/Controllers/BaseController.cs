using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CBS.Main.Controllers
{
    public abstract partial class BaseController : Controller
    {
        public string SuperAdmin;
        public string Admin;
        public string Member;
        public string AccessDeniedUrl = "/AccessDenied"; 

        public BaseController()
        {
            SetRolesStringName();
        }

        public void SetRolesStringName()
        {
            SuperAdmin = "Super Administrator";
            Admin = "Administrator";
            Member = "Member";
        }

        public bool CheckAccess(string gate)
        {
            // Authentication for Roles
            if (gate == "Role_Index")
            {
                return User.IsInRole(SuperAdmin);
            }

            if (gate == "Role_Edit")
            {
                return User.IsInRole(SuperAdmin);
            }

            if (gate == "Role_Add")
            {
                return User.IsInRole(SuperAdmin);
            }

            if (gate == "Role_AssignRole")
            {
                return User.IsInRole(SuperAdmin);
            }


            // Authentication for Organization
            if (gate == "Organization_Index")
            {
                return User.IsInRole(SuperAdmin) || User.IsInRole(Admin);
            }

            if (gate == "Organization_AddBranch")
            {
                return User.IsInRole(SuperAdmin) || User.IsInRole(Admin);
            }

            if (gate == "Organization_EditBranch")
            {
                return User.IsInRole(SuperAdmin) || User.IsInRole(Admin);
            }

            if (gate == "Organization_EditBranchParent")
            {
                return User.IsInRole(SuperAdmin) || User.IsInRole(Admin);
            }

            if (gate == "Organization_AssignApp")
            {
                return User.IsInRole(SuperAdmin) || User.IsInRole(Admin);
            }

            if (gate == "Organization_MyLocation")
            {
                return User.IsInRole(SuperAdmin) || User.IsInRole(Admin);
            }

            // Authentication for Dashboard
            if (gate == "Dashboard_Index")
            {
                return User.IsInRole(SuperAdmin) || User.IsInRole(Admin) || User.IsInRole(Member);
            }

            return false;
        }

        public void RoleAccess()
        {
            

        }
    }
}