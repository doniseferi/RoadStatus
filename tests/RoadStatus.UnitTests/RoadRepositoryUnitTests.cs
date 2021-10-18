using System;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Testing;
using LanguageExt;
using NUnit.Framework;
using RoadStatus.Data;
using RoadStatus.Service.Entities;
using RoadStatus.Service.ValueObjects;

namespace RoadStatus.UnitTests
{
    [TestFixture]
    internal class RoadRepositoryUnitTests
    {

        [Test]
        public async Task Throws_An_Exception_When_Null_Is_Passed_In()
        {
            var roadRepository = new RoadRepository(new FlurlClient());
            Assert.ThrowsAsync<ArgumentNullException>(async () => await roadRepository.GetByIdAsync(null));
        }

        [Test]
        [TestCase("tfl123")]
        [TestCase("abc123")]
        public async Task Returns_None_When_Road_Not_Found(string roadId)
        {
            using var httpTest = new HttpTest();
            httpTest.RespondWith(status: 404);
            
            var roadRepository = new RoadRepository(new FlurlClient());

            var expected = Option<Road>.None;
            var actual = await roadRepository.GetByIdAsync(new RoadId(roadId));

            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}
