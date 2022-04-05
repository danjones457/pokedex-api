using Microsoft.AspNetCore.Mvc;
using PokedexAPI.Models;

namespace PokedexAPI.Controllers
{
    [ApiController]
    [Route("pokemon/translated")]
    public class PokemonTranslatedController : ControllerBase
    {
        private readonly ILogger<PokemonTranslatedController> _logger;

        public PokemonTranslatedController(ILogger<PokemonTranslatedController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Endpoint to fetch Pokemon information based on the name of the passed Pokemon 
        /// with a translated description dependant on the habitat and legendary status of the Pokemon
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{pokemon}")]
        public Pokemon Get(string pokemon)
        {
            return new Pokemon();
        }
    }
}