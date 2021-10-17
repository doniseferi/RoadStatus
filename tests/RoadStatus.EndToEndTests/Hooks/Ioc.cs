using System;
using BoDi;
using RoadStatus.EndToEndTests.Extensions;
using RoadStatus.EndToEndTests.TestHandler;
using TechTalk.SpecFlow;

namespace RoadStatus.EndToEndTests.Hooks
{
    [Binding]
    public class Ioc
    {
        private readonly IObjectContainer _objectContainer;

        public Ioc(IObjectContainer objectContainer) => _objectContainer = objectContainer;

        [BeforeScenario]
        public void RegisterComponents()
        {
            _objectContainer.RegisterInstanceAs(
                new SystemUnderTestExecutionHandler(
                    AppDomain.CurrentDomain.GetConsoleAppExePath()));
        }
    }
}
