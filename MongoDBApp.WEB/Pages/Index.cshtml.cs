using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using MongoDBApp.Domain.Entities;

namespace MongoDBApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMongoDatabase _database;
        

        public IndexModel(IMongoDatabase database)
        {
            _database = database;
        }

        [BindProperty]
        public Film film { get; set; }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost()
        {
            var rnd = new Random();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            film.Id = rnd.Next(0, 100000000);
            film.review = false;

            var collection = _database.GetCollection<Film>("Films");
            await collection.InsertOneAsync(film);
            return RedirectToPage("AllFilms"); // Перенаправлення на іншу сторінку після додавання продукту
        }
    }
}