using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    public class EscalationNew
    {
        public int Id { get; set; }
        public int EscalationId { get; set; }
        public int TimeSheetTableId { get; set; }

        public virtual Escalation Escalation { get; set; }
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
