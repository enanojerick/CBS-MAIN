using CBS.Data.Entities;
using CBS.Dto.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CBS.Service.Interfaces
{
    public interface IApplicationsService
    {
        IList<ApplicationDto> GetApplicationAll();
        ApplicationDto GetApplicationByID(int appid);
        ApplicationDto GetApplicationBySSO(string clientid, Guid clientsecret);
        IList<UsersAppsDto> GetUserAppsByUserID(int userid);
        Task<int> AddUserApps(UsersAppsDto app, int userid);
        bool DeleteUsersApp(int? usersappid, int? userid);
        string UpdateUserApplication(UsersAppsDto app, int userid);
        string DeleteUsersApplication(int usersappid, int userid);
    }
}
