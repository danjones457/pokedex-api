using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokedexAPI.Helpers;
using PokedexAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Unit.Helpers
{
    public class PokeApiToPokemonHelperTests
    {
        private readonly PokeApiToPokemonHelper _helper;
        public PokeApiToPokemonHelperTests()
        {
            _helper = new PokeApiToPokemonHelper();
        }

        [Fact]
        public void PokeApiToPokemonHelper_Should_Throw_When_Pokemon_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => _helper.ConvertPokeApiResponseToPokemon(null, "test string"));
        }

        [Fact]
        public void PokeApiToPokemonHelper_Should_Throw_When_Response_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => _helper.ConvertPokeApiResponseToPokemon("test string", null));
        }

        [Fact]
        public void PokeApiToPokemonHelper_Should_Return_Pokemon_With_Valid_Parameters()
        {
            var pokemon = "test pokemon";
            var habitat = "test habitat";
            var isLegendary = true;
            var description = "test description";

            var expectedResult = new Pokemon
            {
                Name = pokemon,
                Habitat = habitat,
                Description = description,
                IsLegendary = isLegendary
            };

            var flavor_text_entries = new List<object>
            {
                new { 
                    flavor_text = description,
                    language = new
                    {
                        name = "en"
                    }
                }
            };
            var response = new
            {
                habitat = new
                {
                    name = habitat
                },
                is_legendary = isLegendary,
                flavor_text_entries = flavor_text_entries
            };
            var stringResponse = JsonConvert.SerializeObject(response);

            var helperResponse = _helper.ConvertPokeApiResponseToPokemon(pokemon, stringResponse);

            Assert.Equal(expectedResult.Name, helperResponse.Name);
            Assert.Equal(expectedResult.Habitat, helperResponse.Habitat);
            Assert.Equal(expectedResult.Description, helperResponse.Description);
            Assert.Equal(expectedResult.IsLegendary, helperResponse.IsLegendary);
        }
    }
}
