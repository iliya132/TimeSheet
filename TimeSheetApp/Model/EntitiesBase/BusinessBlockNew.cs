using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    public class BusinessBlockNew
    {
        public int Id { get; set; }
        public int BusinessBlockId { get; set; }
        public int TimeSheetTableId { get; set; }

        public virtual BusinessBlock BusinessBlock { get; set; }
        public virtual TimeSheetTable TimeSheetTable { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is BusinessBlockNew && (obj as BusinessBlockNew).Id == this.Id)
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
