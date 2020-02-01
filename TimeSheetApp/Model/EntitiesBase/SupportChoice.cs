using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("SupportChoiceSet")]
    public class SupportChoice
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public Nullable<int> Support_id { get; set; }
        public Nullable<int> Support_id1 { get; set; }
        public Nullable<int> Support_id2 { get; set; }
        public Nullable<int> Support_id3 { get; set; }
        public Nullable<int> Support_id4 { get; set; }
        public Nullable<int> Support_id5 { get; set; }
        public Nullable<int> Support_id6 { get; set; }
        public Nullable<int> Support_id7 { get; set; }
        public Nullable<int> Support_id8 { get; set; }
        public Nullable<int> Support_id9 { get; set; }
        public Nullable<int> Support_id10 { get; set; }
        public Nullable<int> Support_id11 { get; set; }
        public Nullable<int> Support_id12 { get; set; }
        public Nullable<int> Support_id13 { get; set; }
        public Nullable<int> Support_id14 { get; set; }
        public Nullable<int> Support_id15 { get; set; }

        [ForeignKey("Support_id")]
        public virtual Supports Support1 { get; set; }
        [ForeignKey("Support_id1")]
        public virtual Supports Support2 { get; set; }
        [ForeignKey("Support_id2")]
        public virtual Supports Support3 { get; set; }
        [ForeignKey("Support_id3")]
        public virtual Supports Support4 { get; set; }
        [ForeignKey("Support_id4")]
        public virtual Supports Support5 { get; set; }
        [ForeignKey("Support_id5")]
        public virtual Supports Support6 { get; set; }
        [ForeignKey("Support_id6")]
        public virtual Supports Support7 { get; set; }
        [ForeignKey("Support_id7")]
        public virtual Supports Support8 { get; set; }
        [ForeignKey("Support_id8")]
        public virtual Supports Support9 { get; set; }
        [ForeignKey("Support_id9")]
        public virtual Supports Support10 { get; set; }
        [ForeignKey("Support_id10")]
        public virtual Supports Support11 { get; set; }
        [ForeignKey("Support_id11")]
        public virtual Supports Support12 { get; set; }
        [ForeignKey("Support_id12")]
        public virtual Supports Support13 { get; set; }
        [ForeignKey("Support_id13")]
        public virtual Supports Support14 { get; set; }
        [ForeignKey("Support_id14")]
        public virtual Supports Support15 { get; set; }
        [ForeignKey("Support_id15")]
        public virtual Supports Support16 { get; set; }
    }
}
