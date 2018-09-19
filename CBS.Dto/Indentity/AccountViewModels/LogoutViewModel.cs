using System;
using System.Collections.Generic;
using System.Text;

namespace CBS.Dto.AccountViewModels
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; }
    }
}
