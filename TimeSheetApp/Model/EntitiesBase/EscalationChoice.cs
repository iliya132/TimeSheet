using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("EscalationChoiceSet")]
    public class EscalationChoice
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public Nullable<int> Escalation_id { get; set; }
        public Nullable<int> Escalation_id1 { get; set; }
        public Nullable<int> Escalation_id2 { get; set; }
        public Nullable<int> Escalation_id3 { get; set; }
        public Nullable<int> Escalation_id4 { get; set; }
        public Nullable<int> Escalation_id5 { get; set; }
        public Nullable<int> Escalation_id6 { get; set; }
        public Nullable<int> Escalation_id7 { get; set; }
        public Nullable<int> Escalation_id8 { get; set; }
        public Nullable<int> Escalation_id9 { get; set; }
        public Nullable<int> Escalation_id10 { get; set; }
        public Nullable<int> Escalation_id11 { get; set; }
        public Nullable<int> Escalation_id12 { get; set; }
        public Nullable<int> Escalation_id13 { get; set; }
        public Nullable<int> Escalation_id14 { get; set; }
        public Nullable<int> Escalation_id15 { get; set; }
        [ForeignKey("Escalation_id")]
        public virtual Escalation Escalation1 { get; set; }
        [ForeignKey("Escalation_id1")]
        public virtual Escalation Escalation2 { get; set; }
        [ForeignKey("Escalation_id2")]
        public virtual Escalation Escalation3 { get; set; }
        [ForeignKey("Escalation_id3")]
        public virtual Escalation Escalation4 { get; set; }
        [ForeignKey("Escalation_id4")]
        public virtual Escalation Escalation5 { get; set; }
        [ForeignKey("Escalation_id5")]
        public virtual Escalation Escalation6 { get; set; }
        [ForeignKey("Escalation_id6")]
        public virtual Escalation Escalation7 { get; set; }
        [ForeignKey("Escalation_id7")]
        public virtual Escalation Escalation8 { get; set; }
        [ForeignKey("Escalation_id8")]
        public virtual Escalation Escalation9 { get; set; }
        [ForeignKey("Escalation_id9")]
        public virtual Escalation Escalation10 { get; set; }
        [ForeignKey("Escalation_id10")]
        public virtual Escalation Escalation11 { get; set; }
        [ForeignKey("Escalation_id11")]
        public virtual Escalation Escalation12 { get; set; }
        [ForeignKey("Escalation_id12")]
        public virtual Escalation Escalation13 { get; set; }
        [ForeignKey("Escalation_id13")]
        public virtual Escalation Escalation14 { get; set; }
        [ForeignKey("Escalation_id14")]
        public virtual Escalation Escalation15 { get; set; }
        [ForeignKey("Escalation_id15")]
        public virtual Escalation Escalation16 { get; set; }
    }
}
