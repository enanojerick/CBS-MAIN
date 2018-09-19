using CBS.Data.Repository.Interface;
using CBS.Data.Entities;
using CBS.Dto.ViewModel;
using CBS.Common.Interface;
using CBS.Common.Services.Extensions;

using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Mvc.Rendering;
using CBS.Service.Interfaces;

namespace CBS.Main.Services
{
    public class PlatformService : IPlatformService
    {
        private IRepository<CBS_UserPlatform> _UserPlatform;
        private IRepository<CBS_DepartmentPlatforms> _DepartmentPlatforms;
        private IRepository<CBS_DepartmentPlatformsType> _DepartmentPlatformType;

        public PlatformService(IRepository<CBS_UserPlatform> UserPlatform, 
                               IRepository<CBS_DepartmentPlatforms> DepartmentPlatforms,
                               IRepository<CBS_DepartmentPlatformsType> DepartmentPlatformType)
        {
            _UserPlatform = UserPlatform;
            _DepartmentPlatforms = DepartmentPlatforms;
            _DepartmentPlatformType = DepartmentPlatformType;
        }

        #region Set to DTO
        private UserPlatformDto SetUserPlatform(CBS_UserPlatform p)
        {
            var ip = new UserPlatformDto()
            {
                PlatformID = p.PlatformID,
                UserId = p.UserId,
                Icon = p.Icon,
                Name = p.Name,
                URL = p.URL,
                IsActive = p.IsActive,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                ModifiedBy = p.ModifiedBy,
                ModifiedDate = p.ModifiedDate,
            };

            return ip;
        }

        private IList<UserPlatformDto> SetUserPlatformList(IList<CBS_UserPlatform> p)
        {
            IList<UserPlatformDto> plist = new List<UserPlatformDto>();

            foreach (var item in p)
            {
                var ip = new UserPlatformDto()
                {
                    PlatformID = item.PlatformID,
                    UserId = item.UserId,
                    Icon = item.Icon,
                    Name = item.Name,
                    URL = item.URL,
                    IsActive = item.IsActive,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate,
                };

                plist.Add(ip);

            }

            return plist;
        }

        private DepartmentPlatformsDto SetDepartmentPlatforms(CBS_DepartmentPlatforms op)
        {
            var iop = new DepartmentPlatformsDto()
            {
                PlatformID = op.PlatformID,
                PlatformTypeID = op.PlatformTypeID,
                DepartmentID = op.DepartmentID,
                Name = op.Name,
                URL = op.URL,
                Icon = op.Icon,
                IsActive = op.IsActive,
                CreatedBy = op.CreatedBy,
                CreatedDate = op.CreatedDate,
                ModifiedBy = op.ModifiedBy,
                ModifiedDate = op.ModifiedDate
            };

            return iop;
        }

        private IList<DepartmentPlatformsDto> SetDepartmentPlatformsList(IList<CBS_DepartmentPlatforms> op)
        {
            IList<DepartmentPlatformsDto> oplist = new List<DepartmentPlatformsDto>();

            try
            {
                foreach (var item in op)
                {
                    var iop = new DepartmentPlatformsDto()
                    {
                        PlatformID = item.PlatformID,
                        PlatformTypeID = item.PlatformTypeID,
                        DepartmentID = item.DepartmentID,
                        Name = item.Name,
                        URL = item.URL,
                        Icon = item.Icon,
                        IsActive = item.IsActive,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                        ModifiedBy = item.ModifiedBy,
                        ModifiedDate = item.ModifiedDate
                    };

                    oplist.Add(iop);
                }

                return oplist;
            }
            catch (Exception ex)
            {
                return oplist;
            }
        }
        #endregion

        #region User Platform CRUD  
        public async Task<IList<UserPlatformDto>> GetUserPlatform(int userid)
        {
            var ipf = (from u in _UserPlatform.GetAll()
                       where u.IsActive == true
                       && u.UserId == userid
                       select u).ToList();

            var userp = SetUserPlatformList(ipf);

            return userp;
        }

        public async Task<UserPlatformDto> GetUserPlatformByID(int platformid)
        {
            var ipf = (from u in _UserPlatform.GetAll()
                       where u.IsActive == true
                       && u.PlatformID == platformid
                       select u).FirstOrDefault();

            var userplatform = SetUserPlatform(ipf);

            return userplatform;
        }

        #region Add , Edit , Archive
        public async Task<int> AddUserPlatform(UserPlatformDto platform, int userid)
        {
            try
            {
                var iplatform = new CBS_UserPlatform()
                {
                    UserId = platform.UserId,
                    Icon = platform.Icon,
                    Name = platform.Name,
                    URL = platform.URL,
                    IsActive = true,
                    CreatedBy = userid,
                    CreatedDate = DateTime.Now

                };
                return _UserPlatform.Insert(iplatform).PlatformID.Value;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<string> UpdateUserPlatform(UserPlatformDto platform, int userid)
        {
            try
            {
                var iplatform = _UserPlatform.SearchFor(m => m.PlatformID == platform.PlatformID).FirstOrDefault();

                iplatform.Icon = platform.Icon;
                iplatform.Name = platform.Name;
                iplatform.URL = platform.URL;
                iplatform.ModifiedBy = userid;
                iplatform.ModifiedDate = DateTime.Now;

                var id = _UserPlatform.Update(iplatform, m => m.PlatformID == platform.PlatformID).PlatformID;

                return id != null || id > 0 ? "Success" : "Error on updating";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }

        public async Task<string> DeleteUserPlatform(int platformid, int userid)
        {
            try
            {
                var iplatform = _UserPlatform.SearchFor(m => m.PlatformID == platformid).FirstOrDefault();

                iplatform.IsActive = false;
                iplatform.ModifiedBy = userid;
                iplatform.ModifiedDate = DateTime.Now;

                var id = _UserPlatform.Update(iplatform, m => m.PlatformID == platformid).PlatformID;

                return id != null || id > 0 ? "Success" : "Error on deleting";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }
        #endregion

        #endregion

        #region Organization Platforms CRUD
        public async Task<IList<DepartmentPlatformsDto>> GetDepartmentPlatformByDepID(int depid)
        {

            try
            {
                var iop = (from u in _DepartmentPlatforms.GetAll()
                           where u.IsActive == true
                           && u.DepartmentID == depid
                           select u).ToList();

                var op = SetDepartmentPlatformsList(iop);

                foreach (var item in op)
                {
                    item.PlatformType = GetDepartmentPlatformTypeDDList().Where(m => m.Value == item.PlatformTypeID.ToString())
                                                                               .FirstOrDefault()
                                                                               .Text;
                }

                return op;
            }
            catch (Exception)
            {
                return new List<DepartmentPlatformsDto>();
            }
            
        }

        public async Task<DepartmentPlatformsDto> GetDepartmentPlatformByID(int pid)
        {
            try
            {
                var iop = (from u in _DepartmentPlatforms.GetAll()
                           where u.IsActive == true
                           && u.PlatformID == pid
                           select u).FirstOrDefault();

                var platform = SetDepartmentPlatforms(iop);

                platform.PlatformType = GetDepartmentPlatformTypeDDList().Where(m => m.Value == platform.PlatformTypeID.ToString())
                                                                               .FirstOrDefault()
                                                                               .Text;

                return platform;
            }
            catch (Exception)
            {
                return new DepartmentPlatformsDto();
            }
            
        }

        public async Task<int> AddDepartmentPlatform(DepartmentPlatformsDto platform, int userid)
        {
            try
            {
                var iplatform = new CBS_DepartmentPlatforms()
                {
                    PlatformTypeID = platform.PlatformTypeID,
                    DepartmentID = platform.DepartmentID,
                    Icon = platform.Icon,
                    Name = platform.Name,
                    URL = platform.URL,
                    IsActive = true,
                    CreatedBy = userid,
                    CreatedDate = DateTime.Now,
                    

                };

                return _DepartmentPlatforms.Insert(iplatform).PlatformID.Value;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<string> UpdateDepartmentPlatform(DepartmentPlatformsDto platform, int userid)
        {
            try
            {
                var iplatform = _DepartmentPlatforms.SearchFor(m => m.PlatformID == platform.PlatformID).FirstOrDefault();

                iplatform.Icon = platform.Icon;
                iplatform.Name = platform.Name;
                iplatform.URL = platform.URL;
                iplatform.PlatformTypeID = platform.PlatformTypeID;
                iplatform.ModifiedBy = userid;
                iplatform.ModifiedDate = DateTime.Now;

                var id = _DepartmentPlatforms.Update(iplatform, m => m.PlatformID == platform.PlatformID).PlatformID;

                return id != null || id > 0 ? "Success" : "Error on updating";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }

        public async Task<string> DeleteDepartmentPlatform(int platformid, int userid)
        {
            try
            {
                var iplatform = _DepartmentPlatforms.SearchFor(m => m.PlatformID == platformid).FirstOrDefault();

                iplatform.IsActive = false;
                iplatform.ModifiedBy = userid;
                iplatform.ModifiedDate = DateTime.Now;

                var id = _DepartmentPlatforms.Update(iplatform, m => m.PlatformID == platformid).PlatformID;

                return id != null || id > 0 ? "Success" : "Error on deleting";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.ToString();
            }
        }

        #endregion

        #region Misc
        public IList<SelectListItem> GetDepartmentPlatformTypeDDList()
        {
            var orgpTypeList = (from c in _DepartmentPlatformType.GetAll()
                               where c.IsActive == true
                               select new SelectListItem
                               {
                                   Text = c.Name,
                                   Value = c.PlatformTypeID.ToString()
                               }).ToList();

            return orgpTypeList;
        }

        #endregion



    }
}
