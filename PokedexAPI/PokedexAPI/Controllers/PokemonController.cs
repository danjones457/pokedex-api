using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PokedexAPI.Interfaces.Handlers;
using PokedexAPI.Models;

namespace PokedexAPI.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonHandler _pokemonHandler;

        public PokemonController(ILogger<PokemonController> logger, IPokemonHandler pokemonHandler)
        {
            _logger = logger;
            _pokemonHandler = pokemonHandler;
        }

        /// <summary>
        /// Endpoint to fetch Pokemon information based on the name of the passed Pokemon
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pokemon}")]
        public async Task<ActionResult> GetAsync(string pokemon)
        {
            try
            {
                var pokemonResponse = await _pokemonHandler.GetPokemon(pokemon);
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