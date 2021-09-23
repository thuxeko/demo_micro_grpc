using AutoMapper;
using ConfigManagerServices.Protos;
using EntityConfig.Models;
using EntityCore.Repository;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigManagerServices.Services
{
    public class VipService: VipProtoServices.VipProtoServicesBase
    {
        private readonly ILogger<VipService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<VipConfig> _vip;
        public VipService(ILogger<VipService> logger, IMapper mapper, IRepository<VipConfig> vip)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _vip = vip;
        }

        public override async Task<VipModel> GetVipWithMoney(GetVipWithMoneyRequest request, ServerCallContext context)
        {
            try
            {
                var vip_query = await _vip.Query().Where(x => x.RequireVip <= request.Money).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                var vipModel = _mapper.Map<VipModel>(vip_query);

                return vipModel;
            }
            catch (Exception e)
            {
                throw new RpcException(new Status(StatusCode.Unknown, e.Message));
            }
        }

        public override async Task<GetAllVipResponse> GetAllVip(GetAllVipRequest request, ServerCallContext context)
        {
            try
            {
                var vip_query = await _vip.GetAll();
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

        public override async Task<CRUDVipResponse> InsertVip(InsertVipRequest request, ServerCallContext context)
        {
            var response = new CRUDVipResponse();
            try
            {
                var vip_insert = _mapper.Map<VipConfig>(request.Vip);
                await _vip.Insert(vip_insert);

                response.Success = true;
                response.Mes = "Them ban ghi thanh cong";
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Mes = e.Message;
                return response;
            }
        }

        public override async Task<CRUDVipResponse> UpdateVip(UpdateVipRequest request, ServerCallContext context)
        {
            var response = new CRUDVipResponse();
            try
            {
                var vip_insert = _mapper.Map<VipConfig>(request.Vip);
                await _vip.Update(vip_insert);

                response.Success = true;
                response.Mes = "Sua ban ghi thanh cong";
                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Mes = e.Message;
                return response;
            }
        }
    }
}
