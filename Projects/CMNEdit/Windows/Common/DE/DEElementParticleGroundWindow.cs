using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementParticleGroundWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementParticleGround ground = node as DEElementParticleGround;

            form.CreateHeader("Particle Ground");

            form.CreateInput("Scale X", ground.Scale.x.ToString(), delegate (string val) { ground.Scale.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Scale Y", ground.Scale.y.ToString(), delegate (string val) { ground.Scale.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Scale Z", ground.Scale.z.ToString(), delegate (string val) { ground.Scale.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            RGBA32Window.Draw(form, ground.Color);


            form.CreateInput("Alpha Scale", ground.AlphaScale.ToString(), delegate (string val) { ground.AlphaScale = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Time Scale", ground.TimeScale.ToString(), delegate (string val) { ground.TimeScale = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
        }
    }
}
