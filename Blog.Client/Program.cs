using Grpc.Net.Client;
using static Blog.BlogService;

using var channel = GrpcChannel.ForAddress("http://localhost:50052");

await channel.ConnectAsync();
Console.WriteLine("The client is connected");

var client = new BlogServiceClient(channel);
var response = client.CreateBlog(new Blog.CreateBlogRequest
{
    Blog = new()
    {
        AuthId = 1,
        Content = "This is the first blog from gRPC client",
        Title = "Blog from gRPC"
    }
});

Console.WriteLine("Blog with id: {0} has been created", response.Blog.Id);

Console.WriteLine("Press any key to exit...");
Console.ReadKey();

await channel.ShutdownAsync();