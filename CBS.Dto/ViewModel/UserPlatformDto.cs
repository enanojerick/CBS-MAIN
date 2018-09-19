using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using CBS.Dto.Base;

namespace CBS.Dto.ViewModel
{
    public class UserPlatformDto :  BaseDto
    {
        public int? PlatformID { get; set; }

        public int? UserId { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public string Icon { get; set; }

    }
}
