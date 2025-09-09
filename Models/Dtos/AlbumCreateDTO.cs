using System.ComponentModel.DataAnnotations;

namespace asp_album.Models.Dtos
{
    public class AlbumCreateDTO
    {
        [Display(Name = "類別名稱")]
        [Required(ErrorMessage = "必填")]
        public int? CategoryId { get; set; }
        [Display(Name = "相片標題")]
        [Required(ErrorMessage = "必填")]
        public string? Title { get; set; }
        [Display(Name = "相片描述")]
        [Required(ErrorMessage = "必填")]
        public string? Description { get; set; }
        [Display(Name = "相片檔案")]
        [Required]
        public IFormFile Album { get; set; }
    }
}