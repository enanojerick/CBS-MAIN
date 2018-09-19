using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBS.Data.Entities
{
    public partial class CBS_Department : BaseEntity
    {
        [Key]
        public int? DepartmentID { get; set; }

        public string DepartmentName { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneNumber2 { get; set; }

        public string FilePath { get; set; }

        public int? ParentID { get; set; }

    }

}
