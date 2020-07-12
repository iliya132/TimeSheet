using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("Analytic")]
    public class Analytic
    {
        [Key]
        public int Id { get; set; }
        [Column("userName")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        [Column("DepartmentsId")]
        public int DepartmentId { get; set; }
        [Column("DirectionsId")]
        public int DirectionId { get; set; }
        [Column("UpravlenieTableId")]
        public int UpravlenieId { get; set; }
        [Column("OtdelTableId")]
        public int OtdelId { get; set; }
        public int PositionsId { get; set; }
        public int RoleTableId { get; set; }
        public bool? Deleted_Flag { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Departments Departments { get; set; }
        [ForeignKey("DirectionId")]
        public virtual Directions Directions { get; set; }
        [ForeignKey("UpravlenieId")]
        public virtual Upravlenie Upravlenie { get; set; }
        [ForeignKey("OtdelId")]
        public virtual Otdel Otdel { get; set; }
        [ForeignKey("PositionsId")]
        public virtual Positions Positions{get;set;}
        [ForeignKey("RoleTableId")]
        public virtual Role Role { get; set; }

    }
}
