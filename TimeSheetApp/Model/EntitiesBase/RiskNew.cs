using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    [Table("NewRiskSet")]
    public class RiskNew
    {
        public int Id { get; set; }
        public int RiskId { get; set; }
        public int TimeSheetTableId { get; set; }

        [ForeignKey(nameof(RiskId))]
        public virtual Risk Risk { get; set; }
        [ForeignKey(nameof(TimeSheetTableId))]
        public virtual TimeSheetTable TimeSheetTable { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is RiskNew && (obj as RiskNew).Id == this.Id)
            {
                return true;
            }
            return false;
        }
    }
}
