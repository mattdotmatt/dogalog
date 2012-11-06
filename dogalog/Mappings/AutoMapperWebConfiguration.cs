using AutoMapper;
using dogalog.Mappings.Profiles;

namespace dogalog.Mappings
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => cfg.AddProfile(new CategoryProfile()));
        }
    }
}
