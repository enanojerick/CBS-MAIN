using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;
using CBS.Dto.Base;

namespace CBS.Dto.ViewModel
{
    public class UsersAppsDto :  BaseDto
    {
        public int? UsersAppID { get; set; }

        public int? UserID { get; set; }

        public int? AppID { get; set; }

        public ApplicationDto Application { get; set; }
    }
}
