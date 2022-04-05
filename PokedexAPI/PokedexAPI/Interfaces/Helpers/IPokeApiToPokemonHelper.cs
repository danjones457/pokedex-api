using PokedexAPI.Models;

namespace PokedexAPI.Interfaces.Helpers
{
    public interface IPokeApiToPokemonHelper
    {
        /// <summary>
        /// Convert the name of a Pokemon and the response from the PokeAPI to a Pokemon
        /// </summary>
        /// <param name="pokemon"></param>
        /// <param name="repsonse"></param>
        /// <returns></returns>
        public Pokemon ConvertPokeApiResponseToPokemon(string pokemon, string repsonse);
    }
}
