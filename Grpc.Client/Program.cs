using Grpc.Net.Client;
using HelloWorld;

await BiDirectionalClientCalculator();

async static Task BiDirectionalClientCalculator()
{
    using var channel = GrpcChannel.ForAddress("http://localhost:50051");
    var client = new Calculator.CalculatorClient(channel);

    var stream = client.FindMaximum();

    var responseResult = Task.Run(async () =>
    {
        while (await stream.ResponseStream.MoveNext(CancellationToken.None))
        {
            Console.WriteLine("Current max: {0}", stream.ResponseStream.Current.Result);
        }
    });

    for (int i = 1; i <= 10; i++)
    {
        await stream.RequestStream.WriteAsync(new() { Number = i });
    }

    await stream.RequestStream.CompleteAsync();
    await responseResult;

    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

async static Task StreamClientCalculator()
{
    using var channel = GrpcChannel.ForAddress("http://localhost:50051");
    var client = new Calculator.CalculatorClient(channel);

    var stream = client.ComputeAverage();

    for (int i = 1; i <= 10; i++)
    {
        await stream.RequestStream.WriteAsync(new() { Number = i });
    }

    await stream.RequestStream.CompleteAsync();

    var resposne = await stream.ResponseAsync.WaitAsync(CancellationToken.None);

    Console.WriteLine("Average: {0}", resposne.Result);

    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

async static Task UnaryClientCalculator()
{
    using var channel = GrpcChannel.ForAddress("http://localhost:50051");
    var client = new Calculator.CalculatorClient(channel);

    var reply = client.GetPimeDecomposition(new() { Number = 120 });

    while (await reply.ResponseStream.MoveNext(CancellationToken.None))
    {
        Console.WriteLine(reply.ResponseStream.Current.Result);
    }

    await channel.ShutdownAsync().WaitAsync(CancellationToken.None);

    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

async static Task UnaryClient()
{
    using var channel = GrpcChannel.ForAddress("http://localhost:50051");
    var client = new Greeter.GreeterClient(channel);

    // Call the SayHello method
    var reply = await client.SayHelloAsync(new HelloRequest { Name = "Khaled" });

    Console.WriteLine("Greeting: " + reply.Message);
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}