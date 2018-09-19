using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using CBS.Dto.ManageViewModels;

namespace CBS.Service.Interfaces
{
    public interface IManageServices
    {
        Task<IndexViewModel> GetInitialUserDetails(ClaimsPrincipal User, string statusmessage);
    }
}
