using PokedexAPI.Models;

namespace PokedexAPI.Interfaces
{
    public interface IPokemonHandler
    {
        public Task<Pokemon> GetPokemon(string pokemon);
    }
}
