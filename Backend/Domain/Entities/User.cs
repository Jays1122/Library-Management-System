using Backend.Domain.Common;

namespace Backend.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Member";
        // Navigation Property: Ek user multiple books issue kar sakta hai
        public virtual ICollection<IssueRecord> IssueRecords { get; set; } = new List<IssueRecord>();
    }
}
