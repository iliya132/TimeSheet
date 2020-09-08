using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model
{
    [Table("Unit")]
    public class DashboardUnit
    {
        public int Id { get; set; }
        public int Department_Id { get; set; }
        public int Direction_Id { get; set; }
        public int Upravlenie_Id { get; set; }
        public int Otdel_Id { get; set; }
        public string ShortName { get; set; }
    }
}
