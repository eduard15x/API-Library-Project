using AutoMapper;
using ITPLibrary.Api.Core.Dtos.Book;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;

namespace ITPLibrary.Api.Core.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;

        public BookService(IMapper mapper, IBookRepository bookRepository)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
        }

        public async Task<List<GetBookDto>> GetAllBooks()
        {
            var bookList =  await _bookRepository.GetAllBooks();
            return _mapper.Map<List<GetBookDto>>(bookList);
        }

        public async Task<GetBookDto> GetSingleBook(int id)
        {
            var singleBook = await _bookRepository.GetSingleBook(id);
            return _mapper.Map<GetBookDto>(singleBook);
        }

        public async Task<List<GetBookDto>> AddBook(AddBookDto newBook)
        {
            var book = _mapper.Map<Book>(newBook);
            //book.Id = books.Max(c => c.Id) + 1; // increasing the ID will be made automatically be SQL Server
            await _bookRepository.AddBook(book);

            var bookList = await _bookRepository.GetAllBooks();

            return _mapper.Map<List<GetBookDto>>(bookList);
        }

        public async Task<GetBookDto> UpdateBook(UpdateBookDto updatedBook)
        {
            var bookEntity = _mapper.Map<Book>(updatedBook);
            var updatedEntity = await _bookRepository.UpdateBook(bookEntity);

            return _mapper.Map<GetBookDto>(updatedEntity);
        }

        public async Task<List<GetBookDto>> DeleteBook(int id)
        {
            var book = await _bookRepository.DeleteBook(id);

            if (book == null)
            {
                throw new Exception($"Book with Id '{id}' was not found.");
            }

            return  _mapper.Map<List<GetBookDto>>(book);
        }

        public async Task<List<PromotedBookDto>> GetPromotedBooks()
        {
            var promotedBookList = await _bookRepository.GetPromotedBooks();
            if (promotedBookList == null)
            {
                return null!;
            }

            return _mapper.Map<List<PromotedBookDto>>(promotedBookList);
        }

        public async Task<List<BestBookDto>> GetBestAndRecentlyAddedBooks(int lastDaysNumber)
        {
            if (lastDaysNumber < 0)
            {
                throw new Exception($"Number of the last days shouldn't be less than 0.");
            }

            var promotedBookList = await _bookRepository.GetBestAndRecentlyAddedBooks(lastDaysNumber);
            if (promotedBookList == null)
            {
                return null!;
            }

            return _mapper.Map<List<BestBookDto>>(promotedBookList);
        }
    }
}
