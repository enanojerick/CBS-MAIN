using System;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using CBS.Data.Identity;
using CBS.Dto.ManageViewModels;
using CBS.Service.Interfaces;
using CBS.Common.Interface;

namespace CBS.Main.Services
{
    public class ManageServices : IManageServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        public ManageServices(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<ManageServices> logger,
          UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        public async Task<IndexViewModel> GetInitialUserDetails(ClaimsPrincipal User, string statusmessage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = statusmessage
            };


        }

        public async Task<IndexViewModel> GetUserDetails(ClaimsPrincipal User, string statusmessage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = statusmessage
            };
        }

    }
}
