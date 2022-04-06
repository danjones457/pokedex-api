using Microsoft.Extensions.Logging;
using Moq;
using PokedexAPI.Handlers;
using PokedexAPI.Interfaces.Handlers;
using PokedexAPI.Interfaces.Helpers;
using PokedexAPI.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit.Handlers
{
    public class PokemonTranslatedHandlerTests
    {
        private readonly Mock<ILogger<PokemonTranslatedHandler>> _logger;
        private readonly Mock<IPokemonHandler> _pokemonHandler;
        private readonly Mock<IPokemonTranslatedHelper> _pokemonTranslatedHelper;

        public PokemonTranslatedHandlerTests()
        {
            _logger = new Mock<ILogger<PokemonTranslatedHandler>>();
            _pokemonHandler = new Mock<IPokemonHandler>();
            _pokemonTranslatedHelper = new Mock<IPokemonTranslatedHelper>();
        }

        [Fact]
        public void PokemonTranslatedHandler_Should_Throw_When_Logger_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PokemonTranslatedHandler(null, _pokemonHandler.Object, _pokemonTranslatedHelper.Object));
        }

        [Fact]
        public void PokemonTranslatedHandler_Should_Throw_When_PokemonHandler_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PokemonTranslatedHandler(_logger.Object, null, _pokemonTranslatedHelper.Object));
        }

        [Fact]
        public void PokemonTranslatedHandler_Should_Throw_When_PokemonTranslatedHelper_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new PokemonTranslatedHandler(_logger.Object, _pokemonHandler.Object, null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public async Task PokemonTranslatedHandler_Should_Throw_When_Pokemon_Name_Is_Null_Or_Whitespace(dynamic pokemonName)
        {
            var handler = new PokemonTranslatedHandler(_logger.Object, _pokemonHandler.Object, _pokemonTranslatedHelper.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.GetTranslatedPokemon(pokemonName));
        }

        [Fact]
        public async Task PokemonTranslatedHandler_Should_Return_Valid_Pokemon_When_Valid_String_Passed()
        {
            var pokemonName = "test name";

            var expectedResult = new Pokemon
            {
                Name = pokemonName,
                Description = "test description",
                Habitat = "cave",
                IsLegendary = true,
            };

            _pokemonHandler.Setup(x => x.GetPokemon(It.IsAny<string>())).Returns(Task.FromResult(expectedResult));
            _pokemonTranslatedHelper.Setup(x => x.GetTranslatedPokemon(It.IsAny<Pokemon>())).Returns(Task.FromResult(expectedResult));

            var handler = new PokemonTranslatedHandler(_logger.Object, _pokemonHandler.Object, _pokemonTranslatedHelper.Object);
            var pokemonResponse = await handler.GetTranslatedPokemon(pokemonName);

            _pokemonHandler.Verify(x => x.GetPokemon(It.Is<string>(x => x == pokemonName)), Times.Once());
            _pokemonTranslatedHelper.Verify(x => x.GetTranslatedPokemon(It.Is<Pokemon>(x => x == expectedResult)), Times.Once());
            Assert.Equal(expectedResult, pokemonResponse);
        }
    }
}
