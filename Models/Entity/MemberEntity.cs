namespace asp_album.Models.Entity
{
    public class MemberEntity
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}