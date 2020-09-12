namespace TimeSheetApp.Model.EntitiesBase
{
    public class Risk
    {
        public int Id { get; set; }
        public string RiskName { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Risk && (obj as Risk).Id == this.Id)
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
