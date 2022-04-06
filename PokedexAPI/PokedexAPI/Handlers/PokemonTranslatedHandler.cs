using PokedexAPI.Interfaces.Handlers;
using PokedexAPI.Interfaces.Helpers;
using PokedexAPI.Models;

namespace PokedexAPI.Handlers
{
    public class PokemonTranslatedHandler : IPokemonTranslatedHandler
    {
        private readonly ILogger<PokemonTranslatedHandler> _logger;
        private readonly IPokemonHandler _pokemonHandler;
        private readonly IPokemonTranslatedHelper _pokemonTranslatedHelper;

        public PokemonTranslatedHandler(ILogger<PokemonTranslatedHandler> logger, IPokemonHandler pokemonHandler, IPokemonTranslatedHelper pokemonTranslatedHelper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pokemonHandler = pokemonHandler ?? throw new ArgumentNullException(nameof(pokemonHandler));
            _pokemonTranslatedHelper = pokemonTranslatedHelper ?? throw new ArgumentNullException(nameof(pokemonTranslatedHelper));
        }

        /// <summary>
        /// See <see cref="IPokemonTranslatedHandler.GetTranslatedPokemon(string)"/>
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public async Task<Pokemon> GetTranslatedPokemon(string pokemon)
        {
            if (string.IsNullOrWhiteSpace(pokemon)) throw new ArgumentNullException(nameof(pokemon));

            var pokemonResponse = await _pokemonHandler.GetPokemon(pokemon);

            return await _pokemonTranslatedHelper.GetTranslatedPokemon(pokemonResponse);
        }
    }
}
