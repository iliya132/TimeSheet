using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("ProcessType")]
    public class ProcessType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public string ProcessTypeName { get; set; }
    }
}
