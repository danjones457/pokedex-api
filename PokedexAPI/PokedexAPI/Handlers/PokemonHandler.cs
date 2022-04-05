using PokedexAPI.Interfaces.Handlers;
using PokedexAPI.Interfaces.Helpers;
using PokedexAPI.Models;

namespace PokedexAPI.Handlers
{
    public class PokemonHandler : IPokemonHandler
    {
        private readonly ILogger<PokemonHandler> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPokeApiToPokemonHelper _pokeApiToPokemonHelper;

        public PokemonHandler(ILogger<PokemonHandler> logger, IConfiguration configuration, IPokeApiToPokemonHelper pokeApiToPokemonHelper)
        {
            _logger = logger;
            _configuration = configuration;
            _pokeApiToPokemonHelper = pokeApiToPokemonHelper;
        }

        public async Task<Pokemon> GetPokemon(string pokemon)
        {
            try
            {
                using var client = new HttpClient();
                var pokeApiUrl = _configuration["PokeApiUrl"];
                var pokeApiResponse = await client.GetStringAsync(pokeApiUrl + "/pokemon-species/" + pokemon);

                return _pokeApiToPokemonHelper.ConvertPokeApiResponseToPokemon(pokemon, pokeApiResponse);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
