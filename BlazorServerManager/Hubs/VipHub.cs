using BlazorVipService.Protos;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BlazorVipService.Protos.VipProtoServices;

namespace BlazorServerManager.Hubs
{
    public class VipHub : Hub
    {
        private readonly VipProtoServicesClient _vipProtoServicesClient;
        public VipHub(VipProtoServicesClient vipProtoServicesClient)
        {
            _vipProtoServicesClient = vipProtoServicesClient;
        }

        public async Task GetListVip()
        {
            List<VipModel> lstVip = new List<VipModel>();
            var lstOut = await _vipProtoServicesClient.GetAllVipAsync(new GetAllVipRequest());
            lstVip.AddRange(lstOut.Vip);

            await Clients.All.SendAsync("ReceiveListVip", lstVip);
        }

        public async Task GetVipWithId(int id)
        {
            var action = await _vipProtoServicesClient.GetVipWithIdAsync(new GetVipWithIdRequest { Id = id });
            await Clients.Caller.SendAsync("ReceiveObjectVip", action);
        }

        public async Task DeleteVip(int id)
        {
            var action = await _vipProtoServicesClient.DeleteVipAsync(new DeleteVipRequest { Id = id });
            await Clients.Caller.SendAsync("ReceiveDelete", action);
            await GetListVip();
        }

        public async Task InsertVip(VipModel objModel)
        {
            var action = await _vipProtoServicesClient.InsertVipAsync(new InsertVipRequest { Vip = objModel });
            await GetListVip();
        }

        public async Task UpdateVip(VipModel objModel)
        {
            var action = await _vipProtoServicesClient.UpdateVipAsync(new UpdateVipRequest { Vip = objModel });
            await GetListVip();
        }
    }
}
