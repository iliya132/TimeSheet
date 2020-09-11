using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("Supports")]
    public class Supports
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Supports && (obj as Supports).Id == this.Id)
            {
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
