using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

using CBS.Common.Interface;
using CBS.Data.Identity;
using CBS.Service.Interfaces;
using CBS.Data.Repository.Interface;
using CBS.Data.Entities;
using CBS.Dto.ViewModel;

namespace CBS.Main.Services
{
    public class DepartmentService : IDepartmentService
    {
        private IRepository<CBS_Department> _Department;
        private IRepository<CBS_Users> _Users;
        private IOptions<AzureContainerSettings> _AzureContainerSettings;
        //private IAzureBlobStorage _AzureBlobStorage;
        private readonly UserManager<ApplicationUser> _UserManager;

        public DepartmentService(IRepository<CBS_Department> Department,
            IRepository<CBS_Users> Users,
            IOptions<AzureContainerSettings> AzureContainerSettings,
           // IAzureBlobStorage AzureBlobStorage,
            UserManager<ApplicationUser> UserManager)
        {
            _Department = Department;
            _Users = Users;
            _AzureContainerSettings = AzureContainerSettings;
          //  _AzureBlobStorage = AzureBlobStorage;
            _UserManager = UserManager;
            _Users = Users;
        }

        #region DTO

        private DepartmentDto SetDepartmentDto(CBS_Department dep)
        {
            var idep = new DepartmentDto()
            {
                DepartmentID = dep.DepartmentID,
                DepartmentName = dep.DepartmentName,
                PhoneNumber = dep.PhoneNumber,
                PhoneNumber2 = dep.PhoneNumber2,
                IsActive = dep.IsActive,
                CreatedBy = dep.CreatedBy,
                CreatedDate = dep.CreatedDate,
                ModifiedBy = dep.ModifiedBy,
                ModifiedDate = dep.ModifiedDate,
                FilePath = dep.FilePath,
                ParentID = dep.ParentID,

            };

            return idep;
        }

        private List<DepartmentDto> SetDepartmentDtoList(List<CBS_Department> dep)
        {
            List<DepartmentDto> deplist = new List<DepartmentDto>();

            foreach (var item in dep)
            {
                var idep = new DepartmentDto()
                {
                    DepartmentID = item.DepartmentID,
                    DepartmentName = item.DepartmentName,
                    PhoneNumber = item.PhoneNumber,
                    PhoneNumber2 = item.PhoneNumber2,
                    ParentID = item.ParentID,
                    IsActive = item.IsActive,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,

                    FilePath = item.FilePath
                };

                deplist.Add(idep);

            }

            return deplist;
        }
        #endregion

        #region Get
        public async Task<DepartmentDto> GetDepartmentByID(int depid)
        {
            var idep = (from u in _Department.GetAll()
                        where u.IsActive == true
                        && u.DepartmentID == depid
                        select u).FirstOrDefault();

            var dep = SetDepartmentDto(idep);

            return dep;
        }

        public async Task<IList<DepartmentDto>> GetDepartmentBranchByDeptID(int depid)
        {
            var idep = (from u in _Department.GetAll()
                        where u.IsActive == true
                        && u.ParentID == depid
                        select u).ToList();

            var dep = SetDepartmentDtoList(idep);

            foreach (var item in dep)
            {
                var users = _Users.SearchFor(m => m.IsActive == true && m.DepartmentID == item.DepartmentID).ToList();


                item.Branches = await GetDepartmentBranchByDeptID(item.DepartmentID.Value);
            }
            return dep;
        }

        public IList<DepartmentDto> GetDepartmentBranchByOrgIDList(int depid)
        {

            var idep = (from u in _Department.GetAll()
                        where u.IsActive == true
                        && u.ParentID == depid
                        select u).ToList();

            var dep = SetDepartmentDtoList(idep);

            foreach (var item in idep)
            {
                var innerDep = GetDepartmentBranchByOrgIDList(item.DepartmentID.Value).ToList();

                dep.AddRange(innerDep);
            }

            return dep;
        }

        public IList<DepartmentDto> GetDepartmentBranchSingleListByDeptID(int depid)
        {
            var idep = (from u in _Department.GetAll()
                        where u.IsActive == true
                        && u.ParentID == depid
                        select u);

            var dep = SetDepartmentDtoList(idep.ToList());

            foreach (var item in idep)
            {
                var blist = GetDepartmentBranchSingleListByDeptID(item.DepartmentID.Value);

                foreach (var item1 in blist)
                {
                    dep.Add(item1);
                }


            }
            return dep;
        }

        public IList<SelectListItem> GetDepartmentBranchDDLByDeptID(int depid)
        { 
            IList<DepartmentDto> deplist = new List<DepartmentDto>();

            var idep = (from u in _Department.GetAll()
                        where u.IsActive == true
                        && u.DepartmentID == depid
                        select u).FirstOrDefault();

            var dep = SetDepartmentDto(idep);

            deplist.Add(dep);

            var bdeplist = GetDepartmentBranchSingleListByDeptID(dep.DepartmentID.Value);

            foreach (var item in bdeplist)
            {
                deplist.Add(item);
            }

            var finallist = (from c in deplist
                             select new SelectListItem
                             {
                                 Text = c.DepartmentName,
                                 Value = c.DepartmentID.ToString()
                             }).ToList();

            return finallist;
        }

        public IList<DepartmentDto> GetActiveDepartmentList()
        {
            var idep = (from u in _Department.GetAll()
                        where u.IsActive == true
                        select u).ToList();

            var dep = SetDepartmentDtoList(idep);

            return dep;
        }

        #endregion

        #region Add, Edit, Delete
        public async Task<int> AddDepartment(DepartmentDto dep, int userid)
        {
            try
            {
                var idep = new CBS_Department()
                {
                    DepartmentName = dep.DepartmentName,
                    PhoneNumber = (dep.PhoneNumber.StartsWith("0")) ? dep.PhoneNumber.Substring(1) : dep.PhoneNumber,
                    PhoneNumber2 = string.IsNullOrEmpty(dep.PhoneNumber2) == false ? ((dep.PhoneNumber2.StartsWith("0")) ? dep.PhoneNumber2.Substring(1) : dep.PhoneNumber2) : null,
                    ParentID = dep.ParentID,
                    IsActive = true,
                    CreatedBy = userid,
                    CreatedDate = DateTime.Now
                };


                return _Department.Insert(idep).DepartmentID.Value;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateDepartmentAsync(DepartmentDto dep, int userid)
        {
            try
            {
                var idep = _Department.SearchFor(m => m.DepartmentID == dep.DepartmentID).FirstOrDefault();

                idep.DepartmentName = dep.DepartmentName;
                idep.PhoneNumber = (dep.PhoneNumber.StartsWith("0")) ? dep.PhoneNumber.Substring(1) : dep.PhoneNumber;
                idep.PhoneNumber2 = string.IsNullOrEmpty(dep.PhoneNumber2) == false ? ((dep.PhoneNumber2.StartsWith("0")) ? dep.PhoneNumber2.Substring(1) : dep.PhoneNumber2) : null;
                idep.ModifiedBy = userid;
                idep.ModifiedDate = DateTime.Now;

                var id = _Department.Update(idep, m => m.DepartmentID == dep.DepartmentID).DepartmentID;

                return id != null || id > 0 ? id.Value : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string UpdateDepartmentParent(DepartmentDto dep, int userid)
        {
            try
            {
                var idep = new CBS_Department()
                {
                    ParentID = dep.ParentID,
                    ModifiedBy = userid,
                    ModifiedDate = DateTime.Now
                };

                var id = _Department.UpdateSpecificValuesNotNull(idep, m => m.DepartmentID  == dep.DepartmentID).DepartmentID;

                return id != null || id > 0 ? "Success" : "Error on updating";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }

        public string DeleteDepartment(int depid, int userid)
        {
            try
            {
                var iorg = new CBS_Department()
                {
                    IsActive = false,
                    ModifiedBy = userid,
                    ModifiedDate = DateTime.Now
                };

                var id = _Department.UpdateSpecificValuesNotNull(iorg, m => m.DepartmentID == depid).DepartmentID;

                return id != null || id > 0 ? "Success" : "Error on deleting";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }
        #endregion

        #region Misc      
        public async Task<DepartmentDto> GetTopDepartment(int? depid)
        {
            var dep = await GetDepartmentByID(depid.Value);

            if (dep.ParentID != null)
            {
                return await GetDepartmentByID(dep.ParentID.Value);
            }
            else
            {
                return dep;
            }
        }
        #endregion
    }
}
