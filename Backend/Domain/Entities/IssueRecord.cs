using Backend.Domain.Common;

namespace Backend.Domain.Entities
{
    public class IssueRecord : BaseEntity
    {
        public Guid BookId { get; set; } // int se Guid kiya
        public Book Book { get; set; } = null!;

        public Guid UserId { get; set; } // int se Guid kiya
        public User User { get; set; } = null!;

        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; } = false;
    }
}
