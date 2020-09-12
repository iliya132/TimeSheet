using System;

namespace TimeSheetApp.Model.EntitiesBase
{
    public class Process
    {
        public int Id { get; set; }
        public string ProcName { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> CommentNeeded { get; set; }
        public int Block_Id { get; set; }
        public int SubBlock_Id { get; set; }
        public int ProcessType_Id { get; set; }
        public int Result_Id { get; set; }
        public virtual Block Block { get; set; }
        public virtual SubBlock SubBlock { get; set; }
        public virtual ProcessType ProcessType { get; set; }
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
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
