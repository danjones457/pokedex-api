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

        public async Task<string> TranslateToShakespeare(string description)
        {
            try
            {
                using var client = new HttpClient();
                var shakespeareUrl = _configuration["TranslatorApiUrl:Shakespeare"];
                var endcodedDescription = System.Web.HttpUtility.UrlEncode(description);
                var response = await client.GetStringAsync(shakespeareUrl + "?text=" + endcodedDescription);

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

        public Task<string> TranslateToYoda(string description)
        {
            throw new NotImplementedException();
        }
    }
}
