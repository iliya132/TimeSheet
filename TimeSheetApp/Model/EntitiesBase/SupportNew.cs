using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    public class SupportNew
    {
        public int Id { get; set; }
        public int SupportId { get; set; }
        public int TimeSheetTableId { get; set; }

        public Supports Supports { get; set; }

        public TimeSheetTable TimeSheetTable { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SupportNew && (obj as SupportNew).Id == this.Id)
                return true;
            return false;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
