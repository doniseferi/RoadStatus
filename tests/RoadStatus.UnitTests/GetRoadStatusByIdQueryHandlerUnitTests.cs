using System;
using System.Threading.Tasks;
using LanguageExt;
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
        public async Task Returns_The_Road_Name_When_A_Valid_Road_Id_Is_Provided(string roadId, string expectedName)
        {
            var mockedRepo = new Mock<IRoadRepository>();
            mockedRepo.Setup(
                    x => x.GetByIdAsync(It.Is<RoadId>(id => id.Value == roadId)))
                .ReturnsAsync(
                    new Road(new RoadId(roadId), new Name(expectedName),
                        new Status(new Severity("Mild"), new Description("Good"))));

            var handler = new GetRoadStatusByIdQueryHandler(mockedRepo.Object);
            var result = await handler.HandleAsync(new RoadId(roadId));
            var actual = result
                .Map(c => c.Name)
                .Map(x => x.Value)
                .IfNone(string.Empty);

            Assert.That(actual, Is.EqualTo(expectedName));
        }

        [Test]
        [TestCase("a1", "Good")]
        [TestCase("a406", "Closure")]
        public async Task Returns_The_Road_Status_When_A_Valid_Road_Id_Is_Provided(string roadId,
            string expectedDescription)
        {
            var mockedRepo = new Mock<IRoadRepository>();
            mockedRepo.Setup(
                    x => x.GetByIdAsync(It.Is<RoadId>(id => id.Value == roadId)))
                .ReturnsAsync(
                    new Road(new RoadId(roadId), new Name(roadId),
                        new Status(new Severity(expectedDescription), new Description("Abc"))));

            var handler = new GetRoadStatusByIdQueryHandler(mockedRepo.Object);
            var result = await handler.HandleAsync(new RoadId(roadId));
            var actual = result
                .Map(c => c.Status)
                .Map(x => x.Severity)
                .Map(x => x.Value)
                .IfNone(string.Empty);

            Assert.That(actual, Is.EqualTo(expectedDescription));
        }

        [Test]
        [TestCase("a4", "Closure")]
        [TestCase("a406", "No Exceptional Delays")]
        public async Task Returns_The_Road_Status_Description_When_A_Valid_Road_Id_Is_Provided(string roadId,
            string expectedDescription)
        {
            var mockedRepo = new Mock<IRoadRepository>();
            mockedRepo.Setup(
                    x => x.GetByIdAsync(It.Is<RoadId>(id => id.Value == roadId)))
                .ReturnsAsync(
                    new Road(new RoadId(roadId), new Name(roadId),
                        new Status(new Severity(expectedDescription), new Description("Abc"))));

            var handler = new GetRoadStatusByIdQueryHandler(mockedRepo.Object);
            var result = await handler.HandleAsync(new RoadId(roadId));
            var actual = result
                .Map(c => c.Status)
                .Map(x => x.Description)
                .Map(x => x.Value)
                .IfNone(string.Empty);

            Assert.That(actual, Is.EqualTo(expectedDescription));
        }

        [Test]
        [TestCase("tfl123")]
        [TestCase("abc123")]
        public async Task Returns_None_When_The_Road_Is_Not_Found(string roadId)
        {
            var mockedRepo = new Mock<IRoadRepository>();
            mockedRepo.Setup(x => x.GetByIdAsync(It.IsAny<RoadId>()))
                .ReturnsAsync(
                    Option<Road>.None);

            var handler = new GetRoadStatusByIdQueryHandler(mockedRepo.Object);
            var result = await handler.HandleAsync(new RoadId(roadId));
            var actual = result
                .Map(c => c.Status)
                .Map(x => x.Description)
                .Map(x => x.Value)
                .IfNone(string.Empty);

            Assert.That(actual, Is.EqualTo(Option<Road>.None));
        }
        
        [Test]
        public async Task Throws_An_Exception_When_Null_Is_Passed_In()
        {
            var mockedRepo = new Mock<IRoadRepository>();
            var handler = new GetRoadStatusByIdQueryHandler(mockedRepo.Object);
            Assert.ThrowsAsync<ArgumentNullException>(async () => await handler.HandleAsync(null));
        }
    }
}