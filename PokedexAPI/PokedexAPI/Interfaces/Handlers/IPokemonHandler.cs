using PokedexAPI.Models;

namespace PokedexAPI.Interfaces.Handlers
{
    public interface IPokemonHandler
    {
        /// <summary>
        /// Retrieve information about a pokemon based on a passed Pokemon name
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public Task<Pokemon> GetPokemon(string pokemon);
    }
}
