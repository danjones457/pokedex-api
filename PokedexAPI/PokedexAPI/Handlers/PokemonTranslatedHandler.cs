using PokedexAPI.Interfaces.Handlers;
using PokedexAPI.Models;

namespace PokedexAPI.Handlers
{
    public class PokemonTranslatedHandler : IPokemonTranslatedHandler
    {
        private readonly ILogger<PokemonTranslatedHandler> _logger;
        private readonly IPokemonHandler _pokemonHandler;

        public PokemonTranslatedHandler(ILogger<PokemonTranslatedHandler> logger, IPokemonHandler pokemonHandler)
        {
            _logger = logger;
            _pokemonHandler = pokemonHandler;
        }

        /// <summary>
        /// See <see cref="IPokemonTranslatedHandler.GetTranslatedPokemon(string)"/>
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public async Task<Pokemon> GetTranslatedPokemon(string pokemon)
        {
            return await _pokemonHandler.GetPokemon(pokemon);
        }
    }
}
