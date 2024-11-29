using System;
using System.Drawing;
using System.Windows.Forms;
using HActLib;
using CMNEdit.Windows;

namespace CMNEdit
{
    internal static class DEElementParticleWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementParticle particle = node as DEElementParticle;

            form.CreateHeader("Particle");
            form.CreateInput("ID", particle.ParticleID.ToString(), delegate (string val) { particle.ParticleID = uint.Parse(val); Form1.EditingNode.Update(); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Name", particle.ParticleName.ToString(), delegate (string val) { particle.ParticleName = val; });

            form.CreateInput("Near Fade Distance", particle.NearFadeDistance.ToString(), delegate (string val) { particle.NearFadeDistance = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Near Fade Offset", particle.NearFadeOffset.ToString(), delegate (string val) { particle.NearFadeOffset = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Particle Flags", particle.ParticleFlag.ToString(), delegate (string val) { particle.ParticleFlag = uint.Parse(val); Form1.EditingNode.Update(); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Tick Scale", particle.TickScale.ToString(), delegate (string val) { particle.TickScale = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Scale X", particle.Scale.x.ToString(), delegate (string val) {particle.Scale.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Scale Y", particle.Scale.y.ToString(), delegate (string val) { particle.Scale.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Scale Z", particle.Scale.z.ToString(), delegate (string val) { particle.Scale.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Position X", particle.Matrix.VM3.x.ToString(), delegate (string val) { particle.Matrix.VM3.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Position Y", particle.Matrix.VM3.y.ToString(), delegate (string val) { particle.Matrix.VM3.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Position Z", particle.Matrix.VM3.z.ToString(), delegate (string val) { particle.Matrix.VM3.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Color Scale A", particle.ColorScaleA.ToString(), delegate (string val) { particle.ColorScaleA = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            Panel particleColPanel = null;
            particleColPanel = form.CreatePanel("Color", particle.Color,
                delegate (Color col)
                {
                    particle.Color = col;
                    particleColPanel.BackColor = col;
                });

            form.CreateInput("Color A", particle.Color.a.ToString(), delegate (string val) { particle.Color.a = byte.Parse(val); }, NumberBox.NumberMode.Byte);

            form.CreateButton("Alpha Curve", delegate
            {
                CurveView myNewForm = new CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(particle.Animation,
                    delegate (byte[] outCurve)
                    {
                        particle.Animation = outCurve;
                    });
            });

            MatrixWindow.Draw(form, particle.Matrix);
        }
    }
}
