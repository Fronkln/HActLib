using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib.OOE;

namespace CMNEdit
{
    internal static class EffectWindowParticle
    {
        public static void Draw(Form1 form, EffectParticle particle)
        {
            form.CreateHeader("Particle");

            form.CreateInput("Particle ID", particle.ParticleID.ToString(), delegate (string val) { particle.ParticleID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
