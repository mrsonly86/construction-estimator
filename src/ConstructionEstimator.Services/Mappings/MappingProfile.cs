using AutoMapper;
using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Models;

namespace ConstructionEstimator.Services.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<EstimateItem, EstimateItemDto>().ReverseMap();
        CreateMap<Material, MaterialDto>().ReverseMap();
        CreateMap<Labor, LaborDto>().ReverseMap();
        CreateMap<Equipment, EquipmentDto>().ReverseMap();
    }
}