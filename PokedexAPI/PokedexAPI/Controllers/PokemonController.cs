using Microsoft.AspNetCore.Mvc;
using PokedexAPI.Interfaces;
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

        [HttpGet]
        [Route("{pokemon}")]
        public async Task<Pokemon> GetAsync(string pokemon)
        {
            return await _pokemonHandler.GetPokemon(pokemon);
        }
    }
}