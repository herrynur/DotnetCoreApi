using BackendService.Helper.Helper;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Domain
{
    public class Company : EntityBase
    {
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(20)]
        public string? Code { get; set; }
        [StringLength(100)]
        public string? PIC { get; set; }
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
