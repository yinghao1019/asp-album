using System.ComponentModel.DataAnnotations;

namespace asp_album.Models.Dtos
{
    public class MemberLoginDTO
    {
        [Display(Name = "使用者Uid")]
        [StringLength(50)]
        [Required]
        public string Uid { get; set; }

        [Display(Name = "密碼")]
        [StringLength(20)]
        [Required]
        public string Password { get; set; }
    }
}