using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_album.Models.Dtos
{
    public class AlbumQueryDTO
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public string? MemberName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? AlbumName { get; set; }
        public DateTime? ReleaseTime { get; set; }
    }
}