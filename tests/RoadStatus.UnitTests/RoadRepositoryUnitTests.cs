using System;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Testing;
using LanguageExt;
using Newtonsoft.Json;
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

        [Test]
        [TestCase("western cross route")]
        [TestCase("city route")]
        public async Task Returns_A_Road_When_A_Valid_Road_Id_Is_Provided(string roadId)
        {
            var expectedRoad = new Road(
                new RoadId(roadId),
                new Name(roadId),
                new Status(
                    new Severity("Good"),
                    new Description("No Exceptional Delays")));

            var mockedResult = JsonConvert.SerializeObject(expectedRoad);

            using var httpTest = new HttpTest();
            httpTest.RespondWith(status: 200, body: mockedResult);

            var roadRepository = new RoadRepository(new FlurlClient());

            var expected = Option<Road>.Some(expectedRoad);
            var actual = await roadRepository.GetByIdAsync(new RoadId(roadId));

            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}