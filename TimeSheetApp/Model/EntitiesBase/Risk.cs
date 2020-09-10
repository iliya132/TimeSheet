using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("RiskSet")]
    public class Risk
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("riskName")]
        public string RiskName { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Risk && (obj as Risk).Id == this.Id)
            {
                return true;
            }
            return false;
        }
    }
}
