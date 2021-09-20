﻿using AutoMapper;
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
                var vip_query = await _vip.Query().Where(x => x.RequireVip == request.Money).FirstOrDefaultAsync();
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