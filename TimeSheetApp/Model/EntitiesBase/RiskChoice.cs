using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("riskChoise")]
    public class RiskChoice
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public Nullable<int> Risk_id { get; set; }
        public Nullable<int> Risk_id1 { get; set; }
        public Nullable<int> Risk_id2 { get; set; }
        public Nullable<int> Risk_id3 { get; set; }
        public Nullable<int> Risk_id4 { get; set; }
        public Nullable<int> Risk_id5 { get; set; }
        public Nullable<int> Risk_id6 { get; set; }
        public Nullable<int> Risk_id7 { get; set; }
        public Nullable<int> Risk_id8 { get; set; }
        public Nullable<int> Risk_id9 { get; set; }

        [ForeignKey("Risk_id")]
        public virtual Risk Risk1 { get; set; }
        [ForeignKey("Risk_id1")]
        public virtual Risk Risk2 { get; set; }
        [ForeignKey("Risk_id2")]
        public virtual Risk Risk3 { get; set; }
        [ForeignKey("Risk_id3")]
        public virtual Risk Risk4 { get; set; }
        [ForeignKey("Risk_id4")]
        public virtual Risk Risk5 { get; set; }
        [ForeignKey("Risk_id5")]
        public virtual Risk Risk6 { get; set; }
        [ForeignKey("Risk_id6")]
        public virtual Risk Risk7 { get; set; }
        [ForeignKey("Risk_id7")]
        public virtual Risk Risk8 { get; set; }
        [ForeignKey("Risk_id8")]
        public virtual Risk Risk9 { get; set; }
        [ForeignKey("Risk_id9")]
        public virtual Risk Risk10 { get; set; }
    }
}
