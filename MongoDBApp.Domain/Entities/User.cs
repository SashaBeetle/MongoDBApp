namespace MongoDBApp.Domain.Entities
{
    public class User : Dbitem
    {
        public string username { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }
}
