

using LinCsharp.Utils;
using LinCsharp.Utils.Extensions;

namespace LinCsharp.Translator
{
    public class Translator : IDisposable
    {
        private readonly string translated;
        private readonly string translatedTo;

        private readonly HttpClient _client;

        public Translator(string translated, string translatedTo)
        {
            this.translated = translated;
            this.translatedTo = translatedTo;

            _client = new HttpClient();
            _client.BaseAddress = new(Constants.BaseTranslateAddres);

        }

        public Translator(TranslationParameters parameters) :
            this(parameters.TranslatedLanguage, parameters.TranslatedTo)
        {
            _client = new HttpClient();
            _client.BaseAddress = new(Constants.BaseTranslateAddres);
        }

        public Translator() : this("ru", "en")
        {
            _client = new HttpClient();
            _client.BaseAddress = new(Constants.BaseTranslateAddres);
        }

        public async Task<(string translateText, string translatedText)> Translate(string text)
        {
            string response = await _client.GetStringAsync($"single?client=gtx&sl={translated}&tl={translatedTo}&dt=t&q={Uri.EscapeDataString(text)}");
            string translatedText = response.ToTranslatedText();

            return (text, translatedText);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
