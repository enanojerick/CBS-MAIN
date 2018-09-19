using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBS.Data.Entities
{
    public partial class CBS_UsersApps : BaseEntity
    {
        [Key]
        public int? UsersAppID { get; set; }

        [ForeignKey("CBS_Users")]
        public int? UserID { get; set; }
        public CBS_Users Users { get; set; }

        [ForeignKey("CBS_Application")]
        public int? AppID { get; set; }
        public CBS_Application Application { get; set; }
    }
}
