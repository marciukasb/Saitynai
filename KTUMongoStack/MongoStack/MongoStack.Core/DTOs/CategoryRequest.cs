using MongoStack.Core.Entities;
using ServiceStack.ServiceHost;

namespace MongoStack.Core.DTOs
{
    [Route("/category", "GET")]
    public class GetCategories { }

    [Route("/category/{Id}", "GET")]
    public class GetCategory
    {
        public string Id { get; set; }
    }

    [Route("/category/", "POST")]
    public class AddCategory
    {
        // public Category Category { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }

    [Route("/category/{Id}", "DELETE")]
    public class DeleteCategory
    {
        public string Id { get; set; }
    }

    [Route("/category/", "PUT")]
    public class UpdateCategory
    {
        public Category Category { get; set; }
    }
}
