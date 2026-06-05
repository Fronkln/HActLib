using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMNEdit.Windows;
using HActLib.OOE;

namespace CMNEdit
{
    internal static class EffectWindowParticle
    {
        public static void Draw(Form1 form, EffectParticle particle)
        {
            form.CreateHeader("Particle");

            form.CreateInput("Particle ID", particle.ParticleID.ToString(), delegate (string val) { particle.ParticleID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Flag", ((uint)particle.Flag).ToString(), delegate (string val) { particle.Flag = (EffectParticleFlags)uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Unknown 2", particle.Unknown2.ToString(), delegate (string val) { particle.Unknown2 = int.Parse(val); }, NumberBox.NumberMode.UInt);

            form.CreateInput("Scale X", particle.Scale.x.ToString(CultureInfo.InvariantCulture), delegate (string val) { particle.Scale.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Scale Y", particle.Scale.y.ToString(CultureInfo.InvariantCulture), delegate (string val) { particle.Scale.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Scale Z", particle.Scale.z.ToString(CultureInfo.InvariantCulture), delegate (string val) { particle.Scale.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Unknown 4", particle.Unknown4.ToString(), delegate (string val) { particle.Unknown4 = int.Parse(val); }, NumberBox.NumberMode.UInt);

            form.CreateInput("Unknown 5 X", particle.Unknown5.x.ToString(CultureInfo.InvariantCulture), delegate (string val) { particle.Unknown5.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Unknown 5 Y", particle.Unknown5.y.ToString(CultureInfo.InvariantCulture), delegate (string val) { particle.Unknown5.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Unknown 5 Z", particle.Unknown5.z.ToString(CultureInfo.InvariantCulture), delegate (string val) { particle.Unknown5.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Unknown 6", particle.Unknown6.ToString(), delegate (string val) { particle.Unknown6 = int.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Unknown 7", particle.Unknown7.ToString(), delegate (string val) { particle.Unknown7 = int.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Unknown 8", particle.Unknown8.ToString(), delegate (string val) { particle.Unknown8 = int.Parse(val); }, NumberBox.NumberMode.UInt);

            MatrixWindow.Draw(form, particle.Matrix);
        }
    }
}
