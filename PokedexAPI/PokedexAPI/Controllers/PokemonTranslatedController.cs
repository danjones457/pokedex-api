using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PokedexAPI.Interfaces.Handlers;

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
        public async Task<ActionResult> Get(string pokemon)
        {
            try
            {
                var pokemonResponse = await _pokemonTranslatedHandler.GetTranslatedPokemon(pokemon);
                return Ok(JsonConvert.SerializeObject(pokemonResponse));

            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });
            }
        }
    }
}