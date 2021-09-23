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
                case 3:
                    Console.WriteLine("Vui long nhap ten vip: ");
                    string vip_name = Console.ReadLine();
                    Console.WriteLine("Vui long nhap require vip: ");
                    int require_vip = Convert.ToInt32(Console.ReadLine());
                    var obj = new VipModel() { 
                        VipName = vip_name,
                        RequireVip = require_vip
                    };
                    InsertConfig(obj).Wait();
                    break;
                case 4:
                    Console.WriteLine("Vui long nhap vip id: ");
                    int vipID = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Vui long nhap ten vip moi: ");
                    string vipNameNew = Console.ReadLine();
                    Console.WriteLine("Vui long nhap require vip moi: ");
                    int requireVipNew = Convert.ToInt32(Console.ReadLine());
                    var objUpdate = new VipModel()
                    {
                        Id = vipID,
                        VipName = vipNameNew,
                        RequireVip = requireVipNew
                    };
                    UpdateConfig(objUpdate).Wait();
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

        private static async Task InsertConfig(VipModel obj)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new VipProtoServices.VipProtoServicesClient(channel);
            var reply = await client.InsertVipAsync(new InsertVipRequest { Vip = obj });
            Console.WriteLine(reply);
        }

        private static async Task UpdateConfig(VipModel obj)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5002");
            var client = new VipProtoServices.VipProtoServicesClient(channel);
            var reply = await client.UpdateVipAsync(new UpdateVipRequest { Vip = obj });
            Console.WriteLine(reply);
        }
    }
}
