using Dapper;
using ITPLibrary.Api.Data.Shared.Entities;
using ITPLibrary.Api.Data.Shared.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ITPLibrary.Api.Data.SQL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnection _connectionString;

        public BookRepository(IDbConnection connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<Book>> GetAllBooks()
        {
            var query = "SELECT * FROM Books";

            return (await _connectionString.QueryAsync<Book>(query)).ToList();
        }

        public async Task<Book> GetSingleBook(int id)
        {
            var query = "SELECT * FROM Books WHERE Id = @Id";
            return await _connectionString.QueryFirstOrDefaultAsync<Book>(query, new { Id = id });
        }

        public async Task<List<Book>> AddBook(Book newBook)
        {
            var insertQuery = @"
                INSERT INTO Books (Title, Author, Description, Price, Image, Promoted, Popular, Thumbnail, RecentlyAdded)
                VALUES (@Title, @Author, @Description, @Price, @Image, @Promoted, @Popular, @Thumbnail, @RecentlyAdded);
                SELECT CAST(SCOPE_IDENTITY() AS int)";

            await _connectionString.ExecuteAsync(insertQuery, newBook);

            var selectQuery = "SELECT * FROM Books";
            var updatedBookList = await _connectionString.QueryAsync<Book>(selectQuery);

            return updatedBookList.ToList();

        }

        public async Task<Book> UpdateBook(Book updatedBook)
        {
            var updateQuery = @"
                UPDATE Books
                SET Title = @Title, Author = @Author, Description = @Description, Price = @Price, Image = @Image, Promoted = @Promoted, Popular = @Popular, Thumbnail = @Thumbnail
                WHERE Id = @Id";

            await _connectionString.ExecuteAsync(updateQuery, updatedBook);

            var selectQuery = "SELECT * FROM Books WHERE Id = @Id";
            return await _connectionString.QueryFirstOrDefaultAsync<Book>(selectQuery, new { updatedBook.Id });
        }

        public async Task<List<Book>> DeleteBook(int id)
        {
            var deleteQuery = "DELETE FROM Books WHERE Id = @Id";
            await _connectionString.ExecuteAsync(deleteQuery, new { Id = id });

            var selectQuery = "SELECT * FROM Books";
            var updatedBookList = await _connectionString.QueryAsync<Book>(selectQuery);

            return updatedBookList.ToList();
        }

        public async Task<List<Book>> GetPromotedBooks()
        {
            var query = "SELECT * FROM Books Where Promoted = 1";

            return (await _connectionString.QueryAsync<Book>(query)).ToList();
        }

        public async Task<List<Book>> GetBestAndRecentlyAddedBooks(int lastDaysNumber)
        {
            var dateThreshold = DateTime.Now.AddDays(-lastDaysNumber);

            // Define the SQL query
            string sql = @"
                SELECT *
                FROM Books
                WHERE Popular = 1 AND RecentlyAdded >= @dateThreshold";

            var books = await _connectionString.QueryAsync<Book>(sql, new { dateThreshold });
            return books.ToList();
        }
    }
}
