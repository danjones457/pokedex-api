using PokedexAPI.Interfaces.Helpers;

namespace PokedexAPI.Helpers
{
    public class PokeApiHelper : IPokeApiHelper
    {
        private readonly ILogger<PokeApiHelper> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PokeApiHelper(ILogger<PokeApiHelper> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// See <see cref="IPokeApiHelper.GetPokemonSpeciesResponse(string)"/>
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public async Task<string> GetPokemonSpeciesResponse(string pokemon)
        {
            if (string.IsNullOrWhiteSpace(pokemon)) throw new ArgumentNullException(nameof(pokemon));

            try
            {
                var pokeApiUrl = _configuration["PokeApiUrl"];
                return await _httpClient.GetStringAsync(pokeApiUrl + "/pokemon-species/" + pokemon);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new ArgumentException("We were unable to find that Pokemon.");
                }

                throw;
            }
        }
    }
}
