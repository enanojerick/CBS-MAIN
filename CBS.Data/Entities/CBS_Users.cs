using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBS.Data.Entities
{
    public partial class CBS_Users : BaseEntity
    {
        [Key]
        public int? UserID { get; set; }

        //Aspnet User ID
        public string IdentityID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string PreferedName { get; set; }

        public string RegisterPassword { get; set; }

        public bool? IsFirstLogin { get; set; }

        public string PlatformSorting { get; set; }

        [ForeignKey("CBS_Department")]
        public int? DepartmentID { get; set; }
        public CBS_Department Department { get; set; }

    }
}
