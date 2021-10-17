using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using RoadStatus.EndToEndTests.Records;
using RoadStatus.EndToEndTests.TestHandler;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace RoadStatus.EndToEndTests.Steps
{
    [Binding]
    class RoadStatusSteps
    {
        private readonly SystemUnderTestExecutionHandler _systemUnderTestExecutionHandler;
        private List<Road> _roads;
        private List<ConsoleApplicationExecutionResult> _results;

        public RoadStatusSteps(SystemUnderTestExecutionHandler systemUnderTestExecutionHandler)
        {
            _systemUnderTestExecutionHandler = systemUnderTestExecutionHandler ?? throw new ArgumentNullException(nameof(systemUnderTestExecutionHandler));
        }

        [Given(@"a valid road ID is specified:")]
        public void GivenAValidRoadIDIsSpecified(Table table) => _roads = table.CreateSet<Road>().ToList();

        [When(@"the client is run")]
        public async Task WhenTheClientIsRun() =>
            _results = (
                await Task.WhenAll(
                _roads
                    .Select(async r => await _systemUnderTestExecutionHandler.ExecuteAsync(new[] { r.RoadId }))
                    .ToList())).ToList();


        [Then(@"the road displayName should be displayed")]
        public void ThenTheRoadDisplayNameShouldBeDisplayed() =>
            _roads.ForEach(r =>
            {
                var expectedOutput = $"The status of the {r.RoadId} is as follows";

                var doesResultContainExpectedDescription = _results.Any(t => t.ConsoleOutput.Contains(expectedOutput));

                Assert.IsTrue(doesResultContainExpectedDescription);
            });

        [Then(@"the road '(.*)' should be displayed as '(.*)'")]
        public void ThenTheRoadShouldBeDisplayedAs(string key, string value)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"an invalid road ID is specified:")]
        public void GivenAnInvalidRoadIDIsSpecified(Table table) => _roads = table.CreateSet<Road>().ToList();

        [Then(@"the application should return an informative error")]
        public void ThenTheApplicationShouldReturnAnInformativeError()
        {
            var expected = _roads
                .Select(r => $"{r} is not a valid road")
                .ToList();

            var actual = _results
                .Select(x => x.ConsoleOutput).ToList();

            var doesResultContainInformativeError = actual.Any(expected.Contains);

            Assert.IsTrue(doesResultContainInformativeError);
        }

        [Then(@"the application should exit with a non-zero System Error code")]
        public void ThenTheApplicationShouldExitWithANon_ZeroSystemErrorCode()
        {
            var didApplicationEndWithANonZeroError = _results.TrueForAll(x => x.ResultCode != 0);
            Assert.IsTrue(didApplicationEndWithANonZeroError);
        }
    }
}


