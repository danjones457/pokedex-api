namespace PokedexAPI.Interfaces.Helpers
{
    public interface IPokeApiHelper
    {
        public Task<string> GetPokemonSpeciesResponse(string pokemon);
    }
}
