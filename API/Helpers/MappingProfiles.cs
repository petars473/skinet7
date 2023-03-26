using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
            .ForMember(x => x.ProductBrand, o => o.MapFrom(src => src.ProductBrand.Name))
            .ForMember(x => x.ProductType, o => o.MapFrom(src => src.ProductType.Name))
            .ForMember(x => x.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            
        }
    }
}