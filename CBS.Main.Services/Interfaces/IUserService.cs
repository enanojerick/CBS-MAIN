using CBS.Dto.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CBS.Service.Interfaces
{
    public interface IUserService
    {
        UserDto GetProfileByEmail(string email);
        UserDto GetProfileByEmailOAuth(string email);
        UserDto GetProfileByIdentityID(string identityid);
        UserDto GetUserProfileByID(int userid);

        IList<UserDto> GetProfileByIdentityIDList(string[] identityids);
        IList<UserDto> GetUserProfileList();
        IList<UserDto> GetUserProfileListByDepartmentId(int? depid);

        Task<IList<UserDto>> GetUserByDepartmentBranch(int? depid);

        int AddUser(UserDto user, int userid);
        string UpdateUser(UserDto user, int? userid);
        Task<string> DeleteUser(string identityid, int? userid);

        string LoggedInFirst(string identityid, int userid);
        int? UpdateUserPlatformSorting(string data, int? userID);
        string GetUserPlatformSorting(int? userID);
        string RemoveLeadingZero(string text);
    }
}
