using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("Result")]
    public class Result
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("resultName")]
        public string ResultName { get; set; }
    }
}
