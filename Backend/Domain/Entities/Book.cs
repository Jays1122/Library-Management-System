using Backend.Domain.Common;

namespace Backend.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        // Navigation Property: Ek book ke multiple issue records ho sakte hain (history)
        public virtual ICollection<IssueRecord> IssueRecords { get; set; } = new List<IssueRecord>();

    }
}
