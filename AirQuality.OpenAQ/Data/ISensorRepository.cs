using AirQuality.OpenAQ.Contracts;


namespace AirQuality.OpenAQ.Data
{
    internal interface ISensorRepository
    {
        Task SaveChangesAsync();
        Task UpdateSensorsAsync(List<SensorDTO> sensors);
        Task UpdateSensorAsync(SensorDTO sensor);
        Task<List<SensorDTO>> GetSensorsByLocationIdAsync(int locationId);
        Task<SensorDTO?> GetSensorByIdAsync(int id);
        Task<List<SensorDTO>> GetAllSensorsAsync();
        Task RemoveSensorAsync(int id);

    }
}
