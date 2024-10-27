using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LinCsharp.Utils.Extensions
{
    [JsonSerializable(typeof(JsonElement))]
    public partial class TranslationContext : JsonSerializerContext
    {
    }

    public static class Extensions
    {
        [RequiresUnreferencedCode("ToTranslatedText")]
        [UnconditionalSuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "<Pending>")]
        public static string ToTranslatedText(this string json)
        {
            StringBuilder translatedText = new StringBuilder();
            var data = JsonSerializer.Deserialize<JsonElement>(json, new JsonSerializerOptions { TypeInfoResolver = TranslationContext.Default });

            if (data.GetArrayLength() == 0)
                return string.Empty;

            var translations = data[0];

            for (int i = 0; i < translations.GetArrayLength(); i++)
            {
                string text = translations[i][0].ToString();
                if (text.Contains('\n'))
                    translatedText.Append($"{text} \n");
                else
                    translatedText.Append($"{text}");
            }

            return translatedText.ToString();
        }
    }
}
