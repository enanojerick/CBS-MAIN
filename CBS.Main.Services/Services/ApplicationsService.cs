using System.Threading.Tasks;

using CBS.Data.Repository.Interface;
using CBS.Data.Entities;
using CBS.Dto.ViewModel;

using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using CBS.Common.Interface;
using System.IO;
using CBS.Common.Services.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Identity;
using CBS.Data.Identity;
using CBS.Service.Interfaces;

namespace CBS.Main.Services
{
    public class ApplicationsService : IApplicationsService
    {
        private readonly IRepository<CBS_Application> _Applications;
        private readonly IRepository<CBS_UsersApps> _UsersApps;
        private readonly IUserService _UserService;
        private readonly UserManager<ApplicationUser> _UserManager;

        public ApplicationsService(IRepository<CBS_Application> Applications,
                                   IRepository<CBS_UsersApps> UsersApps,
                                   IUserService UserService,
                                   UserManager<ApplicationUser> UserManager)
        {
            _Applications = Applications;
            _UserService = UserService;
            _UserManager = UserManager;
            _UsersApps = UsersApps;
        }

        #region  Applications DTOs  
        private ApplicationDto SetApplicationDto(CBS_Application app)
        {
            var iapp = new ApplicationDto()
            {
                AppID = app.AppID,
                URL = app.URL,
                IsActive = app.IsActive,
                CreatedBy = app.CreatedBy,
                CreatedDate = app.CreatedDate,
                ModifiedBy = app.ModifiedBy,
                ModifiedDate = app.ModifiedDate,
                ClientID = app.ClientID,
                ClientName = app.ClientName,
                CorsOrigins = app.CorsOrigins,
                ReturnURL = app.ReturnURL,
                Sha256 = app.Sha256,
                PostLogoutRedirectUris = app.PostLogoutRedirectUris,
                RedirectURIs = app.RedirectURIs,
            };

            return iapp;
        }

        private IList<ApplicationDto> SetApplicationDtoList(IList<CBS_Application> app)
        {
            IList<ApplicationDto> applist = new List<ApplicationDto>();

            foreach (var item in app)
            {
                var iapp = new ApplicationDto()
                {
                    AppID = item.AppID,
                    URL = item.URL,
                    IsActive = item.IsActive,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    ClientID = item.ClientID,
                    ClientName = item.ClientName,
                    CorsOrigins = item.CorsOrigins,
                    ReturnURL = item.ReturnURL,
                    Sha256 = item.Sha256,
                    PostLogoutRedirectUris = item.PostLogoutRedirectUris,
                    RedirectURIs = item.RedirectURIs,
                };

                applist.Add(iapp);
            }

            return applist;
        }
        #endregion

        #region User Applications DTOs  
        private UsersAppsDto SetUsersAppsDto(CBS_UsersApps app)
        {
            var iapp = new UsersAppsDto()
            {
                UsersAppID = app.UsersAppID,
                AppID = app.AppID,
                UserID = app.UserID,
                IsActive = app.IsActive,
                CreatedBy = app.CreatedBy,
                CreatedDate = app.CreatedDate,
                ModifiedBy = app.ModifiedBy,
                ModifiedDate = app.ModifiedDate,
            };

            return iapp;
        }

        private IList<UsersAppsDto> SetUsersAppsDtoList(IList<CBS_UsersApps> app)
        {
            IList<UsersAppsDto> applist = new List<UsersAppsDto>();

            foreach (var item in app)
            {
                var iapp = new UsersAppsDto()
                {
                    UsersAppID = item.UsersAppID,
                    AppID = item.AppID,
                    UserID = item.UserID,
                    IsActive = item.IsActive,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                };

                applist.Add(iapp);
            }

            return applist;
        }
        #endregion

        #region Get 
        #region Get Application
        public IList<ApplicationDto> GetApplicationAll()
        {
            var ipf = (from u in _Applications.GetAll()
                       where u.IsActive == true
                       select u).ToList();

            var applist = SetApplicationDtoList(ipf);

            return applist;
        }

        public ApplicationDto GetApplicationByID(int appid)
        {
            try
            {
                var ipf = (from ui in _Applications.GetAll()
                           where ui.IsActive == true
                           && ui.AppID == appid
                           select ui).FirstOrDefault();

                var app = SetApplicationDto(ipf);

                return app;
            }
            catch (Exception ex)
            {
                return new ApplicationDto();
            }

        }

        public ApplicationDto GetApplicationBySSO(string clientid, Guid clientsecret)
        {
            try
            {
                var ipf = (from u in _Applications.GetAll()
                           where u.IsActive == true
                           && u.ClientID == clientid
                           && u.Sha256 == clientsecret.ToString()
                           select u).FirstOrDefault();

                return SetApplicationDto(ipf);
            }
            catch (Exception ex)
            {
                return new ApplicationDto();
            }
        }
        #endregion

        #region Get User Application 
        public IList<UsersAppsDto> GetUserAppsByUserID(int userid)
        {
            IList<UsersAppsDto> uapplist = new List<UsersAppsDto>();
            try
            {
                var ipf = (from u in _UsersApps.GetAll()
                           where u.IsActive == true
                           && u.UserID == userid
                           select u).ToList();

                uapplist = SetUsersAppsDtoList(ipf);

                foreach (var item in uapplist)
                {
                    item.Application = GetApplicationByID(item.AppID.Value);
                }

                return uapplist;
            }
            catch (Exception ex)
            {
                return uapplist;
            }
        }
        #endregion

        #endregion

        #region Add
        public async Task<int> AddUserApps(UsersAppsDto app, int userid)
        {
            try
            {
                var user = _UsersApps.SearchFor(m => m.UserID == app.UserID.Value && m.AppID == app.AppID && m.IsActive == false).FirstOrDefault();

                var userdetails = _UserService.GetUserProfileByID(app.UserID.Value);

                bool IsSA = false;

                ApplicationUser targetUser = await _UserManager.FindByEmailAsync(userdetails.Email);

                var targetUserRoles = await _UserManager.GetRolesAsync(targetUser);

                foreach (var item in targetUserRoles)
                {
                    if (item.ToLower() == "super administrator")
                    {
                        IsSA = true;
                    }
                }

                if (user != null)
                {
                    if (user.UsersAppID.Value != 0)
                    {

                        user.IsActive = true;

                        var i = _UsersApps.Update(user, m => m.UsersAppID == user.UsersAppID).UsersAppID.Value;

                        #region Old Code [0]
                        //await CreateHQUser(app.UserID.Value, orgid, app.AppID.Value, IsSA);
                        #endregion

                        return i;
                    }
                }
               

                var iapp = new CBS_UsersApps()
                {
                    AppID = app.AppID,
                    UserID = app.UserID,
                    IsActive = true,
                    CreatedBy = userid,
                    CreatedDate = DateTime.Now

                };

                var id = _UsersApps.Insert(iapp).UsersAppID.Value;

                #region Old Code [1]
                //if (id != 0)
                //{
                //    await CreateHQUser(app.UserID.Value, orgid, app.OrgAppID.Value, IsSA);
                //}
                #endregion


                return id;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public bool DeleteUsersApp(int? usersappid, int? userid)
        {
            try
            {
                var iapp = new CBS_UsersApps()
                {
                    IsActive = false,
                    ModifiedBy = userid,
                    ModifiedDate = DateTime.Now
                };

                var id = _UsersApps.UpdateSpecificValuesNotNull(iapp, m => m.UsersAppID == usersappid).UsersAppID;
                
                return id != null || id > 0 ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region Old Region [2]
        //public async Task<int> AddUserOrgAppsMultiple(UsersAppsDto app, int userid, int orgid, string deployment)
        //{
        //    try
        //    {
        //        bool IsSA = false;

        //        var userdetails = _UserService.GetUserProfileByID(app.UserID.Value);

        //        ApplicationUser targetUser = await _UserManager.FindByEmailAsync(userdetails.Email);
        //        var targetUserRoles = await _UserManager.GetRolesAsync(targetUser);

        //        foreach (var item in targetUserRoles)
        //        {
        //            if (item.ToLower() == "super administrator")
        //            {
        //                IsSA = true;
        //            }
        //        }

        //        if (app.AppIDs != null)
        //        {
        //            foreach (var id in app.OrgAppIDs)
        //            {
        //                var user = _UsersOrgApps.SearchFor(m => m.UserID == app.UserID.Value && m.OrgAppID == id.Value).Count();

        //                if (user != 0)
        //                {
        //                    continue;
        //                }

        //                var iapp = new HQ_UsersOrgApps()
        //                {
        //                    OrgAppID = id,
        //                    UserID = app.UserID,
        //                    IsActive = true,
        //                    CreatedBy = userid,
        //                    CreatedDate = DateTime.Now

        //                };

        //                _UsersOrgApps.Insert(iapp);

        //                await CreateHQUser(app.UserID.Value, orgid, id.Value, IsSA);
        //            }

        //            return 1;
        //        }
        //        else
        //        {
        //            return await AddUserOrgApps(app, userid, orgid, deployment);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}


        //private async Task<bool> CreateHQUser(int userid, int orgid, int orgappid, bool sarole)
        //{
        //    try
        //    {
        //        var user = _UserService.GetUserProfileByID(userid);

        //        var orgapps = await GetOrganizationAppsByID(orgappid);

        //        if (orgapps.Applications.IsHR == 1)
        //        {

        //            var connecthrlink =  orgapps.CorsOrigins;

        //            string data = "?mainid=" + HttpUtility.UrlEncode(user.IdentityID) 
        //                        + "&fname=" + HttpUtility.UrlEncode(user.FirstName) 
        //                        + "&lname=" + HttpUtility.UrlEncode(user.LastName) 
        //                        + "&email=" + HttpUtility.UrlEncode(user.Email) 
        //                        + "&orgid=" + HttpUtility.UrlEncode(orgid.ToString())
        //                        + "&sarole=" + HttpUtility.UrlEncode(sarole.ToString());

        //            HttpClientHandler handler = new HttpClientHandler();
        //            HttpClient httpClient = new HttpClient(handler);
        //            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, connecthrlink + "api/ConnectHQ/User/Add" + data);
        //            HttpResponseMessage response = await httpClient.SendAsync(request);
        //            response.EnsureSuccessStatusCode();

        //            var sRes = response.EnsureSuccessStatusCode();

        //            if (sRes != null)
        //            {
        //                return true;
        //            }
        //        }

        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }


        //}

        //public class HQUserModel
        //{
        //    public string mainid { get; set; }
        //    public string fname { get; set; }
        //    public string lname { get; set; }
        //    public string email { get; set; }
        //    public int orgid { get; set; }
        //}
        #endregion

        #endregion

        #region Update
        public string UpdateUserApplication(UsersAppsDto app, int userid)
        {
            try
            {
                var iapp = _UsersApps.SearchFor(m => m.UsersAppID == app.UsersAppID).FirstOrDefault();

                iapp.UserID = app.UserID;
                iapp.AppID = app.AppID;
                iapp.ModifiedBy = userid;
                iapp.ModifiedDate = DateTime.Now;
                iapp.IsActive = true;

                var id = _UsersApps.Update(iapp, m => m.UsersAppID == app.UsersAppID).UsersAppID;

                return id != null || id > 0 ? "Success" : "Error on updating";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }

        #endregion

        #region Delete



        public string DeleteUsersApplication(int usersappid, int userid)
        {
            try
            {
                var iapp = new CBS_UsersApps()
                {
                    IsActive = false,
                    ModifiedBy = userid,
                    ModifiedDate = DateTime.Now
                };

                var id = _UsersApps.UpdateSpecificValuesNotNull(iapp, m => m.UsersAppID == usersappid).UsersAppID;

                return id != null || id > 0 ? "Success" : "Error on deleting";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }
        #endregion

    }
}
