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
        public int? HeadAdmId { get; set; }
        public int? HeadFuncId { get; set; }
        public bool? Deleted_Flag { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Departments Departments { get; set; }
        [ForeignKey(nameof(DirectionId))]
        public virtual Directions Directions { get; set; }
        [ForeignKey(nameof(UpravlenieId))]
        public virtual Upravlenie Upravlenie { get; set; }
        [ForeignKey(nameof(OtdelId))]
        public virtual Otdel Otdel { get; set; }
        [ForeignKey(nameof(PositionsId))]
        public virtual Positions Positions{get;set;}
        [ForeignKey(nameof(RoleTableId))]
        public virtual Role Role { get; set; }
        public virtual Analytic AdminHead { get; set; }
        public virtual Analytic FunctionHead { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {FatherName}";
        }

        
    }
}
