using Newtonsoft.Json.Linq;
using PokedexAPI.Interfaces.Helpers;
using System.Text.RegularExpressions;

namespace PokedexAPI.Helpers
{
    public class TranslationsHelper : ITranslationsHelper
    {
        private readonly ILogger<TranslationsHelper> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public TranslationsHelper(ILogger<TranslationsHelper> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// See <see cref="ITranslationsHelper.TranslateToShakespeare(string)"/>
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<string> TranslateToShakespeare(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentNullException(nameof(description));

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
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentNullException(nameof(description));

            var yodaUrl = _configuration["TranslatorApiUrl:Yoda"];

            return await GetTranslation(description, yodaUrl);
        }

        /// <summary>
        /// Hit the translation API with the passed description text
        /// </summary>
        /// <param name="description"></param>
        /// <param name="translationUrl"></param>
        /// <returns></returns>
        private async Task<string> GetTranslation(string description, string translationUrl)
        {
            try
            {
                var endcodedDescription = System.Web.HttpUtility.UrlEncode(description);
                var response = await _httpClient.GetStringAsync(translationUrl + "?text=" + endcodedDescription);

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
