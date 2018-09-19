using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using CBS.Dto.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CBS.Dto.ViewModel
{
    public class DepartmentPlatformsDto : BaseDto
    {
        public int? PlatformID { get; set; }

        public int? PlatformTypeID { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public string Icon { get; set; }

        public int? DepartmentID { get; set; }

        public string PlatformType { get; set; }
    }
}
