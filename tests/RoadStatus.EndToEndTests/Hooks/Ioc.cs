using System;
using System.Reflection;
using BoDi;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using RoadStatus.EndToEndTests.Configuration;
using RoadStatus.EndToEndTests.Extensions;
using RoadStatus.EndToEndTests.TestHandler;
using TechTalk.SpecFlow;

namespace RoadStatus.EndToEndTests.Hooks
{
    [Binding]
    public class Ioc
    {
        private readonly IObjectContainer _objectContainer;
        
        public static IConfiguration GetConfiguration() =>
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

        public Ioc(IObjectContainer objectContainer) => _objectContainer = objectContainer;

        [BeforeScenario]
        public void RegisterComponents()
        {
            _objectContainer.RegisterInstanceAs(CreateTfLHttpClient());
            _objectContainer.RegisterInstanceAs(
                new SystemUnderTestExecutionHandler(
                    AppDomain.CurrentDomain.GetConsoleAppExePath()));
        }

        private IFlurlClient CreateTfLHttpClient()
        {
            var tflApiConfig = new TfLApiConfig();
            GetConfiguration().GetSection(TfLApiConfig.Section).Bind(tflApiConfig);
            _objectContainer.RegisterInstanceAs(tflApiConfig);
            return new FlurlClient();
        }
    }
}
