using Microsoft.EntityFrameworkCore;
using CBS.Data.Entities;
using CBS.Data.Repository.Interface;

namespace CBS.Data.Context
{
    public class CBSContext : DbContext, IContext
    {

        public CBSContext(DbContextOptions<CBSContext> options) : base(options)
        {

        }

        public virtual DbSet<CBS_Application> CBS_Application { get; set; }
        public virtual DbSet<CBS_Department> CBS_Department { get; set; }
        public virtual DbSet<CBS_DepartmentPlatforms> CBS_DepartmentPlatforms { get; set; }
        public virtual DbSet<CBS_DepartmentPlatformsType> CBS_DepartmentPlatformsType { get; set; }
        public virtual DbSet<CBS_RolesExtention> CBS_RolesExtention { get; set; }
        public virtual DbSet<CBS_UserPlatform> CBS_UserPlatform { get; set; }
        public virtual DbSet<CBS_Users> CBS_Users { get; set; }
        public virtual DbSet<CBS_UsersApps> CBS_UsersApps { get; set; }
        public virtual DbSet<CBS_StaffDailySchedule> CBS_StaffDailySchedule { get; set; }
        

    }
    
}
