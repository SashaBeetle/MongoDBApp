namespace MongoDBApp.Domain.Entities
{
    public class Film : Dbitem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool review { get; set; }
    }
}
