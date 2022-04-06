using Microsoft.AspNetCore.Mvc;
using PokedexAPI.Interfaces.Handlers;
using PokedexAPI.Models;

namespace PokedexAPI.Controllers
{
    [ApiController]
    [Route("pokemon/translated")]
    public class PokemonTranslatedController : ControllerBase
    {
        private readonly ILogger<PokemonTranslatedController> _logger;
        private readonly IPokemonTranslatedHandler _pokemonTranslatedHandler;

        public PokemonTranslatedController(ILogger<PokemonTranslatedController> logger, IPokemonTranslatedHandler pokemonTranslatedHandler)
        {
            _logger = logger;
            _pokemonTranslatedHandler = pokemonTranslatedHandler;
        }

        /// <summary>
        /// Endpoint to fetch Pokemon information based on the name of the passed Pokemon 
        /// with a translated description dependant on the habitat and legendary status of the Pokemon
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pokemon}")]
        public async Task<Pokemon> Get(string pokemon)
        {
            return await _pokemonTranslatedHandler.GetTranslatedPokemon(pokemon);
        }
    }
}