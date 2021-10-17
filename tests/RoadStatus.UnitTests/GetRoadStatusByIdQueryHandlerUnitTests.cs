using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Moq;
using NUnit.Framework;
using RoadStatus.Service.Entities;
using RoadStatus.Service.QueryHandlers;
using RoadStatus.Service.Repository;
using RoadStatus.Service.ValueObjects;

namespace RoadStatus.UnitTests
{
    internal class GetRoadStatusByIdQueryHandlerUnitTests
    {
        [Test]
        [TestCase("a1", "A1")]
        [TestCase("a406", "North Circular(A406)")]
        [TestCase("a404", "A404(M)")]
        public async Task Returns_The_Road_Name_When_A_Valid_Road_Id_Is_Found(string roadId, string expectedName)
        {
            var mockedRepo = new Mock<IRoadRepository>();
            mockedRepo.Setup(x => x.GetByIdAsync(It.Is<RoadId>(id => id.Value == roadId))).ReturnsAsync(
                new Road(new RoadId(roadId), new Name(expectedName),
                    new Status(new Severity("Mild"), new Description("Good"))));

            var handler = new GetRoadStatusByIdQueryHandler(mockedRepo.Object);
            var result = await handler.HandleAsync(new RoadId("A1"));
            var actual = result
                .Map(c => c.Name)
                .Map(x => x.Value)
                .IfNone(string.Empty);

            Assert.That(actual, Is.EqualTo(actual));
        }
    }
}