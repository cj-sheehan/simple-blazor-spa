using AutoMapper;
using NameApi.MappingProfiles;
using Xunit;

namespace NameApi.Tests.MappingProfiles
{
    public class NameProfileTests
    {
        [Fact]
        public void NameProfilesMappingsAreValid()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new NameProfile());
            });

            configuration.AssertConfigurationIsValid();
        }
    }
}
