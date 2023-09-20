using ITPLibrary.Api.Core.Dtos.Book;

namespace ITPLibrary.Api.Core.Services.BookService
{
    public interface IBookService
    {
        Task<List<GetBookDto>> GetAllBooks();
        Task<GetBookDto> GetSingleBook(int id);
        Task<List<GetBookDto>> AddBook(AddBookDto newBook);
        Task<GetBookDto> UpdateBook(UpdateBookDto updatedBook);
        Task<List<GetBookDto>> DeleteBook(int id);
        Task<List<PromotedBookDto>> GetPromotedBooks();
        Task<List<BestBookDto>> GetBestAndRecentlyAddedBooks(int lastDaysNumber);
    }
}
