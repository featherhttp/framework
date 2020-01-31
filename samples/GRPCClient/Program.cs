using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace GRPCClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number(3000) must match the port of the gRPC server.
            var channel = GrpcChannel.ForAddress("https://localhost:3000");
            var client = new Greeter.GreeterClient(channel);

            var reply = await client.SayHelloAsync(new HelloRequest { Name = "grpcClient" });

            Console.WriteLine("Greeting: " + reply.Message);

            Console.ReadKey();
        }
    }
}
