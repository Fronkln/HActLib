using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OEParticleWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEParticle particle = node as OEParticle;

            form.CreateHeader("Particle");
            form.CreateInput("Particle ID", particle.ParticleID.ToString(), delegate (string val) { particle.ParticleID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Unknown", particle.Unknown1.ToString(), delegate (string val) { particle.Unknown1 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
