
using System.ComponentModel.DataAnnotations;

namespace asp_album.Models.Dtos
{
    public class MemberEditDTO
    {

        public string Uid { get; set; }
        [Display(Name = "使用者名稱")]
        [StringLength(20)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
        public string Mail { get; set; }
        [Display(Name = "權限")]
        [Required]
        public string Role { get; set; }
    }
}