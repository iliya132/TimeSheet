namespace TimeSheetApp.Model.EntitiesBase
{
    public class Escalation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Escalation && (obj as Escalation).Id == this.Id)
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
