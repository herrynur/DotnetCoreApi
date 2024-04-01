using System.ComponentModel.DataAnnotations;

namespace BackendService.Helper.Helper
{
    public class EntityBase
    {
        public EntityBase()
        {
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
            IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
