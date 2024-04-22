using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDBApp.Domain.Entities;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class IndexController : ControllerBase
{
    private readonly IMongoDatabase _database;

    public IndexController(IMongoDatabase database)
    {
        _database = database;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(Film book)
    {
        var collection = _database.GetCollection<Film>("Products");
        await collection.InsertOneAsync(book);
        return Ok("Продукт додано до бази даних");
    }
}

