using System;
using System.Drawing;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementParticleWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementParticle particle = node as DEElementParticle;

            form.CreateHeader("Particle");
            form.CreateInput("ID", particle.ParticleID.ToString(), delegate (string val) { particle.ParticleID = uint.Parse(val); }, NumberBox.NumberMode.UInt);

            form.CreateInput("Near Fade Distance", particle.NearFadeDistance.ToString(), delegate (string val) { particle.NearFadeDistance = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Near Fade Offset", particle.NearFadeOffset.ToString(), delegate (string val) { particle.NearFadeOffset = float.Parse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Scale X", particle.Scale.x.ToString(), delegate (string val) {particle.Scale.x = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Scale Y", particle.Scale.y.ToString(), delegate (string val) { particle.Scale.y = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Scale Z", particle.Scale.z.ToString(), delegate (string val) { particle.Scale.z = float.Parse(val); }, NumberBox.NumberMode.Float);



            form.CreateInput("Position X", particle.Matrix.VM3.x.ToString(), delegate (string val) { particle.Matrix.VM3.x = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Position Y", particle.Matrix.VM3.y.ToString(), delegate (string val) { particle.Matrix.VM3.y = float.Parse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Position Z", particle.Matrix.VM3.z.ToString(), delegate (string val) { particle.Matrix.VM3.z = float.Parse(val); }, NumberBox.NumberMode.Float);



            Panel particleColPanel = null;
            particleColPanel = form.CreatePanel("Particle Color", particle.Color,
                delegate (Color col)
                {
                    particle.Color = col;
                    particleColPanel.BackColor = col;
                });
        }
    }
}
