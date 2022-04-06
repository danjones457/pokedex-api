using PokedexAPI.Models;

namespace PokedexAPI.Interfaces.Handlers
{
    public interface IPokemonTranslatedHandler
    {
        /// <summary>
        /// Get information about a Pokemon with a translated description dependant on the Pokemon's attributes
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public Task<Pokemon> GetTranslatedPokemon(string pokemon);
    }
}
