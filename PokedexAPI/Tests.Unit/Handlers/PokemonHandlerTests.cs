using Microsoft.Extensions.Logging;
using Moq;
using PokedexAPI.Handlers;
using PokedexAPI.Interfaces.Helpers;
using PokedexAPI.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit.Handlers
{
    public class PokemonHandlerTests
    {
        private Mock<ILogger<PokemonHandler>> _logger;
        private Mock<IPokeApiToPokemonHelper> _pokeApiToPokemonHelper;
        private Mock<IPokeApiHelper> _pokeApiHelper;

        public PokemonHandlerTests()
        {
            _logger = new Mock<ILogger<PokemonHandler>>();
            _pokeApiToPokemonHelper = new Mock<IPokeApiToPokemonHelper>();
            _pokeApiHelper = new Mock<IPokeApiHelper>();
        }

        [Fact]
        public void PokemonHandler_Should_Throw_When_Logger_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PokemonHandler(null, _pokeApiToPokemonHelper.Object, _pokeApiHelper.Object));
        }

        [Fact]
        public void PokemonHandler_Should_Throw_When_PokeApiToPokemonHelper_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PokemonHandler(_logger.Object, null, _pokeApiHelper.Object));
        }

        [Fact]
        public void PokemonHandler_Should_Throw_When_PokeApiHelper_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PokemonHandler(_logger.Object, _pokeApiToPokemonHelper.Object, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task PokemonHandler_Should_Throw_When_Pokemon_Name_Is_Null_Or_Whitespace(dynamic pokemonName)
        {
            var handler = new PokemonHandler(_logger.Object, _pokeApiToPokemonHelper.Object, _pokeApiHelper.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.GetPokemon(pokemonName));
        }

        [Fact]
        public async Task PokemonHandler_Should_Return_Valid_Pokemon_When_Valid_String_Passed()
        {
            var pokemonName = "test name";
            var pokemonDescription = "test species response";

            var expectedResult = new Pokemon 
            {
                Name = pokemonName,
                Description = pokemonDescription,
                Habitat = "cave",
                IsLegendary = true,
            };

            _pokeApiHelper.Setup(x => x.GetPokemonSpeciesResponse(It.IsAny<string>())).Returns(Task.FromResult(pokemonDescription));
            _pokeApiToPokemonHelper.Setup(
                x => x.ConvertPokeApiResponseToPokemon(
                    It.IsAny<string>(),
                    It.IsAny<string>())
                ).Returns(expectedResult);

            var handler = new PokemonHandler(_logger.Object, _pokeApiToPokemonHelper.Object, _pokeApiHelper.Object);
            var pokemonResponse = await handler.GetPokemon(pokemonName);

            Assert.Equal(expectedResult, pokemonResponse);
        }
    }
}
