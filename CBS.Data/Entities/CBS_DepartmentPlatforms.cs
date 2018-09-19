using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBS.Data.Entities
{
    public partial class CBS_DepartmentPlatforms : BaseEntity
    {
        [Key]
        public int? PlatformID { get; set; }

        [ForeignKey("CBS_DepartmentPlatformsType")]
        public int? PlatformTypeID { get; set; }
        public CBS_DepartmentPlatformsType DepartmentPlatformsType { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public string Icon { get; set; }

        [ForeignKey("CBS_Department")]
        public int? DepartmentID { get; set; }
        public CBS_Department Department { get; set; }
    }
}
