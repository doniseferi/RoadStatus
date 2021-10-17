using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using NUnit.Framework;
using RoadStatus.EndToEndTests.Configuration;
using RoadStatus.EndToEndTests.Records;
using RoadStatus.EndToEndTests.TestHandler;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace RoadStatus.EndToEndTests.Steps
{
    [Binding]
    class RoadStatusSteps
    {
        class RoadResponse
        {
            [JsonProperty("$type")]
            public string Type { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("statusSeverity")]
            public string StatusSeverity { get; set; }

            [JsonProperty("statusSeverityDescription")]
            public string StatusSeverityDescription { get; set; }

            [JsonProperty("bounds")]
            public string Bounds { get; set; }

            [JsonProperty("envelope")]
            public string Envelope { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        private readonly SystemUnderTestExecutionHandler _systemUnderTestExecutionHandler;
        private readonly IFlurlClient _tfLHttpClient;
        private readonly TfLApiConfig _tpLApiConfig;
        private List<Road> _roads;
        private List<ConsoleApplicationExecutionResult> _results;

        public RoadStatusSteps(SystemUnderTestExecutionHandler systemUnderTestExecutionHandler,
            IFlurlClient tfLHttpClient, TfLApiConfig tpLApiConfig)
        {
            _systemUnderTestExecutionHandler = systemUnderTestExecutionHandler ??
                                               throw new ArgumentNullException(nameof(systemUnderTestExecutionHandler));
            _tfLHttpClient = tfLHttpClient;
            _tpLApiConfig = tpLApiConfig;
        }

        [Given(@"a valid road ID is specified:")]
        public void GivenAValidRoadIDIsSpecified(Table table) => _roads = table.CreateSet<Road>().ToList();

        [When(@"the client is run")]
        public async Task WhenTheClientIsRun() =>
            _results = (
                await Task.WhenAll(
                    _roads
                        .Select(async r => await _systemUnderTestExecutionHandler.ExecuteAsync(new[] {r.RoadId}))
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
        public async Task ThenTheRoadShouldBeDisplayedAs(string key, string consoleKeyValue)
        {
            var roadResponse = await "Road/"
                .SetQueryParam("app_key", _tpLApiConfig.ApiKey)
                .GetJsonAsync<RoadResponse>();

            /*
             * call api at tfl
             * get property from key
             * call console app
             * get consoleKeyValue of console response
             */
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