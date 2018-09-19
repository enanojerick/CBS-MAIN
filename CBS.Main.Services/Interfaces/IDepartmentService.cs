using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using CBS.Data.Entities;
using CBS.Dto.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CBS.Service.Interfaces
{
    public interface IDepartmentService
    {

        Task<DepartmentDto> GetDepartmentByID(int depid);
        Task<IList<DepartmentDto>> GetDepartmentBranchByDeptID(int depid);

        IList<DepartmentDto> GetDepartmentBranchByOrgIDList(int depid);
        IList<DepartmentDto> GetDepartmentBranchSingleListByDeptID(int depid);
        IList<SelectListItem> GetDepartmentBranchDDLByDeptID(int depid);
        IList<DepartmentDto> GetActiveDepartmentList();

        Task<int> AddDepartment(DepartmentDto dep, int userid);
        Task<int> UpdateDepartmentAsync(DepartmentDto dep, int userid);
        string UpdateDepartmentParent(DepartmentDto dep, int userid);
        string DeleteDepartment(int depid, int userid);

        Task<DepartmentDto> GetTopDepartment(int? depid);
    }
}
