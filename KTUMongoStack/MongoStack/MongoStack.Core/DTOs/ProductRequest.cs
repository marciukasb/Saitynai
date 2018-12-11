using ServiceStack.ServiceHost;
namespace MongoStack.Core.DTOs
{
    [Route("/product/{Id}", "GET")]
    public class GetProduct : IReturn<EmptyResponse>
    {
        public string Id { get; set; }
    }

    [Route("/product/", "GET")]
    public class GetProducts  { } 

    [Route("/product/", "POST")]
    public class AddProduct : IReturn<EmptyResponse>
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
    }

    [Route("/product/", "DELETE")]
    [Route("/product/{Id}", "DELETE")]
    public class DeleteProduct : IReturn<EmptyResponse>
    {
        public string Id { get; set; }
    }

    [Route("/product/", "PUT")]
    [Route("/product/{Id}", "PUT")]
    public class UpdateProduct : IReturn<EmptyResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
    }
    public class EmptyResponse { }
}
