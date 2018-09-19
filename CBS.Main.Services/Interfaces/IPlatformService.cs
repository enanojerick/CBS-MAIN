using Microsoft.AspNetCore.Mvc.Rendering;
using CBS.Data.Entities;
using CBS.Dto.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CBS.Service.Interfaces
{
    public interface IPlatformService
    {
        Task<IList<UserPlatformDto>> GetUserPlatform(int userid);
        Task<UserPlatformDto> GetUserPlatformByID(int platformid);
        Task<int> AddUserPlatform(UserPlatformDto platform, int userid);
        Task<string> UpdateUserPlatform(UserPlatformDto platform, int userid);
        Task<string> DeleteUserPlatform(int platformid, int userid);

        Task<IList<DepartmentPlatformsDto>> GetDepartmentPlatformByDepID(int depid);
        Task<DepartmentPlatformsDto> GetDepartmentPlatformByID(int pid);

        Task<int> AddDepartmentPlatform(DepartmentPlatformsDto platform, int userid);
        Task<string> UpdateDepartmentPlatform(DepartmentPlatformsDto platform, int userid);
        Task<string> DeleteDepartmentPlatform(int platformid, int userid);

        IList<SelectListItem> GetDepartmentPlatformTypeDDList();
    }
}
