using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_album.Models.Entity
{
    public class AlbumEntity
    {

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? MemberId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImgName { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}