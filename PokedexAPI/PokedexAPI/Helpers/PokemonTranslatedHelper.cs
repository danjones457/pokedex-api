using PokedexAPI.Interfaces.Helpers;
using PokedexAPI.Models;

namespace PokedexAPI.Helpers
{
    public class PokemonTranslatedHelper : IPokemonTranslatedHelper
    {
        private readonly ILogger<PokemonTranslatedHelper> _logger;
        private readonly ITranslationsHelper _translationsHelper;

        public PokemonTranslatedHelper(ILogger<PokemonTranslatedHelper> logger, ITranslationsHelper translationsHelper)
        {
            _logger = logger;
            _translationsHelper = translationsHelper;
        }

        /// <summary>
        /// See <see cref="IPokemonTranslatedHelper.GetTranslatedPokemon(Pokemon)"/>
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public async Task<Pokemon> GetTranslatedPokemon(Pokemon pokemon)
        {
            if (pokemon == null) throw new ArgumentNullException(nameof(pokemon));
            if (string.IsNullOrWhiteSpace(pokemon.Description)) return pokemon;

            try
            {
                if (pokemon.Habitat == "cave" || pokemon.IsLegendary)
                {
                    // Translate description using the Yoda API
                    pokemon.Description = await _translationsHelper.TranslateToYoda(pokemon.Description);
                }
                else
                {
                    // Translate description using the Shakespeare API
                    pokemon.Description = await _translationsHelper.TranslateToShakespeare(pokemon.Description);
                }
                return pokemon;
            }
            catch (Exception)
            {
                // Log exception here
                return pokemon;
            }
        }
    }
}
