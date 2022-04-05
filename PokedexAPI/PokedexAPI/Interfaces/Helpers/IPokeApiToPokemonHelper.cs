using PokedexAPI.Models;

namespace PokedexAPI.Interfaces.Helpers
{
    public interface IPokeApiToPokemonHelper
    {
        public Pokemon ConvertPokeApiResponseToPokemon(string pokemon, string repsonse);
    }
}
