using System.ComponentModel.DataAnnotations;

namespace MongoDBApp.Domain.Entities
{
    public class CreateFilmRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool review { get; set; }
    }
}
