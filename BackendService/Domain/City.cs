using BackendService.Helper.Helper;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Domain
{
    public class City : EntityBase
    {
        public long ExternalId { get; set; } = 0;
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(20)]
        public string? Code { get; set; }
        [StringLength(100)]
        public string? Province { get; set; }
    }
}
