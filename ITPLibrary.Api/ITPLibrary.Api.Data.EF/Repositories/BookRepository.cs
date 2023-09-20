using ITPLibrary.Api.Data.EF.Data;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ITPLibrary.Api.Data.EF.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetSingleBook(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Book>> AddBook(Book newBook)
        {
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return await _context.Books.ToListAsync();
        }

        public async Task<Book> UpdateBook(Book updatedBook)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(c => c.Id == updatedBook.Id);

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Description = updatedBook.Description;
            existingBook.Price = updatedBook.Price;
            existingBook.Image = updatedBook.Image;
            existingBook.Popular = updatedBook.Popular;
            existingBook.Promoted = updatedBook.Promoted;
            existingBook.Thumbnail = updatedBook.Thumbnail;

            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task<List<Book>> DeleteBook(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(c => c.Id == id);

            if (book == null)
            {
                throw new Exception($"Book with Id '{book.Id}' was not found.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return await _context.Books.ToListAsync();
        }

        public async Task<List<Book>> GetPromotedBooks()
        {
            return await _context.Books
                .Where(c => c.Promoted)
                .ToListAsync();
        }

        public async Task<List<Book>> GetBestAndRecentlyAddedBooks(int lastDaysNumber)
        {
            var dateTreshold = DateTime.Now.AddDays(-lastDaysNumber);

            return await _context.Books
                .Where(c => c.Popular && c.RecentlyAdded >= dateTreshold)
                .ToListAsync();
        }
    }
}
