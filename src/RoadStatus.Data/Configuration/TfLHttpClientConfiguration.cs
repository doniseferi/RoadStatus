using System.Threading.Tasks;
using Flurl.Http;

namespace RoadStatus.Data.Configuration
{
    internal class TfLHttpClientConfiguration
    {
        public static void Configure(ITfLClientConfiguration tfLClientConfiguration) =>
            FlurlHttp.ConfigureClient(
                tfLClientConfiguration.BaseUrl,
                x => x
                    .Configure(settings =>
                    {
                        settings.BeforeCallAsync = call =>
                            Task.FromResult(call.Request.SetQueryParam("app_key", tfLClientConfiguration.AppKey));

                        settings.BeforeCall = call =>
                            call.Request.SetQueryParam("app_key", tfLClientConfiguration.AppKey);
                    }));
    }
}