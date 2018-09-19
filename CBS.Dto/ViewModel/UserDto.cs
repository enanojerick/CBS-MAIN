using System;
using System.Collections.Generic;
using System.Text;
using CBS.Dto.Base;

namespace CBS.Dto.ViewModel
{
    public class UserDto :  BaseDto
    {
        public int? UserID { get; set; }

        public string IdentityID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string PreferedName { get; set; }

        public string RegisterPassword { get; set; }

        public bool? IsFirstLogin { get; set; }

        public string PlatformSorting { get; set; }

        public int? DepartmentID { get; set; }

        public DepartmentDto Department { get; set; }
    }
}
