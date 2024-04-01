using BackendService.Helper.Helper;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Domain
{
    public class MsUser : EntityBase
    {
        [StringLength(100)]
        public string? Fullname { get; set; }
        [StringLength(20)]
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
