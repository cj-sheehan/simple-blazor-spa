using AutoMapper;
using NameApi.DataAccess.Models;
using NameApi.Models;

namespace NameApi.MappingProfiles
{
    public class NameProfile : Profile
    {
        public NameProfile()
        {
            CreateMap<NameModel, NameResponseModel>();
        }
    }
}
