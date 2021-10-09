using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Nielsen.TechTest.Q1.Common;

namespace Nielsen.TechTest.Q1.RemoteRobotController
{
    public class RemoteRobotController : ISimpleAsyncRobot<Location, MoveInstruction>
    {
        private readonly ILogger<RemoteRobotController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _svrUrl;
        private readonly JsonSerializerOptions _serOption;

        public RemoteRobotController(ILogger<RemoteRobotController> logger,
            IHttpClientFactory clientFactory,
            IConfiguration config)
        {
            this._logger = logger;
            this._httpClientFactory = clientFactory;
            this._svrUrl = config.GetValue<string>("RobotHostingUrl");
            this._serOption = new JsonSerializerOptions();
            this._serOption.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            this._serOption.PropertyNameCaseInsensitive = true;
            this._serOption.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task<Location> GetCurrentLocationAsync()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, this._svrUrl))
            {
                using (var httpClient = this._httpClientFactory.CreateClient())
                {
                    var response = await httpClient.SendAsync(request,
                        HttpCompletionOption.ResponseContentRead,
                        CancellationToken.None);
                    var stringContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode == false)
                    {
                        throw new ApplicationException($"Request, {this._svrUrl} (GET), failed (Http Status Code: {response.StatusCode}{Environment.NewLine}{stringContent}");
                    }

                    this._logger.LogInformation($"Received content, {stringContent}");
                    return JsonSerializer.Deserialize<Location>(stringContent, this._serOption);
                }
            }
        }

        public async Task<Location> MoveAsync(MoveInstruction moveIntruction)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, this._svrUrl))
            {
                var cnt = JsonSerializer.Serialize<MoveInstruction>(moveIntruction);
                request.Content = new StringContent(cnt, Encoding.UTF8, "application/json");

                using (var httpClient = this._httpClientFactory.CreateClient())
                {
                    var response = await httpClient.SendAsync(request,
                        HttpCompletionOption.ResponseContentRead,
                        CancellationToken.None);
                    var stringContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode == false)
                    {
                        throw new ApplicationException($"Request, {this._svrUrl} (PUT), failed (Http Status Code: {response.StatusCode}{Environment.NewLine}{stringContent}");
                    }

                    this._logger.LogInformation($"Received content, {stringContent}");
                    return JsonSerializer.Deserialize<Location>(stringContent, this._serOption);
                }
            }
        }
    }
}
