using System.Data.Entity;
using SubmitBug.Models;

namespace SubmitBug.DAL
{
    public class SubBugEntities:DbContext
    {
        public DbSet<TB_LoginOn> TB_LoginOn { get; set; }
        public DbSet<TB_BugSubmit> TB_BugSubmit { get; set; }

        public DbSet<TB_UserAccess> TB_UserAccess { get; set; }
        public DbSet<TB_Follow> TB_Follow { get; set; }

        public DbSet<TB_Role> TB_Role { get; set; }

        public DbSet<TB_RoleAction> TB_RoleAction { get; set; }

        public DbSet<TB_Users> TB_Users { get; set; }
        public DbSet<TB_Schedule> TB_Schedule { get; set; }

        public DbSet<TB_ShowMenu> TB_ShowMenu { get; set; }
        public DbSet<TB_Difficultity> TB_Difficultiy { get; set; }

        public SubBugEntities():base("SubBugEntities")
        {

        }

        public static SubBugEntities Create()
        {
            return new SubBugEntities();
        }

        public System.Data.Entity.DbSet<SubmitBug.Models.TB_UserFace> TB_UserFace { get; set; }

        //public System.Data.Entity.DbSet<SubmitBug.Models.TB_Schedule> TB_Schedule { get; set; }
    }
}