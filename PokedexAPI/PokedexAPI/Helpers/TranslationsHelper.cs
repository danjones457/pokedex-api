using Newtonsoft.Json.Linq;
using PokedexAPI.Interfaces.Helpers;
using System.Text.RegularExpressions;

namespace PokedexAPI.Helpers
{
    public class TranslationsHelper : ITranslationsHelper
    {
        private readonly ILogger<TranslationsHelper> _logger;
        private readonly IConfiguration _configuration;

        public TranslationsHelper(ILogger<TranslationsHelper> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// See <see cref="ITranslationsHelper.TranslateToShakespeare(string)"/>
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<string> TranslateToShakespeare(string description)
        {
            var shakespeareUrl = _configuration["TranslatorApiUrl:Shakespeare"];
            
            return await GetTranslation(description, shakespeareUrl);
        }

        /// <summary>
        /// See <see cref="ITranslationsHelper.TranslateToYoda(string)"/>
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<string> TranslateToYoda(string description)
        {
            var yodaUrl = _configuration["TranslatorApiUrl:Yoda"];

            return await GetTranslation(description, yodaUrl);
        }

        /// <summary>
        /// Hit the translation API with the passed description text
        /// </summary>
        /// <param name="description"></param>
        /// <param name="translationUrl"></param>
        /// <returns></returns>
        private static async Task<string> GetTranslation(string description, string translationUrl)
        {
            try
            {
                using var client = new HttpClient();
                var endcodedDescription = System.Web.HttpUtility.UrlEncode(description);
                var response = await client.GetStringAsync(translationUrl + "?text=" + endcodedDescription);

                var formattedResponse = JObject.Parse(response);
                var translatedText = formattedResponse.SelectToken("contents.translated")?.Value<string>() ?? description;
                translatedText = Regex.Replace(translatedText, @"\s+", " ");

                return translatedText;
            }
            catch (Exception)
            {
                // Log exception here
                return description;
            }
        }
    }
}
