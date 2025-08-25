namespace InventoryManagement.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public string AuthorId { get; set; } = "";
        public string Markdown { get; set; } = "";
    }
}
