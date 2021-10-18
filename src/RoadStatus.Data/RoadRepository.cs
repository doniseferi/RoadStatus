using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using LanguageExt;
using RoadStatus.Service.Entities;
using RoadStatus.Service.Repository;
using RoadStatus.Service.ValueObjects;

[assembly: InternalsVisibleTo("RoadStatus.Console"),
           InternalsVisibleTo("RoadStatus.UnitTests")]

namespace RoadStatus.Data
{
    internal class RoadRepository : IRoadRepository
    {
        private readonly IFlurlClient _tfLHttpClient;

        public RoadRepository(IFlurlClient tfLHttpClient)
        {
            _tfLHttpClient = tfLHttpClient ?? throw new ArgumentNullException(nameof(tfLHttpClient));
        }


        public Task<Option<Road>> GetByIdAsync(RoadId roadId)
        {
            if (roadId == null)
                throw new ArgumentNullException(nameof(roadId));

            throw new NotImplementedException();
        }
    }
}