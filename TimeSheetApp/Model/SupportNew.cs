using System.ComponentModel.DataAnnotations.Schema;

using TimeSheetApp.Model.EntitiesBase;

namespace TimeSheetApp.Model
{
    [Table("NewSupportsSet")]
    public class SupportNew
    {
        public int Id { get; set; }
        public int SupportId { get; set; }
        public int TimeSheetTableId { get; set; }

        [ForeignKey(nameof(SupportId))]
        public Supports Supports { get; set; }

        [ForeignKey(nameof(TimeSheetTableId))]
        public TimeSheetTable TimeSheetTable { get; set; }
    }
}
