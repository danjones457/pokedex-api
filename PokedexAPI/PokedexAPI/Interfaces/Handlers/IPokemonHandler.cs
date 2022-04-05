using PokedexAPI.Models;

namespace PokedexAPI.Interfaces.Handlers
{
    public interface IPokemonHandler
    {
        public Task<Pokemon> GetPokemon(string pokemon);
    }
}
