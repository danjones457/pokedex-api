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

        [Fact]
        public async void Test_Get_Mewtwo_Is_Successful()
        {
            var response = await Client.GetAsync("/pokemon/mewtwo");

            response.EnsureSuccessStatusCode();

            var formattedResponse = await response.Content.ReadAsStringAsync();

            var pokemonResponse = JsonConvert.DeserializeObject<Pokemon>(formattedResponse);

            Assert.Equal("mewtwo", pokemonResponse.Name);
            Assert.Equal("It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.", pokemonResponse.Description);
            Assert.Equal("rare", pokemonResponse.Habitat);
            Assert.True(pokemonResponse.IsLegendary);
        }
    }
}