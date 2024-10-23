using System.Text;
using System.Text.Json;

namespace LinCsharp.Utils.Extensions
{
    public static class Extensions
    {
        public static string ToTranslatedText(this string json)
        {
            StringBuilder translatedText = new StringBuilder();

            var data = JsonSerializer.Deserialize<dynamic>(json);

            int i = 0;

            try
            {
                while (true)
                {
                    string text = data[0][i][0].ToString();

                    if (text.Contains('\n'))
                        translatedText.Append($"{text} \n");

                    translatedText.Append($"{text}");

                    i++;
                }
            }
            catch (IndexOutOfRangeException)
            {
                return translatedText.ToString();
            }
        }
    }
}
