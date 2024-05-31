using Grpc.Core;
using HelloWorld;

const int Port = 50051;

ServerBIDirectionalExample(Port);

static void UnaryExample(int Port)
{
    Server server = new()
    {
        Services = { Greeter.BindService(new GreeterImpl()) },
        Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
    };
    server.Start();

    Console.WriteLine("Greeter server listening on port " + Port);
    Console.WriteLine("Press any key to stop the server...");
    Console.ReadKey();

    server.ShutdownAsync().Wait();
}

static void ServerStreamExample(int Port)
{
    Server server = new()
    {
        Services = { Calculator.BindService(new CalculatorImp()) },
        Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
    };
    server.Start();

    Console.WriteLine("Calculator server listening on port " + Port);
    Console.WriteLine("Press any key to stop the server...");
    Console.ReadKey();

    server.ShutdownAsync().Wait();
}

static void ServerBIDirectionalExample(int Port)
{
    Server server = new()
    {
        Services = { Calculator.BindService(new CalculatorImp()) },
        Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
    };
    server.Start();

    Console.WriteLine("Calculator server listening on port " + Port);
    Console.WriteLine("Press any key to stop the server...");
    Console.ReadKey();

    server.ShutdownAsync().Wait();
}

class GreeterImpl : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply { Message = $"Hello, {request.Name} from RPC server" });
    }
}

class CalculatorImp : Calculator.CalculatorBase
{
    public async override Task GetPimeDecomposition(PrimeDecompositionRequest request, IServerStreamWriter<PrimeDecompositionResult> responseStream, ServerCallContext context)
    {
        int k = 2;
        while (request.Number > 1)
        {
            if (request.Number % k == 0)
            {
                await responseStream.WriteAsync(new() { Result = k });
                request.Number /= k;
            }
            else
            {
                k++;
            }

        }
    }

    public async override Task<CalculateAverageResponse> ComputeAverage(IAsyncStreamReader<CalculateAverageRequest> requestStream, ServerCallContext context)
    {
        int count = 0;
        double sum = 0;

        while (await requestStream.MoveNext())
        {
            count++;
            sum += requestStream.Current.Number;
        }

        return new() { Result = sum / count };
    }

    public override async Task FindMaximum(IAsyncStreamReader<FindMaximumRequest> requestStream, IServerStreamWriter<FindMaximumResponse> responseStream, ServerCallContext context)
    {
        int max = int.MinValue;

        while (await requestStream.MoveNext())
        {
            if (requestStream.Current.Number > max)
            {
                max = requestStream.Current.Number;

                await responseStream.WriteAsync(new() { Result = max });
            }
        }

    }
}