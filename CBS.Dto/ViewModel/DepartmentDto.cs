using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;
using CBS.Dto.Base;

namespace CBS.Dto.ViewModel
{
    public class DepartmentDto :  BaseDto
    {
        public int? DepartmentID { get; set; }

        public string DepartmentName { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneNumber2 { get; set; }

        public string FilePath { get; set; }

        public int? ParentID { get; set; }

        public IList<DepartmentDto> Branches { get; set; }
    }
}
