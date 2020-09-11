using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("Escalations")]
    public class Escalation
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Escalation && (obj as Escalation).Id == this.Id)
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
