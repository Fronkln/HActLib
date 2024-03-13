using HActLib.OOE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMNEdit
{
    internal static class EffectWindowSound
    {
        public static void Draw(Form1 form, EffectSound sound)
        {
            form.CreateHeader("Sound");

            form.CreateInput("Start", sound.Start.ToString(), delegate (string val) { sound.Start = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("End", sound.End.ToString(), delegate (string val) { sound.End = int.Parse(val); }, NumberBox.NumberMode.Int);

            form.CreateInput("Cue ID", sound.CuesheetID.ToString(), delegate (string val) { sound.CuesheetID = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Sound ID", sound.SoundID.ToString(), delegate (string val) { sound.SoundID = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
        }
    }
}
