using Blog;
using Blog.Server;
using Grpc.Core;

const int PORT = 50052;

Server server = new()
{
    Services = { BlogService.BindService(new BlogServiceImpl()) },
    Ports = { new ServerPort("localhost", PORT, ServerCredentials.Insecure) }
};
server.Start();

Console.WriteLine("Blog server listening on port " + PORT);
Console.WriteLine("Press any key to stop the server...");
Console.ReadKey();

server.ShutdownAsync().Wait();