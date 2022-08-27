using AutoMapper;

namespace ScrumPoker.Application.Configurations
{
    public class AutoMapperApiConfig : Profile
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(x => x.AllowNullCollections = true);
        }

        public AutoMapperApiConfig()
        {
            this.AddApplicationAutoMapperConfig();
        }
    }
}
