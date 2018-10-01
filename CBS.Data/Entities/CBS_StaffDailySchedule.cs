using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBS.Data.Entities
{
    public partial class CBS_StaffDailySchedule : BaseEntity
    {
        [Key]
        public int? DailySchedID { get; set; }

        public int? UserID { get; set; }

        public int? DepartmentID { get; set; }

        public DateTime ScheduleDate { get; set; }

        public DateTime ScheduleTimeIn { get; set; }

        public DateTime ScheduleTimeOut { get; set; }

        public DateTime ScheduleLunchIn { get; set; }

        public DateTime ScheduleLunchOut { get; set; }

        public DateTime TimeIn { get; set; }

        public DateTime TimeOut { get; set; }

        public DateTime LunchIn { get; set; }

        public DateTime LunchOut { get; set; }

        public Double Overtime { get; set; }

        public bool ApprovedOverTime { get; set; }

    }

}
