using System.Net.Http;
using Newtonsoft.Json;
using PokedexAPI.Models;
using Xunit;

namespace Tests.Integration
{
    public class PokemonTests
    {
        private readonly HttpClient Client;
        public PokemonTests()
        {
            var server = new MockPokedexApi();
            Client = server.CreateClient();
        }

        [Theory]
        [InlineData("mewtwo", "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.", "rare", true)]
        [InlineData("pikachu", "When several of these POKÈMON gather, their electricity could build and cause lightning storms.", "forest", false)]
        [InlineData("ditto", "Capable of copying an enemy's genetic code to instantly transform itself into a duplicate of the enemy.", "urban", false)]
        public async void Test_Get_Valid_Pokemon_Is_Successful(string name, string description, string habitat, bool isLegendary)
        {
            var response = await Client.GetAsync("/pokemon/" + name);

            response.EnsureSuccessStatusCode();

            var formattedResponse = await response.Content.ReadAsStringAsync();

            var pokemonResponse = JsonConvert.DeserializeObject<Pokemon>(formattedResponse);

            Assert.Equal(name, pokemonResponse.Name);
            Assert.Equal(description, pokemonResponse.Description);
            Assert.Equal(habitat, pokemonResponse.Habitat);
            Assert.Equal(isLegendary, pokemonResponse.IsLegendary);
        }
    }
}