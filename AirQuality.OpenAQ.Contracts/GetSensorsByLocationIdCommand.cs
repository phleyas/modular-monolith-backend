using FastEndpoints;

namespace AirQuality.OpenAQ.Contracts
{
    public class GetSensorsByLocationIdCommand : ICommand<List<SensorDTO>>
    {
        public int locationId;
    }
}
