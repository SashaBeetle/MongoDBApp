using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBApp.Domain.Entities;

namespace MongoDBApp.Pages
{
    public class AllFilmsModel : PageModel
    {
        private readonly IMongoDatabase _database;

        public AllFilmsModel(IMongoDatabase database)
        {
            _database = database;
        }

        public List<Film> films { get; set; }

        public async Task OnGet()
        {
            var collection = _database.GetCollection<Film>("Films");
            films = await collection.Find(_ => true).ToListAsync();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
          
                // Створіть з'єднання з колекцією MongoDB
                var collection = _database.GetCollection<Film>("Films"); // Замість "Film" вкажіть назву вашої колекції

                // Створіть фільтр для знаходження об'єкта за id
                var filter = Builders<Film>.Filter.Eq("_id", id);

                // Видаліть об'єкт з MongoDB
                await collection.DeleteOneAsync(filter);

            
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChange(int id)
        {

            var collection = _database.GetCollection<Film>("Films"); 

            var filter = Builders<Film>.Filter.Eq("_id", id);

            var currentFilm = await collection.Find(filter).FirstOrDefaultAsync();

            bool currentValue = currentFilm.review; 

            bool invertedValue = !currentValue;

            var update = Builders<Film>.Update.Set("review", invertedValue);

            await collection.UpdateOneAsync(filter, update);




            return RedirectToPage();
        }
    }
}
