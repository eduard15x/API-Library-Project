using ITPLibrary.Api.Data.Shared.Entities;

namespace ITPLibrary.Api.Data.Shared.IRepository
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooks();
        Task<Book> GetSingleBook(int id);
        Task<List<Book>> AddBook(Book newBook);
        Task<Book> UpdateBook(Book updatedBook);
        Task<List<Book>> DeleteBook(int id);
        Task<List<Book>> GetPromotedBooks();
        Task<List<Book>> GetBestAndRecentlyAddedBooks(int lastDaysNumber);
    }
}
