using System.ComponentModel.DataAnnotations;

namespace CBS.Data.Entities
{
    public partial class CBS_DepartmentPlatformsType : BaseEntity
    {
        [Key]
        public int? PlatformTypeID { get; set; }

        public string Name { get; set; }

    }
}
