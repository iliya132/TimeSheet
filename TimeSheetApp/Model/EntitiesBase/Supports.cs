namespace TimeSheetApp.Model.EntitiesBase
{
    public class Supports
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Supports && (obj as Supports).Id == this.Id)
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
