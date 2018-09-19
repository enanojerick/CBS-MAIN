using IdentityModel;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

using System.Linq;
using Microsoft.AspNetCore.Identity;
using CBS.Data.Identity;
using CBS.Service.Interfaces;

namespace CBS.Main.Extensions
{

    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _UserService;

        public ProfileService(UserManager<ApplicationUser> userManager,
                              IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
                              IUserService UserService)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _UserService = UserService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            var userdetails = _UserService.GetProfileByIdentityID(user.Id);

            if (userdetails.FirstName != null)
            {
                claims.Add(new Claim(JwtClaimTypes.GivenName, userdetails.FirstName));
            }

            if (userdetails.LastName != null)
            {
                claims.Add(new Claim(JwtClaimTypes.FamilyName, userdetails.LastName));
            }

            var roles = await _userManager.GetRolesAsync(user);


            foreach (var item in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, item));
                //claims.Add(new Claim(JwtClaimTypes.GivenName, item));
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
