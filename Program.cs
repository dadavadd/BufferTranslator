using LinCsharp.Hooks;
using LinCsharp.Tranlator;
using System.Runtime.Versioning;
using System.Windows.Forms;
using WindowsInput;

namespace LinCsharp
{
    [SupportedOSPlatform("windows")]
    public class Program
    {
        private static InputSimulator _simulator = new InputSimulator();
        static void Main()
        {
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
                Console.WriteLine($"Original text: {originalText}");
                Console.ResetColor();

                using var translator = new Translator();

                var (translateText, translatedText) = await translator.Translate(originalText);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Translated text: {translatedText}");
                Console.WriteLine("Translated text set to clipboard.");

                SimulateTextEntry(translatedText);

            }
        }

        private static void SimulateTextEntry(string text)
        {
            _simulator.Keyboard.TextEntry(text);
        }
    }
}
