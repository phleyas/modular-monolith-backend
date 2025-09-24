namespace AirQuality.OpenAQ.Contracts
{
    public class UpdateSensorsEvent
    {
        public List<SensorDTO> Sensors { get; set; }
        public int LocationId { get; set; }
    }
}
