using Bookstore.Web.Data;
using Bookstore.Web.Entities;
using Bookstore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Web.Controllers;

[Authorize(Roles = "Author")]
public class BooksController(ApplicationDbContext context, IWebHostEnvironment environment) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<Book> books = await context.Books
            .Include(b => b.Authors)
            .OrderBy(b => b.Title)
            .ToListAsync();

        return View(books);
    }

    public IActionResult Show(int id)
    {
        return View();
    }

    public async Task<IActionResult> New([FromQuery] int category)
    {
        await PopulateSelectFields(category);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewBookRequest bookRequest)
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectFields(bookRequest.CategoryId);
            return View("New", bookRequest);
        }

        if (bookRequest.Image != null)
        {
            string[] allowedExtensions = [".jpg", ".png", ".jpeg", ".webp"]; // !!! TEM QUE POR O PONTO ANTES DA EXTENSÃO !!!
            string extension = Path.GetExtension(bookRequest.Image.FileName).ToLower(); // Obtém extensão do arquivo

            // Verifica se a extensão do arquivo é permitida (se existe no array que criamos)
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("Image", "Tipo de arquivo inválido");
                await PopulateSelectFields(bookRequest.CategoryId);
                return View("New", bookRequest);
            }

            // Obtém o caminho da pasta /wwwroot/uploads
            string uploadFolder = Path.Combine(environment.WebRootPath, "uploads");
            // Concatena o caminho da pasta com o nome do arquivo para obter o caminho completo
            string filePath = Path.Combine(uploadFolder, bookRequest.Image.FileName);

            // Instancia o FileStream para criar o arquivo no caminho especificado
            await using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Copia o conteúdo do arquivo enviado para o "target" (FileStream), que irá salvar o arquivo no servidor
                await bookRequest.Image.CopyToAsync(fileStream);
            }
        }

        Book book = new Book()
        {
            Title = bookRequest.Title,
            Description = bookRequest.Description,
            Isbn = bookRequest.Isbn,
            Price = bookRequest.Price,
            Format = bookRequest.Format,
            CategoryId = bookRequest.CategoryId,
            PublisherId = bookRequest.PublisherId,
            CoverImage = bookRequest.Image?.FileName, // Salva o nome do arquivo
            Authors = await context.Authors
                .Where(a => bookRequest.AuthorIds.Contains(a.Id))
                .ToListAsync()
        };

        context.Books.Add(book);
        await context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    private async Task PopulateSelectFields(int category)
    {
        List<Category> categories = await context.Categories.OrderBy(c => c.Name).ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", category);

        List<Publisher> publishers = await context.Publishers.OrderBy(p => p.Name).ToListAsync();
        ViewBag.Publishers = new SelectList(publishers, "Id", "Name");

        List<Author> authors = await context.Authors.OrderBy(a => a.FullName).ToListAsync();
        ViewBag.Authors = new MultiSelectList(authors, "Id", "FullName");
    }
}
