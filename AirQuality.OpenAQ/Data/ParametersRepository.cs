using AirQuality.OpenAQ.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AirQuality.OpenAQ.Data
{
    internal class ParametersRepository : IParametersRepository
    {
        private readonly OpenAQDbContext _context;
        public ParametersRepository(OpenAQDbContext context)
        {
            context.Database.EnsureCreated();
            _context = context;

        }
        public async Task<List<ParameterDTO>> GetAllParametersAsync()
        {
            return await _context.Parameters.ToListAsync();
        }

        public async Task<ParameterDTO> GetParameterByIdAsync(int id)
        {
            return await _context.Parameters.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateParametersAsync(List<ParameterDTO> parameters)
        {
            foreach (var parameter in parameters)
            {

                // Check if the parameter exists in the database
                var existingParameter = await _context.Parameters.FirstOrDefaultAsync(p => p.Id == parameter.Id);

                if (existingParameter != null)
                {
                    // Update the existing parameter
                    existingParameter.Name = parameter.Name;
                    existingParameter.Units = parameter.Units;
                    existingParameter.DisplayName = parameter.DisplayName;
                    existingParameter.Description = parameter.Description;
                }
                else
                {
                    // Add the new parameter
                    await _context.Parameters.AddAsync(parameter);
                }

            }
        }
    }
}
