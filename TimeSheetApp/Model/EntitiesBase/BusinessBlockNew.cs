using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    [Table("NewBusinessBlockSet")]
    public class BusinessBlockNew
    {
        public int Id { get; set; }
        public int BusinessBlockId { get; set; }
        public int TimeSheetTableId { get; set; }

        [ForeignKey(nameof(BusinessBlockId))]
        public virtual BusinessBlock BusinessBlock { get; set; }
        [ForeignKey(nameof(TimeSheetTableId))]
        public virtual TimeSheetTable TimeSheetTable { get; set; }
    }
}
