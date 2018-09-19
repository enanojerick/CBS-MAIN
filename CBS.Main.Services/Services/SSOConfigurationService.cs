using CBS.Data.Entities;
using CBS.Dto.ViewModel;
using System.Collections.Generic;
using System.Linq;

using CBS.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CBS.Main.Services
{
    public class SSOConfigurationService
    {
        private CBSContext Db;

        private IList<ApplicationDto> SetOrganizationAppsList(IList<CBS_Application> app)
        {
            IList<ApplicationDto> applist = new List<ApplicationDto>();

            foreach (var item in app)
            {
                var iapp = new ApplicationDto()
                {
                    AppID = item.AppID,
                    URL = item.URL,
                    ClientID = item.ClientID,
                    ClientName = item.ClientName,
                    CorsOrigins = item.CorsOrigins,
                    Sha256 = item.Sha256,
                    RedirectURIs = item.RedirectURIs,
                    PostLogoutRedirectUris = item.PostLogoutRedirectUris,
                    IsActive = item.IsActive,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    ModifiedBy = item.ModifiedBy,
                    ModifiedDate = item.ModifiedDate
                };

                applist.Add(iapp);
            }

            return applist;
        }

        public IList<ApplicationDto> GetApplications(string connectionstring)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CBSContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            Db = new CBSContext(optionsBuilder.Options);

            var apps = Db.Set<CBS_Application>();

            var appsenum = apps.Where(m => m.IsActive == true).ToList();

            var appslist = SetOrganizationAppsList(appsenum);

            Db.Dispose();

            return appslist;
        }

    }
}
