using System.ComponentModel.DataAnnotations;

namespace asp_album.Models.Dtos
{
    public class MemberCreateDTO
    {

        [Display(Name = "使用者Uid")]
        [StringLength(50)]
        [Required]
        public string Uid { get; set; }

        [Display(Name = "密碼")]
        [StringLength(20)]
        [Required]
        public string Password { get; set; }

        [Display(Name = "使用者名稱")]
        [StringLength(20)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
        public string Mail { get; set; }
    }
}