namespace ITPLibrary.Api.Core.Dtos.Book
{
    public class BestBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Author { get; set; }
        public bool Popular { get; set; }
        public string Thumbnail { get; set; }
        public DateTime RecentlyAdded { get; set; }
    }
}
