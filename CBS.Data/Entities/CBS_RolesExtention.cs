using System.ComponentModel.DataAnnotations;

namespace CBS.Data.Entities
{
    public partial class CBS_RolesExtention : BaseEntity
    {
        [Key]
        public int? RoleExtId { get; set; }

        public string Role { get; set; }

        public string RoleId { get; set; }

        public string Description { get; set; }

    }
}
