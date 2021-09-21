using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcVipClient;

namespace ClientConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            //using var channel = GrpcChannel.ForAddress("https://localhost:5002");
            //var client = new VipProtoServices.VipProtoServicesClient(channel);
            //var reply = await client.GetVipWithMoneyAsync(new GetVipWithMoneyRequest { Money = 500000 });
            MenuMain();
        }
        public static void MenuMain()
        {
            Console.WriteLine("Vui long nhap chuc nang: 1 - Get All Vip | 2 - Get Vip With Money | 3 - Insert Vip | 4 - Update Vip");
            int action = Convert.ToInt32(Console.ReadLine());
            switch (action)
            {
                case 1:
                    GetAllVip().Wait();
                    break;
                case 2:
                    Console.WriteLine("Vui long nhap so tien: ");
                    int money = Convert.ToInt32(Console.ReadLine());
                    GetVipWithMoney(money).Wait();
                    break;
                default:
                    break;
            }
            MenuMain();
            Console.ReadKey();
        }
        private static async Task GetAllVip()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new VipProtoServices.VipProtoServicesClient(channel);
            var reply = await client.GetAllVipAsync(new GetAllVipRequest());
            foreach (var item in reply.Vip)
            {
                Console.WriteLine(item);
            }
        }

        private static async Task GetVipWithMoney(int money)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new VipProtoServices.VipProtoServicesClient(channel);
            var reply = await client.GetVipWithMoneyAsync(new GetVipWithMoneyRequest { Money = money });
            Console.WriteLine(reply);
        }
        //async Task InsertConfig()
        //{
        //    using var channel = GrpcChannel.ForAddress("https://localhost:5002");
        //    var client = new VipProtoServices.VipProtoServicesClient(channel);
        //}
    }
}
