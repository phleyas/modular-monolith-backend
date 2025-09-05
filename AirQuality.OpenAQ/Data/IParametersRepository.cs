using AirQuality.OpenAQ.Contracts;

namespace AirQuality.OpenAQ.Data
{
    internal interface IParametersRepository
    {
        Task<ParameterDTO> GetParameterByIdAsync(int id);
        Task<List<ParameterDTO>> GetAllParametersAsync();
        Task UpdateParametersAsync(List<ParameterDTO> parameters);
        Task SaveChangesAsync();
    }
}
