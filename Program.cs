using LinCsharp.Hooks;
using System.Runtime.Versioning;
using System.Windows.Forms;
using WindowsInput;

namespace LinCsharp
{
    [SupportedOSPlatform("windows")]
    public class Program
    {
        private static InputSimulator _simulator = new InputSimulator();
        private static string _languageFrom;
        private static string _languageTo;
        static void Main()
        {
            Console.Title = "Buffer Translator | https://t.me/larkliy";


            Console.Write("Write the abbreviation of the language from which you want to translate: ");
            _languageFrom = Console.ReadLine() ?? "ru";
            Console.Write("Write the abbreviation of the language you want to translate into: ");
            _languageTo = Console.ReadLine() ?? "en";

            Console.Clear();

            Console.WriteLine("Clipboard watcher started. Press Ctrl+C to exit.");

            ClipboardNotification.ClipboardUpdate += async (sender, e) => await OnClipboardUpdate();

            var translatorThread = new Thread(() =>
            {
                ClipboardNotification.RegisterClipboardListener();
                Application.Run();
            });

            translatorThread.SetApartmentState(ApartmentState.STA);
            translatorThread.Start();

            Console.ReadLine();
        }

        private static async Task OnClipboardUpdate()
        {
            if (Clipboard.ContainsText())
            {
                string originalText = Clipboard.GetText();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Original text: {originalText}");
                Console.ResetColor();

                using var translator = new Translator.Translator(_languageFrom, _languageTo);
                var (translateText, translatedText) = await translator.Translate(originalText);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Translated text: {translatedText}");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Translated text succesfully pasted.");
                Console.ResetColor();

                SimulateTextEntry(translatedText);

                Console.WriteLine(new string('-', 100));
            }
        }

        private static void SimulateTextEntry(string text)
        {
            _simulator.Keyboard.TextEntry(text);
        }
    }
}
