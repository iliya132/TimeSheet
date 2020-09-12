using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    public class RiskNew
    {
        public int Id { get; set; }
        public int RiskId { get; set; }
        public int TimeSheetTableId { get; set; }

        public virtual Risk Risk { get; set; }
        public virtual TimeSheetTable TimeSheetTable { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is RiskNew && (obj as RiskNew).Id == this.Id)
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
