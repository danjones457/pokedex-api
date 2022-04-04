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

        [HttpGet(Name = "GetTranslatedPokemon")]
        public Pokemon Get(string pokemon)
        {
            return new Pokemon();
        }
    }
}