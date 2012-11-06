using System.Collections.Generic;
using AutoMapper;
using dogalog.Entities;
using dogalog.Models;

namespace dogalog.Mappings.Profiles
{
    public class CategoryProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Category, CategoryModel>();
            Mapper.CreateMap<IList<Category>, IList<CategoryModel>>();
            Mapper.CreateMap<CategoryModel, Category>();
        }
    }
}