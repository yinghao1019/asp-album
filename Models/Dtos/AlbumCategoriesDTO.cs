using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace asp_album.Models.Dtos
{
    public class AlbumCategoriesDTO
    {
        [Display(Name = "類別編號")]
        public int Id { get; set; }
        [Display(Name = "類別名稱")]
        [Required(ErrorMessage = "必填")]
        public string? Name { get; set; }
    }
}