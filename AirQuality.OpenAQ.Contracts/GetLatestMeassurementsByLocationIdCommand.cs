using FastEndpoints;

namespace AirQuality.OpenAQ.Contracts
{
    public class GetLatestMeassurementsByLocationIdCommand : ICommand<List<MeasurementDTO>>
    {
        public int locationId;
    }
}
