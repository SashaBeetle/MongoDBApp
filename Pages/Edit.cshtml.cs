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
                // ��'��� � �������� id �� ��������, �� ������ ����� ��������� 䳿 ���
                return NotFound();
            }

            // ������� ����� ��'��� CreateFilmRequest �� ��������� ���� ��������
            var updatedFilm = new CreateFilmRequest
            {
                Title = currentFilm.Title,
                Description = currentFilm.Description,
                // ���� ����������, �� �� ������ �������
            };

            // ������ �������� ������������ �� ����� ������� ����� Ownfilm
            updatedFilm.Title = Ownfilm.Title;
            updatedFilm.Description = Ownfilm.Description;

            // ������� ������ ��� ����������� ��'���� �� id
            filter = Builders<Film>.Filter.Eq("_id", id);

            // ������� ��������� ��� ���� �������� ���� �� ��� ��������
            var update = Builders<Film>.Update
                .Set("Title", updatedFilm.Title)
                .Set("Description", updatedFilm.Description);

            // ��������� ��������� ����������
            await collection.UpdateOneAsync(filter, update);
            return RedirectToAction("Index");
        

    }
    }
}
