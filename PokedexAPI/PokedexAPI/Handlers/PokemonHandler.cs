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
            _logger = logger;
            _pokeApiToPokemonHelper = pokeApiToPokemonHelper;
            _pokeApiHelper = pokeApiHelper;
        }

        /// <summary>
        /// See <see cref="IPokemonHandler.GetPokemon(string)"/>
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public async Task<Pokemon> GetPokemon(string pokemon)
        {
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
