namespace ITPLibrary.Api.Data.Shared.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = "No title";
        public string Description { get; set; } = "No description";
        public string Image { get; set; } = "https://images.unsplash.com/photo-1621944193575-816edc981878?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1974&q=80";
        public double Price { get; set; } = 50.0;
        public string Author { get; set; } = string.Empty;
        public bool Promoted { get; set; }
        public bool Popular { get; set; }
        public string Thumbnail { get; set; } = string.Empty;
        public DateTime RecentlyAdded { get; set; } = DateTime.Now;
    }
}
