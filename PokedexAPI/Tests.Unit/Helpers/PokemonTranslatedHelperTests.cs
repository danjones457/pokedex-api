using Microsoft.Extensions.Logging;
using Moq;
using PokedexAPI.Helpers;
using PokedexAPI.Interfaces.Helpers;
using PokedexAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit.Helpers
{
    public class PokemonTranslatedHelperTests
    {
        private readonly Mock<ILogger<PokemonTranslatedHelper>> _logger;
        private readonly Mock<ITranslationsHelper> _translationsHelper;
        private readonly string _yodaTranslation;
        private readonly string _shakespeareTranslation;

        public PokemonTranslatedHelperTests()
        {
            _logger = new Mock<ILogger<PokemonTranslatedHelper>>();
            _translationsHelper = new Mock<ITranslationsHelper>();
            _yodaTranslation = "This is a yoda translation";
            _shakespeareTranslation = "This is a shakespeare translation";

            _translationsHelper.Setup(x => x.TranslateToYoda(It.IsAny<string>())).Returns(Task.FromResult(_yodaTranslation));
            _translationsHelper.Setup(x => x.TranslateToShakespeare(It.IsAny<string>())).Returns(Task.FromResult(_shakespeareTranslation));
        }

        [Fact]
        public async Task PokemonTranslatedHelper_Should_Throw_When_Pokemon_Is_Null()
        {
            var helper = new PokemonTranslatedHelper(_logger.Object, _translationsHelper.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => helper.GetTranslatedPokemon(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task PokemonTranslatedHelper_Should_Return_Untranslated_Pokemon_If_Description_Null_Or_Empty(string description)
        {
            var helper = new PokemonTranslatedHelper(_logger.Object, _translationsHelper.Object);

            var pokemon = new Pokemon
            {
                Name = "test name",
                Description = description,
                Habitat = "test habitat",
                IsLegendary = true
            };

            var response = await helper.GetTranslatedPokemon(pokemon);

            Assert.Equal(pokemon.Name, response.Name);
            Assert.Equal(pokemon.Habitat, response.Habitat);
            Assert.Equal(pokemon.IsLegendary, response.IsLegendary);
            Assert.Equal(description, response.Description);

            _translationsHelper.Verify(x => x.TranslateToYoda(It.IsAny<string>()), Times.Never);
            _translationsHelper.Verify(x => x.TranslateToShakespeare(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task PokemonTranslatedHelper_Should_Return_Yoda_Translated_Description_If_Habitat_Cave()
        {
            var helper = new PokemonTranslatedHelper(_logger.Object, _translationsHelper.Object);

            var pokemon = new Pokemon
            {
                Name = "test name",
                Description = "test description",
                Habitat = "cave",
                IsLegendary = false
            };

            var response = await helper.GetTranslatedPokemon(pokemon);

            Assert.Equal(pokemon.Name, response.Name);
            Assert.Equal(pokemon.Habitat, response.Habitat);
            Assert.Equal(pokemon.IsLegendary, response.IsLegendary);
            Assert.Equal(_yodaTranslation, response.Description);

            _translationsHelper.Verify(x => x.TranslateToYoda(It.IsAny<string>()), Times.Once);
            _translationsHelper.Verify(x => x.TranslateToShakespeare(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task PokemonTranslatedHelper_Should_Return_Yoda_Translated_Description_If_IsLegendary()
        {
            var helper = new PokemonTranslatedHelper(_logger.Object, _translationsHelper.Object);

            var pokemon = new Pokemon
            {
                Name = "test name",
                Description = "test description",
                Habitat = "test habitat",
                IsLegendary = true
            };

            var response = await helper.GetTranslatedPokemon(pokemon);

            Assert.Equal(pokemon.Name, response.Name);
            Assert.Equal(pokemon.Habitat, response.Habitat);
            Assert.Equal(pokemon.IsLegendary, response.IsLegendary);
            Assert.Equal(_yodaTranslation, response.Description);

            _translationsHelper.Verify(x => x.TranslateToYoda(It.IsAny<string>()), Times.Once);
            _translationsHelper.Verify(x => x.TranslateToShakespeare(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task PokemonTranslatedHelper_Should_Return_Yoda_Translated_Description_If_IsLegendary_And_Habitat_Cave()
        {
            var helper = new PokemonTranslatedHelper(_logger.Object, _translationsHelper.Object);

            var pokemon = new Pokemon
            {
                Name = "test name",
                Description = "test description",
                Habitat = "cave",
                IsLegendary = true
            };

            var response = await helper.GetTranslatedPokemon(pokemon);

            Assert.Equal(pokemon.Name, response.Name);
            Assert.Equal(pokemon.Habitat, response.Habitat);
            Assert.Equal(pokemon.IsLegendary, response.IsLegendary);
            Assert.Equal(_yodaTranslation, response.Description);

            _translationsHelper.Verify(x => x.TranslateToYoda(It.IsAny<string>()), Times.Once);
            _translationsHelper.Verify(x => x.TranslateToShakespeare(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task PokemonTranslatedHelper_Should_Return_Shakespeare_Translated_Description_If_Not_IsLegendary_And_Habitat_Not_Cave()
        {
            var helper = new PokemonTranslatedHelper(_logger.Object, _translationsHelper.Object);

            var pokemon = new Pokemon
            {
                Name = "test name",
                Description = "test description",
                Habitat = "test habitat",
                IsLegendary = false
            };

            var response = await helper.GetTranslatedPokemon(pokemon);

            Assert.Equal(pokemon.Name, response.Name);
            Assert.Equal(pokemon.Habitat, response.Habitat);
            Assert.Equal(pokemon.IsLegendary, response.IsLegendary);
            Assert.Equal(_shakespeareTranslation, response.Description);

            _translationsHelper.Verify(x => x.TranslateToYoda(It.IsAny<string>()), Times.Never);
            _translationsHelper.Verify(x => x.TranslateToShakespeare(It.IsAny<string>()), Times.Once);
        }
    }
}
