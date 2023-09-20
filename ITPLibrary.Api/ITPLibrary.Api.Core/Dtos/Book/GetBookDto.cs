namespace ITPLibrary.Api.Core.Dtos.Book
{
    public class GetBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "No title";
        public string Author { get; set; } = "No name";
        public string Description { get; set; } = "No description";
        public double Price { get; set; } = 50.0;
        public string Image { get; set; } = "https://images.unsplash.com/photo-1621944193575-816edc981878?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1974&q=80";
        public bool Promoted { get; set; }
        public bool Popular { get; set; }
        public string Thumbnail { get; set; } = string.Empty;
        public DateTime RecentlyAdded { get; set; }
    }
}
