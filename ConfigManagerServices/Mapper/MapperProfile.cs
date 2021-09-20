using AutoMapper;
using ConfigManagerServices.Protos;
using EntityConfig.Models;

namespace ConfigManagerServices.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile(){

            CreateMap<VipModel, VipConfig>().ReverseMap();
        }
    }
}
