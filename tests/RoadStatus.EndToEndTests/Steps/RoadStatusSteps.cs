using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using NUnit.Framework;
using RoadStatus.EndToEndTests.Configuration;
using RoadStatus.EndToEndTests.Extensions;
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
        private readonly IFlurlClient _tfLHttpClient;
        private readonly TfLApiConfig _tfLApiConfig;
        private List<Road> _roads;
        private List<ConsoleApplicationExecutionResult> _results;

        public RoadStatusSteps(SystemUnderTestExecutionHandler systemUnderTestExecutionHandler,
            IFlurlClient tfLHttpClient, TfLApiConfig tfLApiConfig)
        {
            _systemUnderTestExecutionHandler = systemUnderTestExecutionHandler ??
                                               throw new ArgumentNullException(nameof(systemUnderTestExecutionHandler));
            _tfLHttpClient = tfLHttpClient ?? throw new ArgumentNullException(nameof(tfLHttpClient));
            _tfLApiConfig = tfLApiConfig ?? throw new ArgumentNullException(nameof(tfLApiConfig));
        }

        [Given(@"a valid road ID is specified:")]
        public void GivenAValidRoadIDIsSpecified(Table table) => _roads = table.CreateSet<Road>().Skip(1).ToList();

        [When(@"the client is run")]
        public async Task WhenTheClientIsRun() =>
            _results = await GetApplicationResponse(_roads.Select(x => x.RoadId).ToList());

        private async Task<List<ConsoleApplicationExecutionResult>> GetApplicationResponse(
            IReadOnlyCollection<string> roadIds) =>
            (await Task.WhenAll(
                roadIds
                    .Select(
                        async id => await _systemUnderTestExecutionHandler.ExecuteAsync(new[] {id}))))
            .ToList();

        [Then(@"the road displayName should be displayed")]
        public void ThenTheRoadDisplayNameShouldBeDisplayed() =>
            _roads.ForEach(r =>
            {
                var expectedOutput = $"The status of the {r.RoadId} is as follows";

                var doesResultContainExpectedDescription = _results.Any(t => t.ConsoleOutput.Contains(expectedOutput));

                Assert.IsTrue(doesResultContainExpectedDescription);
            });

        [Then(@"the road '(.*)' should be displayed as '(.*)'")]
        public async Task ThenTheRoadShouldBeDisplayedAs(string key, string consoleKeyValue)
        {
            var roads = _roads
                .Select(x => x.RoadId)
                .Select(GetRoadAsync);

            (await Task.WhenAll(roads))
                .ToList()
                .ForEach(r =>
                {
                    var valueFromTflResponse = r.GetType().GetProperty(key.ToTitledCase())?.GetValue(r, null);
                    var hasValueInApplicationResponse = _results
                        .Any(x =>
                            x.ConsoleOutput.Contains($"The status of the {r.DisplayName} is as follows")
                            && x.ConsoleOutput.Contains(
                                $"{consoleKeyValue} is {valueFromTflResponse}"));

                    Assert.IsTrue(hasValueInApplicationResponse);
                });
        }

        private async Task<RoadResponse> GetRoadAsync(string roadId) =>
            (await _tfLApiConfig.BaseUrl
                .AppendPathSegment("Road")
                .AppendPathSegment(roadId)
                .SetQueryParam("app_key", _tfLApiConfig.AppKey)
                .WithClient(_tfLHttpClient)
                .GetJsonAsync<List<RoadResponse>>())
            .FirstOrDefault();

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