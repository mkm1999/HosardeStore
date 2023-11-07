using Application.NeshanDistabceService;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DeliveryCost
{
    public interface IDeliveryCostService
    {
        Task<int> BasedOnTime(string DesLat, string DesLng, string OriginLat, string OriginLng, int EachMinuteCost,string NeshanApiKey,string NeshanType, int MinimumPrice);
        Task<int> BasedOnDistance(string DesLat, string DesLng, string OriginLat, string OriginLng, int EachKMeterCost, string NeshanApiKey,string NeshanType, int MinimumPrice);
    }

    public class DeliveryCostService : IDeliveryCostService
    {
        private readonly IDistanceService _distanceService;

        public DeliveryCostService(IDistanceService distanceService)
        {
            _distanceService = distanceService;
        }

        public async Task<int> BasedOnDistance(string DesLat, string DesLng, string OriginLat, string OriginLng, int EachKMeterCost, string NeshanApiKey,string NeshanType, int MinimumPrice)
        {
            int Distance = await _distanceService.DistanceOnly(new RequestDistanceDto
            {
                ApiKey = NeshanApiKey,
                Deslat = DesLat,
                Deslng = DesLng,
                OriginLat = OriginLat,
                Originlng = OriginLng,
                type = NeshanType,
            });
            if ((Distance / 1000) * EachKMeterCost < MinimumPrice) return MinimumPrice;
            return (Distance / 1000) * EachKMeterCost;
        }

        public async  Task<int> BasedOnTime(string DesLat, string DesLng, string OriginLat, string OriginLng, int EachMinuteCost, string NeshanApiKey,string NeshanType, int MinimumPrice)
        {
            var distanceAndDuration = await _distanceService.DistanceAndDuration(new RequestDistanceDto
            {
                ApiKey = NeshanApiKey,
                Deslat = DesLat,
                Deslng = DesLng,
                OriginLat = OriginLat,
                Originlng = OriginLng,
                type = NeshanType,
            });

            int Minutes = distanceAndDuration.Duration;
            if(Minutes * EachMinuteCost < MinimumPrice) return MinimumPrice;
            return Minutes * EachMinuteCost;
        }
    }
}
