namespace TimeSheetApp.Model.EntitiesBase
{
    public class ClientWays
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is ClientWays && (obj as ClientWays).Id == this.Id)
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
