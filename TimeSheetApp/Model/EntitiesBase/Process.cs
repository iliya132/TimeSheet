using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("Process")]
    public class Process
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("procName")]
        public string ProcName { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> CommentNeeded { get; set; }
        public int Block_Id { get; set; }
        [Column("SubBlockId")]
        public int SubBlock_Id { get; set; }
        public int ProcessType_Id { get; set; }
        public int Result_Id { get; set; }

        [ForeignKey("Block_Id")]
        [JsonProperty("block")]
        public virtual Block Block { get; set; }
        [ForeignKey("SubBlock_Id")]
        [JsonProperty("subblock")]
        public virtual SubBlock SubBlock { get; set; }
        [ForeignKey("ProcessType_Id")]
        public virtual ProcessType ProcessType { get; set; }
        [ForeignKey("Result_Id")]
        public virtual Result Result1 { get; set; }
        public string CodeFull { 
            get
            {
                return $"{Block_Id}.{SubBlock_Id}.{Id}";
            } 
        }

        public override bool Equals(object obj)
        {
            if(obj is Process && (obj as Process).Id == this.Id)
            {
                return true;
            }
            return false;
        }

    }
}
