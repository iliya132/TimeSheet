using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("BusinessBlockChoiceSet")]
    public class BusinessBlockChoice
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("BusinessBlockid")]
        public Nullable<int> BusinessBlock_id { get; set; }
        public Nullable<int> BusinessBlock_id1 { get; set; }
        public Nullable<int> BusinessBlock_id2 { get; set; }
        public Nullable<int> BusinessBlock_id3 { get; set; }
        public Nullable<int> BusinessBlock_id4 { get; set; }
        public Nullable<int> BusinessBlock_id5 { get; set; }
        public Nullable<int> BusinessBlock_id6 { get; set; }
        public Nullable<int> BusinessBlock_id7 { get; set; }
        public Nullable<int> BusinessBlock_id8 { get; set; }
        public Nullable<int> BusinessBlock_id9 { get; set; }
        public Nullable<int> BusinessBlock_id10 { get; set; }
        public Nullable<int> BusinessBlock_id11 { get; set; }
        public Nullable<int> BusinessBlock_id12 { get; set; }
        public Nullable<int> BusinessBlock_id13 { get; set; }
        public Nullable<int> BusinessBlock_id14 { get; set; }
        public Nullable<int> BusinessBlock_id15 { get; set; }
        [ForeignKey("BusinessBlock_id")]
        public virtual BusinessBlock BusinessBlock1 { get; set; }
        [ForeignKey("BusinessBlock_id1")]
        public virtual BusinessBlock BusinessBlock2 { get; set; }
        [ForeignKey("BusinessBlock_id2")]
        public virtual BusinessBlock BusinessBlock3 { get; set; }
        [ForeignKey("BusinessBlock_id3")]
        public virtual BusinessBlock BusinessBlock4 { get; set; }
        [ForeignKey("BusinessBlock_id4")]
        public virtual BusinessBlock BusinessBlock5 { get; set; }
        [ForeignKey("BusinessBlock_id5")]
        public virtual BusinessBlock BusinessBlock6 { get; set; }
        [ForeignKey("BusinessBlock_id6")]
        public virtual BusinessBlock BusinessBlock7 { get; set; }
        [ForeignKey("BusinessBlock_id7")]
        public virtual BusinessBlock BusinessBlock8 { get; set; }
        [ForeignKey("BusinessBlock_id8")]
        public virtual BusinessBlock BusinessBlock9 { get; set; }
        [ForeignKey("BusinessBlock_id9")]
        public virtual BusinessBlock BusinessBlock10 { get; set; }
        [ForeignKey("BusinessBlock_id10")]
        public virtual BusinessBlock BusinessBlock11 { get; set; }
        [ForeignKey("BusinessBlock_id11")]
        public virtual BusinessBlock BusinessBlock12 { get; set; }
        [ForeignKey("BusinessBlock_id12")]
        public virtual BusinessBlock BusinessBlock13 { get; set; }
        [ForeignKey("BusinessBlock_id13")]
        public virtual BusinessBlock BusinessBlock14 { get; set; }
        [ForeignKey("BusinessBlock_id14")]
        public virtual BusinessBlock BusinessBlock15 { get; set; }
        [ForeignKey("BusinessBlock_id15")]
        public virtual BusinessBlock BusinessBlock16 { get; set; }
    }
}
