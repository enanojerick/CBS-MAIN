using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBS.Data.Entities
{
    public partial class CBS_UserPlatform : BaseEntity
    {
        [Key]
        public int? PlatformID { get; set; }

        [ForeignKey("CBS_Users")]
        public int? UserId { get; set; }
        public CBS_Users Users { get; set; }


        public string Name { get; set; }

        public string URL { get; set; }

        public string Icon { get; set; }
    }
}
