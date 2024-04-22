using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBApp.Domain.Entities;

namespace MongoDBApp.Pages
{
    public class EditModel : PageModel
    {

        private readonly IMongoDatabase _database;

        public EditModel(IMongoDatabase database)
        {
            _database = database;
        }
        public Film film { get; set; }
        public Film Ownfilm { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var collection = _database.GetCollection<Film>("Films");

            var filter = Builders<Film>.Filter.Eq("_id", id);

            film = await collection.Find(filter).FirstOrDefaultAsync();

            return Page();

        }
        public async Task<IActionResult> OnPost(int id)
        {


            var collection = _database.GetCollection<Film>("Films");
            var filter = Builders<Film>.Filter.Eq("_id", id);
            var currentFilm = await collection.Find(filter).FirstOrDefaultAsync();

            if (currentFilm == null)
            {
                // Об'єкт з вказаним id не знайдено, ви можете взяти відповідний дії тут
                return NotFound();
            }

            // Створіть новий об'єкт CreateFilmRequest та присвойте йому значення
            var updatedFilm = new CreateFilmRequest
            {
                Title = currentFilm.Title,
                Description = currentFilm.Description,
                // Інші властивості, які ви хочете оновити
            };

            // Оновіть значення властивостей на основі вхідних даних Ownfilm
            updatedFilm.Title = Ownfilm.Title;
            updatedFilm.Description = Ownfilm.Description;

            // Створіть фільтр для знаходження об'єкта за id
            filter = Builders<Film>.Filter.Eq("_id", id);

            // Створіть оновлення для зміни значення полів на нові значення
            var update = Builders<Film>.Update
                .Set("Title", updatedFilm.Title)
                .Set("Description", updatedFilm.Description);

            // Виконайте оновлення асинхронно
            await collection.UpdateOneAsync(filter, update);
            return RedirectToAction("Index");
        

    }
    }
}
