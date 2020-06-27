using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model
{
    public class DashboardContext:DbContext
    {
        public DashboardContext() : base("DashboardDBEntities")
        {

        }

        public DbSet<DashboardRecord> DashboardRecords { get; set; }
        public DbSet<DashboardUnit> DashboardUnits { get; set; }
    }
}
