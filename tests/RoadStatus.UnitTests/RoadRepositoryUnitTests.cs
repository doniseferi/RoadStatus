using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http.Testing;
using LanguageExt;
using Newtonsoft.Json;
using NUnit.Framework;
using RoadStatus.Data.Dto;
using RoadStatus.Data.Factory;
using RoadStatus.Service.Entities;
using RoadStatus.Service.Repository;
using RoadStatus.Service.ValueObjects;
using RoadStatus.UnitTests.Configuration;

namespace RoadStatus.UnitTests
{
    [TestFixture]
    internal class RoadRepositoryUnitTests
    {
        [Test]
        public async Task Throws_An_Exception_When_Null_Is_Passed_In()
        {
            var roadRepository = GetRoadRepository();
            Assert.ThrowsAsync<ArgumentNullException>(async () => await roadRepository.GetByIdAsync(null));
        }

        [Test]
        [TestCase("tfl123")]
        [TestCase("abc123")]
        public async Task Returns_None_When_Road_Not_Found(string roadId)
        {
            using var httpTest = new HttpTest();
            httpTest.RespondWith(status: 404);

            var roadRepository = GetRoadRepository();

            var expected = Option<Road>.None;
            var actual = await roadRepository.GetByIdAsync(new RoadId(roadId));
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        [TestCase("western cross route")]
        [TestCase("city route")]
        [TestCase("blackwall tunnel")]
        [TestCase("a41")]
        public async Task Returns_A_Makes_A_Request_To_TfLs_Road_Endpoint(string roadId)
        {
            var expectedRoad = new RoadResponse[]
            {
                new()
                {
                    Id = roadId,
                    DisplayName = roadId,
                    StatusSeverity = "Good",
                    StatusSeverityDescription = "No Exceptional Delays"
                }
            };

            using var httpTest = new HttpTest();
            httpTest.RespondWith(
                status: 200,
                body: JsonConvert.SerializeObject(expectedRoad));

            var roadRepository = GetRoadRepository();
            var testConfig = new TestConfiguration();

            await roadRepository.GetByIdAsync(new RoadId(roadId));

            httpTest.ShouldHaveCalled($"{testConfig.BaseUrl}/Road/{Url.Encode(roadId)}?app_key={testConfig.AppKey}");
        }

        [Test]
        [TestCase("western cross route")]
        [TestCase("city route")]
        [TestCase("blackwall tunnel")]
        [TestCase("a41")]
        public async Task Returns_A_Road_When_A_Valid_Road_Id_Is_Provided(string roadId)
        {
            var expectedRoad = new RoadResponse[]
            {
                new()
                {
                    Id = roadId,
                    DisplayName = roadId,
                    StatusSeverity = "Good",
                    StatusSeverityDescription = "No Exceptional Delays"
                }
            };

            using var httpTest = new HttpTest();
            httpTest.RespondWith(
                status: 200,
                body: JsonConvert.SerializeObject(expectedRoad));

            var roadRepository = GetRoadRepository();

            var actual = await roadRepository.GetByIdAsync(new RoadId(roadId));

            Assert.IsTrue(actual.IsSome);
            Assert.IsFalse(actual.IsNone);
        }

        private static IRoadRepository GetRoadRepository()
        {
            var config = new TestConfiguration();
            return new RoadRepositoryFactory(
                    config)
                .Create();
        }
    }
}