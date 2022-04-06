using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokedexAPI.Models;
using System;
using System.Net.Http;
using Xunit;

namespace Tests.Integration
{
    public class PokemonTranslatedTests
    {
        private readonly HttpClient Client;

        public PokemonTranslatedTests()
        {
            var server = new MockPokedexApi();
            Client = server.CreateClient();
        }

        [Theory]
        [InlineData(
            "mewtwo",
            "Created by a scientist after years of horrific gene splicing and dna engineering experiments, it was.",
            "rare",
            true)]
        [InlineData(
            "zubat",
            "Forms colonies in perpetually dark places.Ultrasonic waves to identify and approach targets, uses.",
            "cave",
            false)]
        [InlineData(
            "registeel",
            "A body that is harder than any kind of metal, registeel has.Apparently hollow, its body is.Any idea what this pokémon eats, no one has.",
            "cave",
            true)]
        public async void Test_Get_Valid_Translated_Pokemon_For_Yoda_Translation_Is_Successful(string name, string description, string habitat, bool isLegendary)
        {
            var response = await Client.GetAsync("/pokemon/translated/" + name);

            response.EnsureSuccessStatusCode();

            var formattedResponse = await response.Content.ReadAsStringAsync();

            var pokemonResponse = JsonConvert.DeserializeObject<Pokemon>(formattedResponse);

            Assert.Equal(name, pokemonResponse.Name);
            Assert.Equal(description, pokemonResponse.Description);
            Assert.Equal(habitat, pokemonResponse.Habitat);
            Assert.Equal(isLegendary, pokemonResponse.IsLegendary);
        }

        [Theory]
        [InlineData("rattata", "Bites aught at which hour 't attacks. Bawbling and very quick, 't is a ingraft sight in many places.", "grassland", false)]
        [InlineData("caterpie", "Its short feet art tipped with suction pads yond enable 't to tirelessly climb slopes and walls.", "forest", false)]
        public async void Test_Get_Valid_Translated_Pokemon_For_Shakespeare_Translation_Is_Successful(string name, string description, string habitat, bool isLegendary)
        {
            var response = await Client.GetAsync("/pokemon/translated/" + name);

            response.EnsureSuccessStatusCode();

            var formattedResponse = await response.Content.ReadAsStringAsync();

            var pokemonResponse = JsonConvert.DeserializeObject<Pokemon>(formattedResponse);

            Assert.Equal(name, pokemonResponse.Name);
            Assert.Equal(description, pokemonResponse.Description);
            Assert.Equal(habitat, pokemonResponse.Habitat);
            Assert.Equal(isLegendary, pokemonResponse.IsLegendary);
        }

        [Fact]
        public async void Test_Get_Invalid_Translated_Pokemon_Is_Not_Successful()
        {
            var response = await Client.GetAsync("/pokemon/translated/not-a-pokemon");
            var responseContent = await response.Content.ReadAsStringAsync();
            var formattedResponse = JObject.Parse(responseContent);
            var responseMessage = formattedResponse.Value<string>("message");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("We were unable to find that Pokemon.", responseMessage);
        }

        [Fact]
        public async void Test_Get_No_Translated_Pokemon_Is_Not_Successful()
        {
            var response = await Client.GetAsync("/pokemon/translated");
            var responseContent = await response.Content.ReadAsStringAsync();
            var formattedResponse = JObject.Parse(responseContent);
            var responseMessage = formattedResponse.Value<string>("message");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("We were unable to find that Pokemon.", responseMessage);
        }
    }
}