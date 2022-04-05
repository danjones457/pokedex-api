using Newtonsoft.Json.Linq;
using PokedexAPI.Interfaces.Helpers;
using PokedexAPI.Models;

namespace PokedexAPI.Helpers
{
    public class PokeApiToPokemonHelper : IPokeApiToPokemonHelper
    {
        /// <summary>
        /// See <see cref="IPokeApiToPokemonHelper.ConvertPokeApiResponseToPokemon(string, string)"/>
        /// </summary>
        /// <param name="pokemon"></param>
        /// <param name="repsonse"></param>
        /// <returns></returns>
        public Pokemon ConvertPokeApiResponseToPokemon(string pokemon, string repsonse)
        {
            var formattedResponse = JObject.Parse(repsonse);

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
    }
}
