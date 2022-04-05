using Newtonsoft.Json.Linq;
using PokedexAPI.Interfaces;
using PokedexAPI.Models;

namespace PokedexAPI.Handlers
{
    public class PokemonHandler : IPokemonHandler
    {
        private readonly ILogger<PokemonHandler> _logger;
        private readonly IConfiguration _configuration;

        public PokemonHandler(ILogger<PokemonHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<Pokemon> GetPokemon(string pokemon)
        {
            using var client = new HttpClient();
            var pokeApiUrl = _configuration["PokeApiUrl"];

            try
            {
                var pokeApiResponse = await client.GetStringAsync(pokeApiUrl + "/pokemon-species/" + pokemon);

                var formattedResponse = JObject.Parse(pokeApiResponse);

                var habitat = formattedResponse.SelectToken("habitat.name").Value<string>();
                var isLegendary = formattedResponse.SelectToken("is_legendary").Value<bool>();

                var descriptions = formattedResponse.SelectToken("flavor_text_entries").Value<JArray>();
                var descriptionsList = descriptions.ToObject<List<JObject>>();
                var enDescriptionObject = descriptionsList.FirstOrDefault(description => description.SelectToken("language.name").Value<string>() == "en");
                var enDescription = "";

                if (enDescriptionObject != null)
                {
                    enDescription = enDescriptionObject.Value<string>("flavor_text");
                    enDescription = enDescription.Replace("\n", " ");
                    enDescription = enDescription.Replace("\f", " ");
                }

                return new Pokemon
                {
                    Name = pokemon,
                    Description = enDescription,
                    Habitat = habitat,
                    IsLegendary = isLegendary,
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
