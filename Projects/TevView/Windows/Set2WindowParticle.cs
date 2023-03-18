using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib.OOE;

namespace TevView
{
    internal static class Set2WindowParticle
    {
        public static void Draw(Form1 form, Set2ElementParticle particle)
        {
            form.CreateHeader("Particle");

            form.CreateInput("Particle ID", particle.ParticleID.ToString(), delegate (string val) { particle.ParticleID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
