using LinCsharp.Native;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace LinCsharp.Hooks
{
    [SupportedOSPlatform("windows")]
    public sealed class ClipboardNotification
    {
        public static event EventHandler ClipboardUpdate;

        private static NotificationWindow _window;

        public static void RegisterClipboardListener()
        {
            _window = new NotificationWindow();
            NativeMethods.AddClipboardFormatListener(_window.Handle);
        }

        private static void OnClipboardUpdate(EventArgs e)
        {
            ClipboardUpdate?.Invoke(null, e);
        }

        private class NotificationWindow : NativeWindow
        {
            private const int WM_CLIPBOARDUPDATE = 0x031D;

            public NotificationWindow()
            {
                CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_CLIPBOARDUPDATE)
                {
                    OnClipboardUpdate(null);
                }
                base.WndProc(ref m);
            }
        }
    }
}
