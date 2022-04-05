using PokedexAPI.Interfaces.Helpers;

namespace PokedexAPI.Helpers
{
    public class PokeApiHelper : IPokeApiHelper
    {
        private readonly ILogger<PokeApiHelper> _logger;
        private readonly IConfiguration _configuration;

        public PokeApiHelper(ILogger<PokeApiHelper> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetPokemonSpeciesResponse(string pokemon)
        {
            try
            {
                using var client = new HttpClient();
                var pokeApiUrl = _configuration["PokeApiUrl"];
                return await client.GetStringAsync(pokeApiUrl + "/pokemon-species/" + pokemon);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
