using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Application.NeshanDistabceService
{
    public interface IDistanceService
    {
        public  Task<int> DistanceOnly(RequestDistanceDto request);
        public  Task<DistanceAndDurationDto> DistanceAndDurationNoTraffic(RequestDistanceDto request);
        public Task<DistanceAndDurationDto> DistanceAndDuration(RequestDistanceDto request);
    }

    public class DistanceService : IDistanceService
    {
        public async Task<int> DistanceOnly(RequestDistanceDto request)
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("https://api.neshan.org/v1/distance-matrix/no-traffic");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Api-Key", request.ApiKey);

            var response = await client.GetAsync($"?type={request.type}&origins={request.OriginLat},{request.Originlng}&destinations={request.Deslat},{request.Deslng}");
            var rp = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(rp);
            var Distance = (int)jsonObject["rows"][0]["elements"][0]["distance"]["value"];
            return Distance;
        }

        public async Task<DistanceAndDurationDto> DistanceAndDurationNoTraffic(RequestDistanceDto request)
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("https://api.neshan.org/v1/distance-matrix/no-traffic");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Api-Key", request.ApiKey);

            var response = await client.GetAsync($"?type={request.type}&origins={request.OriginLat},{request.Originlng}&destinations={request.Deslat},{request.Deslng}");
            var rp = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(rp);
            var Distance = (int)jsonObject["rows"][0]["elements"][0]["distance"]["value"];
            var Duration = (int)jsonObject["rows"][0]["elements"][0]["duration"]["value"];

            return new DistanceAndDurationDto
            {
                Distance = Distance,
                Duration = Duration
            };
        }



        public async Task<DistanceAndDurationDto> DistanceAndDuration(RequestDistanceDto request)
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("https://api.neshan.org/v1/distance-matrix");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Api-Key", request.ApiKey);

            var response = await client.GetAsync($"?type={request.type}&origins={request.OriginLat},{request.Originlng}&destinations={request.Deslat},{request.Deslng}");
            var rp = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(rp);
            var Distance = (int)jsonObject["rows"][0]["elements"][0]["distance"]["value"];
            var Duration = (int)jsonObject["rows"][0]["elements"][0]["duration"]["value"];

            return new DistanceAndDurationDto
            {
                Distance = Distance,
                Duration = Duration
            };
        }

    }

    public class DistanceAndDurationDto
    {
        public int Distance { get; set; }
        public int Duration { get; set; }
    }
    public class RequestDistanceDto
    {
        public string OriginLat {  get; set; }
        public string Originlng {  get; set; }
        public string Deslat {  get; set; }
        public string Deslng {  get; set; }
        public string ApiKey {  get; set; }
        public string type {  get; set; }
    }
}
