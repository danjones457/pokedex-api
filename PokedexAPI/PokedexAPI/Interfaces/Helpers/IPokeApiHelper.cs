namespace PokedexAPI.Interfaces.Helpers
{
    public interface IPokeApiHelper
    {
        /// <summary>
        /// Hit the PokeAPI pokemon species and return the response
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public Task<string> GetPokemonSpeciesResponse(string pokemon);
    }
}
