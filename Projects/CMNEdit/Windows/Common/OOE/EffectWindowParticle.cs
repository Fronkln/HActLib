﻿using System;
using System.Collections.Generic;
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
            form.CreateInput("Flag", particle.Flag.ToString(), delegate (string val) { particle.Flag = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Unknown", particle.Unknown2.ToString(), delegate (string val) { particle.Unknown2 = int.Parse(val); }, NumberBox.NumberMode.UInt);
            MatrixWindow.Draw(form, particle.Matrix);
        }
    }
}
