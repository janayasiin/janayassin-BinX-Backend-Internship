using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstApi.Data;
using MyFirstApi.DTOs;
using MyFirstApi.Models;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookRequest request)
        {
            var book = new Book
            {
                Title = request.Title,
                ISBN = request.ISBN,
                Price = request.Price,
                AuthorId = request.AuthorId,
                CategoryId = request.CategoryId
            };

            _context.Books.Add(book);

            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetById),
                new { id = book.Id },
                book
            );
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _context.Books.ToListAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [Authorize(Policy = "CanManageBooks")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookRequest request)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            book.Title = request.Title;
            book.ISBN = request.ISBN;
            book.Price = request.Price;
            book.AuthorId = request.AuthorId;
            book.CategoryId = request.CategoryId;

            await _context.SaveChangesAsync();

            return Ok(book);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
