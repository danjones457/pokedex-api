namespace PokedexAPI.Interfaces.Helpers
{
    public interface ITranslationsHelper
    {
        /// <summary>
        /// Translate a description using the Yoda API
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Task<string> TranslateToYoda(string description);

        /// <summary>
        /// Translate a description using the Shakespeare API
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Task<string> TranslateToShakespeare(string description);
    }
}
