using AirQuality.OpenAQ.Contracts;
using AirQuality.OpenAQ.Data;
using FastEndpoints;

namespace AirQuality.OpenAQ.Integrations
{
    internal class GetParametersByIdHandler : ICommandHandler<GetParameterByIdCommand, List<ParameterDTO>>
    {
        private readonly IOpenAQService _openAQService;
        private readonly IParametersRepository _parametersRepository;
        public GetParametersByIdHandler(IOpenAQService openAQService, IParametersRepository parametersRepository)
        {
            _openAQService = openAQService;
            _parametersRepository = parametersRepository;
        }

        public async Task<List<ParameterDTO>> ExecuteAsync(GetParameterByIdCommand command, CancellationToken ct)
        {
            if (command.id == null)
            {
                var localParameters = await _parametersRepository.GetAllParametersAsync();
                if (localParameters != null && localParameters.Count > 0)
                {
                    return localParameters;
                }
                else
                {
                    var response = await _openAQService.GetParametersAsync();
                    if (response == null || response.Results.Count == 0)
                    {
                        return [];
                    }
                    await _parametersRepository.UpdateParametersAsync(response.Results);
                    await _parametersRepository.SaveChangesAsync();
                    return response.Results;
                }
            }
            else
            {
                var localParameter = await _parametersRepository.GetParameterByIdAsync(command.id.Value);
                if (localParameter != null)
                {
                    return new List<ParameterDTO> { localParameter };
                }
                else
                {
                    var response = await _openAQService.GetParametersAsync(command.id);
                    if (response != null && response.Results.Count > 0)
                    {
                        await _parametersRepository.UpdateParametersAsync(response.Results);
                        await _parametersRepository.SaveChangesAsync();
                        return response.Results;
                    }

                }
                return [];
            }

        }
    }
}
