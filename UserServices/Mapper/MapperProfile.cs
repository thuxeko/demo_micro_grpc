using AutoMapper;
using EntityBusiness.Models;
using UserServices.Protos;

namespace UserServices.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile(){

            CreateMap<PaymentModel, Payment>().ReverseMap();
            CreateMap<UserModel, UserVip>().ReverseMap();
        }
    }
}
