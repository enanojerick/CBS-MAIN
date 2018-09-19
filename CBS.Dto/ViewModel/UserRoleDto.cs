using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;
using CBS.Dto.Base;

namespace CBS.Dto.ViewModel
{
    public class UserRoleDto :  BaseDto
    {        
        public string user { get; set; }

        public string role { get; set; }

        public bool selected { get; set; }

        public string selectedrole { get; set; }
    }
}
