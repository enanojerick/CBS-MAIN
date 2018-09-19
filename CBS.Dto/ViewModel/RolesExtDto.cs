using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using CBS.Dto.Base;

namespace CBS.Dto.ViewModel
{
    public class RolesExtDto:  BaseDto
    {
        public int? RoleExtId { get; set; }

        public string Role { get; set; }

        public string RoleId { get; set; }

        public string Description { get; set; }
    }
}
