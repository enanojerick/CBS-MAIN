using System;
using System.Collections.Generic;
using System.Text;

namespace CBS.Dto.AccountViewModels
{
    public class LogoutInputModel
    {
        public string LogoutId { get; set; }

        public string AccessId { get; set; }

        public string ClientID { get; set; }

        public bool IsFromExt { get; set; }
    }
}
