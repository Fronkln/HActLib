using System;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using HActLib;

namespace CMNEdit
{
    public class DEElementBaseSpotlightWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementBaseLight light = node as DEElementBaseLight;

            form.CreateInput("Position X", light.Position.x.ToString(CultureInfo.InvariantCulture), delegate (string val) { light.Position.x = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Position Y", light.Position.y.ToString(CultureInfo.InvariantCulture), delegate (string val) { light.Position.y = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Position Z", light.Position.z.ToString(CultureInfo.InvariantCulture), delegate (string val) { light.Position.z = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Kelvin", light.Kelvin.ToString(), delegate (string val) { light.Kelvin = int.Parse(val); }, NumberBox.NumberMode.Int);

            Panel colPanel = null;
            colPanel = form.CreatePanel("Color", light.Color,
                delegate (Color col)
                {
                    light.Color = col;
                    colPanel.BackColor = col;
                });
        }
    }
}
