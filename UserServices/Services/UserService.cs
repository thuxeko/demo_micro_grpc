using AutoMapper;
using Config.Protos;
using EntityBusiness.Models;
using EntityCore.Repository;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserServices.Protos;
using static Config.Protos.VipProtoServices;

namespace UserServices.Services
{
    public class UserService : UserProtoServices.UserProtoServicesBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Payment> _payment;
        private readonly IRepository<User> _user;
        private readonly VipProtoServicesClient _vipProtoServicesClient;

        public UserService(ILogger<UserService> logger, IMapper mapper, IRepository<Payment> payment, IRepository<User> user, VipProtoServicesClient vipProtoServicesClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _payment = payment;
            _user = user;
            _vipProtoServicesClient = vipProtoServicesClient;
        }

        public override async Task<GetPaymentWithUserResponse> GetPaymentWithUser(GetPaymentWithUserRequest request, ServerCallContext context)
        {
            try
            {
                var payment_query = await _payment.Query().Where(x => x.UserId == request.Userid).ToListAsync();
                var payment_list = new List<PaymentModel>();
                if (!payment_query.Any())
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Khong tim thay ban ghi nao"));
                }

                foreach (var payment in payment_query)
                {
                    var paymentModel = _mapper.Map<PaymentModel>(payment);
                    payment_list.Add(paymentModel);
                }

                GetPaymentWithUserResponse payment_response = new GetPaymentWithUserResponse();
                payment_response.Payment.AddRange(payment_list);
                return payment_response;
            }
            catch (Exception e)
            {
                throw new RpcException(new Status(StatusCode.Unknown, e.Message));
            }
        }

        public override async Task<UserModel> GetUserInfo(GetUserRequest request, ServerCallContext context)
        {
            try
            {

                var userInfo = await (from u in _user.Query()
                                      join p in _payment.Query() on u.Id equals p.UserId
                                      where u.Id == request.Userid
                                      group p by new { u.Username, u.Name, u.PhoneNumber } into g
                                      select new UserVip
                                      {
                                          Name = g.Key.Name,
                                          PhoneNumber = g.Key.PhoneNumber,
                                          TotalPay = g.Sum(p => p.Money),
                                          VipLatest = ""
                                      }).FirstOrDefaultAsync();

                if (userInfo == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Khong tim thay ban ghi nao"));
                }

                var vip_with_money = await _vipProtoServicesClient.GetVipWithMoneyAsync(new GetVipWithMoneyRequest { Money = userInfo.TotalPay });
                if (vip_with_money == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Khong tim thay ban ghi nao"));
                }

                userInfo.VipLatest = vip_with_money.VipName;
                var userModel = _mapper.Map<UserModel>(userInfo);
                return userModel;
            }
            catch (Exception e)
            {
                throw new RpcException(new Status(StatusCode.Unknown, e.Message));
            }
        }

        public override async Task<AddPaymentResponse> AddPayment(AddPaymentRequest request, ServerCallContext context)
        {
            try
            {

                var paymentModel = _mapper.Map<Payment>(request.Payment);

                await _payment.Insert(paymentModel);
                
                return new AddPaymentResponse()
                {
                    Success = true,
                    Message = "Them thanh cong"
                };
            }
            catch (Exception e)
            {
                return new AddPaymentResponse()
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }
    }
}
