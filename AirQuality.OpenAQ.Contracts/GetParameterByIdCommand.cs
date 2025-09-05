using FastEndpoints;

namespace AirQuality.OpenAQ.Contracts
{
    public class GetParameterByIdCommand : ICommand<List<ParameterDTO>>
    {
        public int? id;
    }
}
