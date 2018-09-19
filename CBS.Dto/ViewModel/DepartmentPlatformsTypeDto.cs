using System;
using System.Collections.Generic;
using System.Text;
using CBS.Dto.Base;

namespace CBS.Dto.ViewModel
{
    public class DepartmentPlatformsTypeDto : BaseDto
    {
        public int? PlatformTypeID { get; set; }

        public string Name { get; set; }
    }
}
