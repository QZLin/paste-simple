using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WK.Libraries.SharpClipboardNS;
using static PasteSimple2.MainForm;
using static WK.Libraries.SharpClipboardNS.SharpClipboard;

namespace PasteSimple2
{
    public class SyncClient
    {
        public SyncServer? server;
        public SharpClipboard clipboard;

        public string lastSetClip = "";
        bool waitClipUpdate = false;
        public SyncClient()
        {
            clipboard = new()
            {
                ObserveLastEntry = false
            };
            clipboard.ClipboardChanged += OnClipBoardChange;
        }
        public void UpdateClipboard(string content)
        {
            Thread STAThread = new(
                    delegate ()
                    {
                        Clipboard.SetText(content, TextDataFormat.UnicodeText);
                    });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            lastSetClip = content;
            waitClipUpdate = true;
            //STAThread.Join();
        }

        private void OnClipBoardChange(Object sender, ClipboardChangedEventArgs e)
        {
            // Is the content copied of text type?
            if (e.ContentType == ContentTypes.Text)
            {
                if (e.Content.ToString() == lastSetClip || waitClipUpdate)
                {
                    Debug.WriteLine("self_clip_update:" + clipboard.ClipboardText);
                    waitClipUpdate = false;
                    return;
                }
                Debug.WriteLine($"clip_update({e.SourceApplication.Name}):{clipboard.ClipboardText}");
                server!.SelfSendSync(clipboard.ClipboardText);
            }

            // Is the content copied of image type?
            else if (e.ContentType == SharpClipboard.ContentTypes.Image)
            {
                // Get the cut/copied image.
                Image img = clipboard.ClipboardImage;
            }

            // Is the content copied of file type?
            else if (e.ContentType == SharpClipboard.ContentTypes.Files)
            {
                // Get the cut/copied file/files.
                Debug.WriteLine(clipboard.ClipboardFiles.ToArray());

                // ...or use 'ClipboardFile' to get a single copied file.
                Debug.WriteLine(clipboard.ClipboardFile);
            }

            // If the cut/copied content is complex, use 'Other'.
            else if (e.ContentType == SharpClipboard.ContentTypes.Other)
            {
                // Do something with 'clipboard.ClipboardObject' or 'e.Content' here...
            }
        }
    }
}

