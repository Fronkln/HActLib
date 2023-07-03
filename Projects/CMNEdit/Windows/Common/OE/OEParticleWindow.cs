using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    internal static class OEParticleWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEParticle particle = node as OEParticle;

            Panel colorPanel = null;

            form.CreateHeader("Particle");
            form.CreateInput("Particle ID", particle.ParticleID.ToString(), delegate (string val) { particle.ParticleID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Unknown", particle.Unknown.ToString(), delegate (string val) { particle.Unknown = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", particle.Unknown2.ToString(), delegate (string val) { particle.Unknown2 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", particle.Unknown3.ToString(), delegate (string val) { particle.Unknown3 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", particle.Unknown4.ToString(), delegate (string val) { particle.Unknown4 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", particle.Unknown5.ToString(), delegate (string val) { particle.Unknown5 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", particle.Unknown6.ToString(), delegate (string val) { particle.Unknown6 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", particle.Unknown7.ToString(), delegate (string val) { particle.Unknown7 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Scale X", particle.Scale.x.ToString(), delegate (string val) { particle.Scale.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Scale Y", particle.Scale.x.ToString(), delegate (string val) { particle.Scale.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Scale Z", particle.Scale.x.ToString(), delegate (string val) { particle.Scale.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);


            colorPanel = form.CreatePanel("Color", particle.Color,
                delegate (Color col)
                {
                    particle.Color = col;
                    colorPanel.BackColor = col;
                });

            form.CreateInput("Transparency", particle.Color.a.ToString(), delegate (string val) { particle.Color.a = byte.Parse(val); }, NumberBox.NumberMode.Byte);
        }
    }
}
