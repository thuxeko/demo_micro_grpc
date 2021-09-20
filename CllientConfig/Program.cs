using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcVipClient;

namespace ClientConfig
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new VipProtoServices.VipProtoServicesClient(channel);
            var reply = await client.GetVipWithMoneyAsync(new GetVipWithMoneyRequest { Money = 500000 });
            Console.WriteLine("Vip Name: " + reply.VipName + " Require: " + reply.RequireVip);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
