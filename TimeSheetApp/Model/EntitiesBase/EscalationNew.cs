﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    [Table("NewEscalations")]
    public class EscalationNew
    {
        public int Id { get; set; }
        public int EscalationId { get; set; }
        public int TimeSheetTableId { get; set; }

        [ForeignKey(nameof(EscalationId))]
        public virtual Escalation Escalation { get; set; }
        [ForeignKey(nameof(TimeSheetTableId))]
        public virtual TimeSheetTable TimeSheetTable { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is EscalationNew && (obj as EscalationNew).Id == this.Id)
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
