using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Config.Protos;
using EntityConfig.Models;
using EntityCore.Repository;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Config.Services
{
    public class VipConfigService : VipProtoServices.VipProtoServicesBase
    {
        private readonly IMapper _mapper;
        private readonly IRepository<VipConfig> _vipconfig;
        public VipConfigService(IMapper mapper, IRepository<VipConfig> vipconfig)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _vipconfig = vipconfig;
        }

        public override async Task<GetAllVipResponse> GetAllVip(GetAllVipRequest request, ServerCallContext context)
        {
            try
            {
                var vip_query = await _vipconfig.GetAll();
                var vip_list = new List<VipModel>();
                if (!vip_query.Any())
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Khong tim thay ban ghi nao"));
                }

                foreach (var vip in vip_query)
                {
                    var vipModel = _mapper.Map<VipModel>(vip);
                    vip_list.Add(vipModel);
                }

                GetAllVipResponse vip_response = new GetAllVipResponse();
                vip_response.Vip.AddRange(vip_list);
                return vip_response;
            }
            catch (Exception e)
            {
                throw new RpcException(new Status(StatusCode.Unknown, e.Message));
            }
        }

        public override async Task<VipModel> GetVipWithMoney(GetVipWithMoneyRequest request, ServerCallContext context)
        {
            try
            {
                var vip_query = await _vipconfig.Query().Where(x => x.RequireVip <= request.Money).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                var vip_list = new List<VipModel>();
                if (vip_query == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Khong tim thay ban ghi nao"));
                }
                var vipModel = _mapper.Map<VipModel>(vip_query);
                return vipModel;
            }
            catch (Exception e)
            {
                throw new RpcException(new Status(StatusCode.Unknown, e.Message));
            }
        }
    }
}
