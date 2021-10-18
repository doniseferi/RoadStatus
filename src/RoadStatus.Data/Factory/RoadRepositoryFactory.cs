using System;
using Flurl.Http;
using RoadStatus.Data.Configuration;
using RoadStatus.Data.Mapper;
using RoadStatus.Data.Repository;
using RoadStatus.Service.Repository;

namespace RoadStatus.Data.Factory
{
    internal class RoadRepositoryFactory
    {
        private readonly ITfLClientConfiguration _tfLClientConfiguration;

        public RoadRepositoryFactory(
            ITfLClientConfiguration tfLClientConfiguration)
        {
            _tfLClientConfiguration = tfLClientConfiguration
                                      ?? throw new ArgumentNullException(nameof(tfLClientConfiguration));
        }

        public IRoadRepository Create()
        {
            TfLHttpClientConfiguration.Configure(_tfLClientConfiguration);

            return new RoadRepository(
                new FlurlClient(_tfLClientConfiguration.BaseUrl),
                new DtoToDomainMapper());
        }
    }
}