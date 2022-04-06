using PokedexAPI.Models;

namespace PokedexAPI.Interfaces.Helpers
{
    public interface IPokemonTranslatedHelper
    {
        /// <summary>
        /// Translate a Pokemon's description based on its attributes
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public Task<Pokemon> GetTranslatedPokemon(Pokemon pokemon);
    }
}
