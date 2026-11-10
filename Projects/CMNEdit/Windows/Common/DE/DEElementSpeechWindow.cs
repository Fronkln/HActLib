using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementSpeechWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementSpeech speech = node as DEElementSpeech;

            PropertyEditorBuilder.CreateHeader("Speech");

            PropertyEditorBuilder.CreateInput("Version", speech.SpeechVersion.ToString(), delegate (string val) { speech.SpeechVersion = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateInput("Cuesheet ID", speech.SpeechCuesheet.ToString(), delegate (string val) { speech.SpeechCuesheet = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            PropertyEditorBuilder.CreateInput("Sound ID", speech.SpeechID.ToString(), delegate (string val) { speech.SpeechID = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);

        }
    }
}
