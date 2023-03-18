using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OENodeElementSEWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEElementSE inf = node as OEElementSE;

            form.CreateSpace(25);
            form.CreateHeader("Sound Effect");

            form.CreateInput("Cuesheet ID", inf.Cuesheet.ToString(), delegate (string val) { inf.Cuesheet = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Sound ID", inf.Sound.ToString(), delegate (string val) { inf.Sound = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
            form.CreateInput("Flag?", inf.Flags.ToString(), delegate (string val) { inf.Flags = uint.Parse(val); }, NumberBox.NumberMode.UInt);


            form.CreateInput("Volume?", inf.Volume.ToString(), delegate (string val) { inf.Volume = int.Parse(val); }, NumberBox.NumberMode.Int);
           
            form.CreateInput("Decay Near Distance", inf.CustomDecayNearDist.ToString(), delegate (string val) { inf.CustomDecayNearDist = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Decay Near Volume", inf.CustomDecayNearVol.ToString(), delegate (string val) { inf.CustomDecayNearVol = float.Parse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Decay Far Distance", inf.CustomDecayFarDist.ToString(), delegate (string val) { inf.CustomDecayFarDist = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Decay Far Volume", inf.CustomDecayFarVol.ToString(), delegate (string val) { inf.CustomDecayFarVol = float.Parse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
