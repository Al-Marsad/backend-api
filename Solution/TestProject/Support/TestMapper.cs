using AutoMapper;
using BL.MappingProfiles;
using Microsoft.Extensions.Logging.Abstractions;

namespace TestProject.Support;

internal static class TestMapper
{
    public static IMapper Create()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MainMappingProfile>(), NullLoggerFactory.Instance);
        return config.CreateMapper();
    }
}
