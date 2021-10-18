using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using LanguageExt;
using RoadStatus.Data.Dto;
using RoadStatus.Data.Mapper;
using RoadStatus.Service.Entities;
using RoadStatus.Service.Repository;
using RoadStatus.Service.ValueObjects;

[assembly: InternalsVisibleTo("RoadStatus.Console"),
           InternalsVisibleTo("RoadStatus.UnitTests")]

namespace RoadStatus.Data.Repository
{
    internal class RoadRepository : IRoadRepository
    {
        private readonly IFlurlClient _tfLHttpClient;
        private readonly IDtoToDomainMapper _dtoToDomainMapper;

        public RoadRepository(
            IFlurlClient tfLHttpClient,
            IDtoToDomainMapper dtoToDomainMapper)
        {
            _tfLHttpClient = tfLHttpClient ?? throw new ArgumentNullException(nameof(tfLHttpClient));
            _dtoToDomainMapper = dtoToDomainMapper ?? throw new ArgumentNullException(nameof(dtoToDomainMapper));
        }


        public async Task<Option<Road>> GetByIdAsync(RoadId roadId)
        {
            if (roadId == null)
                throw new ArgumentNullException(nameof(roadId));

            try
            {
                return ((await _tfLHttpClient
                            .BaseUrl
                            .AppendPathSegment("Road")
                            .AppendPathSegment(roadId.Value)
                            .GetJsonAsync<RoadResponse[]>())
                        .FirstOrDefault() ?? Option<RoadResponse>.None)
                    .Match(
                        Some: s => _dtoToDomainMapper.Map(s),
                        None: Option<Road>.None);
            }
            catch (FlurlHttpException e) when (e.StatusCode == 404)
            {
                return Option<Road>.None;
            }
        }
    }
}