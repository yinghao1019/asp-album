using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_album.Models.Dtos
{
    public class MemberQueryDTO
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}