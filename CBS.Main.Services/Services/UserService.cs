using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

using Microsoft.AspNetCore.Identity;

using CBS.Data.Identity;
using CBS.Dto.ViewModel;
using CBS.Data.Repository.Interface;
using CBS.Data.Entities;
using CBS.Service.Interfaces;

namespace CBS.Main.Services
{
    public class UserService : IUserService
    {
        private IRepository<CBS_Users> _Users;
        private readonly IDepartmentService _DepartmentService;
        private readonly UserManager<ApplicationUser> _UserManager;

        public UserService(IRepository<CBS_Users> Users,
                           IDepartmentService DepartmentService,
                           UserManager<ApplicationUser> UserManager)
        {
            _Users = Users;
            _DepartmentService = DepartmentService;
            _UserManager = UserManager;
        }

        #region Dto
        private UserDto SetUserProfile(CBS_Users user)
        {
            var iuser = new UserDto()
            {
                UserID = user.UserID,
                IdentityID = user.IdentityID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mobile = user.Mobile,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.CreatedDate,
                ModifiedBy = user.ModifiedBy,
                ModifiedDate = user.ModifiedDate,
                IsFirstLogin = user.IsFirstLogin,
                PlatformSorting = user.PlatformSorting,
                DepartmentID = user.DepartmentID,


            };

            return iuser;
        }

        private IList<UserDto> SetUserList(IList<CBS_Users> user)
        {
            IList<UserDto> userlist = new List<UserDto>();

            var org = _DepartmentService.GetActiveDepartmentList();

            foreach (var item in user)
            {
                var iuser = new UserDto()
                {
                    UserID = item.UserID,
                    IdentityID = item.IdentityID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Mobile = item.Mobile,
                    Email = item.Email,
                    IsActive = item.IsActive,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                    IsFirstLogin = item.IsFirstLogin,
                    PlatformSorting = item.PlatformSorting,
                    DepartmentID = item.DepartmentID,
                };

                userlist.Add(iuser);

            }

            return userlist;
        }

        #endregion

        #region Get    
        public UserDto GetProfileByEmail(string email)
        {
            var iup = (from u in _Users.GetAll()
                       where u.IsActive == true
                       && u.Email == email
                       select u).FirstOrDefault();

            var userprof = iup == null ? null : SetUserProfile(iup);

            return userprof;
        }

        public UserDto GetProfileByEmailOAuth(string email)
        {
            var iup = (from u in _Users.GetAll()
                       where u.IsActive == true
                       && u.Email == email
                       select u).FirstOrDefault();

            var userprof = iup == null ? null : SetUserProfile(iup);

            return userprof;
        }

        public UserDto GetProfileByIdentityID(string identityid)
        {
            var iup = (from u in _Users.GetAll()
                       where u.IsActive == true
                       && u.IdentityID == identityid
                       select u).FirstOrDefault();

            var userprof = SetUserProfile(iup);

            return userprof;
        }

        public UserDto GetUserProfileByID(int userid)
        {
            var iup = (from u in _Users.GetAll()
                       where u.IsActive == true
                       && u.UserID == userid
                       select u).FirstOrDefault();

            var userprof = SetUserProfile(iup);

            return userprof;
        }


        public IList<UserDto> GetProfileByIdentityIDList(string[] identityids)
        {
            var iup = (from u in _Users.GetAll()
                       where u.IsActive == true
                       && identityids.Contains(u.IdentityID)
                       select u).ToList();

            var userprof = SetUserList(iup);

            return userprof;
        }

        public IList<UserDto> GetUserProfileList()
        {
            var iup = (from u in _Users.GetAll()
                       where u.IsActive == true
                       select u).ToList();

            var userprof = SetUserList(iup);

            return userprof;
        }

        public IList<UserDto> GetUserProfileListByDepartmentId(int? depid)
        {
            var iup = (from u in _Users.GetAll()
                       where u.IsActive == true
                       && u.DepartmentID == depid
                       select u).ToList();

            var userprof = SetUserList(iup);

            return userprof;
        }
        #endregion

        public async Task<IList<UserDto>> GetUserByDepartmentBranch(int? depid)
        {
            var department = _DepartmentService.GetDepartmentBranchByOrgIDList(depid.Value);

            var iup = (from u in GetUserProfileList()
                       where u.IsActive == true
                       select u).ToList();

            IList<UserDto> userlist = new List<UserDto>();

            foreach (var item in department)
            {
                var users = iup.Where(m => m.DepartmentID == item.DepartmentID).ToList();

                if (users != null)
                {
                    foreach (var userItem in users)
                    {
                        userItem.Department = department.Where(m => m.DepartmentID == userItem.DepartmentID).FirstOrDefault();
                        userlist.Add(userItem);
                    }
                }
            }

            var parentOrgUser = iup.Where(m => m.DepartmentID == depid).ToList();

            if (parentOrgUser != null)
            {
                foreach (var userItem in parentOrgUser)
                {
                    userItem.Department = await _DepartmentService.GetDepartmentByID(userItem.DepartmentID.Value);
                    userlist.Add(userItem);
                }
            }
            return userlist;
        }

        #region CRUD     
        public int AddUser(UserDto user, int userid)
        {
            try
            {
                var iuser = new CBS_Users()
                {
                    IdentityID = user.IdentityID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Mobile = RemoveLeadingZero(user.Mobile),
                    Email = user.Email,
                    IsFirstLogin = true,
                    IsActive = true,
                    CreatedBy = userid,
                    CreatedDate = DateTime.Now,
                    RegisterPassword = user.RegisterPassword,
                    DepartmentID = user.DepartmentID,

                };

                return _Users.Insert(iuser).UserID.Value;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string UpdateUser(UserDto user, int? userid)
        {
            try
            {
                var iuser = _Users.SearchFor(m => m.UserID == user.UserID).FirstOrDefault();

                iuser.FirstName = user.FirstName;
                iuser.LastName = user.LastName;
                iuser.Mobile = RemoveLeadingZero(user.Mobile);
                iuser.ModifiedBy = userid;
                iuser.ModifiedDate = DateTime.Now;

                var id = _Users.Update(iuser, m => m.UserID == user.UserID).UserID;
                return id != null || id > 0 ? "Success" : "Error on updating";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }

        public async Task<string> DeleteUser(string identityid, int? userid)
        {
            try
            {
                var iuser = new CBS_Users()
                {
                    IsActive = false,
                    ModifiedBy = userid,
                    ModifiedDate = DateTime.Now
                };

                var deletedUser = _Users.UpdateSpecificValuesNotNull(iuser, m => m.IdentityID == identityid);
                var id = deletedUser.UserID;

                ApplicationUser targetUser = await _UserManager.FindByEmailAsync(deletedUser.Email);
                var targetUserRoles = await _UserManager.GetRolesAsync(targetUser);

                if (targetUserRoles.Count > 0)
                {
                    foreach (var item in targetUserRoles)
                    {
                        var result = await _UserManager.RemoveFromRoleAsync(targetUser, item);
                    }
                }

                await _UserManager.DeleteAsync(targetUser);

                return id != null || id > 0 ? "Success" : "Error on deleting";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }
        #endregion

        #region Misc and User Platform Settings      
        public string LoggedInFirst(string identityid, int userid)
        {
            try
            {
                var iuser = new CBS_Users()
                {
                    IsFirstLogin = false,
                    ModifiedBy = userid,
                    ModifiedDate = DateTime.Now
                };

                var id = _Users.UpdateSpecificValuesNotNull(iuser, m => m.IdentityID == identityid).UserID;

                return id != null || id > 0 ? "Success" : "Error";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }

        public int? UpdateUserPlatformSorting(string data, int? userID)
        {
            var user = _Users.SearchFor(m => m.UserID == userID).FirstOrDefault();

            user.PlatformSorting = data;
            user.ModifiedBy = userID;
            user.ModifiedDate = DateTime.Now;

            var id = _Users.Update(user, m => m.UserID == user.UserID).UserID;
            return id;
        }

        public string GetUserPlatformSorting(int? userID)
        {
            var data = "";
            var user = _Users.SearchFor(m => m.UserID == userID).FirstOrDefault();
            data = user.PlatformSorting;

            return data;
        }

        public string RemoveLeadingZero(string text)
        {
            string resultString = text;

            if (!string.IsNullOrEmpty(resultString))
            {
                string firstChar = text.Substring(0, 1);
                if (firstChar == "0") resultString = text.Remove(0, 1);
            }

            return resultString;
        }

        #endregion
    }
}
