using PokedexAPI.Interfaces.Handlers;
using PokedexAPI.Interfaces.Helpers;
using PokedexAPI.Models;

namespace PokedexAPI.Handlers
{
    public class PokemonHandler : IPokemonHandler
    {
        private readonly ILogger<PokemonHandler> _logger;
        private readonly IPokeApiToPokemonHelper _pokeApiToPokemonHelper;
        private readonly IPokeApiHelper _pokeApiHelper;

        public PokemonHandler(ILogger<PokemonHandler> logger, IPokeApiToPokemonHelper pokeApiToPokemonHelper, IPokeApiHelper pokeApiHelper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pokeApiToPokemonHelper = pokeApiToPokemonHelper ?? throw new ArgumentNullException(nameof(pokeApiToPokemonHelper));
            _pokeApiHelper = pokeApiHelper ?? throw new ArgumentNullException(nameof(pokeApiHelper));
        }

        /// <summary>
        /// See <see cref="IPokemonHandler.GetPokemon(string)"/>
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public async Task<Pokemon> GetPokemon(string pokemon)
        {
            if (string.IsNullOrWhiteSpace(pokemon)) throw new ArgumentNullException(nameof(pokemon));

            try
            {
                var pokeApiResponse = await _pokeApiHelper.GetPokemonSpeciesResponse(pokemon);
                return _pokeApiToPokemonHelper.ConvertPokeApiResponseToPokemon(pokemon, pokeApiResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
