using Microsoft.AspNetCore.Mvc;
using PokedexAPI.Models;

namespace PokedexAPI.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(ILogger<PokemonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{pokemon}")]
        public Pokemon Get(string pokemon)
        {
            return new Pokemon();
        }
    }
}