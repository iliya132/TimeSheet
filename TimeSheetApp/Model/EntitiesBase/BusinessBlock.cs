namespace TimeSheetApp.Model.EntitiesBase
{
    public class BusinessBlock
    {
        public int Id { get; set; }
        public string BusinessBlockName { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is BusinessBlock && (obj as BusinessBlock).Id == this.Id)
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
