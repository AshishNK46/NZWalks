using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionsDTO>().ReverseMap();
            CreateMap<Region, AddRegionRequestDTO>().ReverseMap();
            CreateMap<Region, UpdateRegionRequestDTO>().ReverseMap();
        }
    }
}
