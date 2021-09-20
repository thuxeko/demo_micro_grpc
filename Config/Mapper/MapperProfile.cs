using AutoMapper;
using Config.Protos;
using EntityConfig.Models;

namespace UserServices.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile(){

            CreateMap<VipModel, VipConfig>().ReverseMap();
        }
    }
}
