using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementCharacterSpeedWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementCharacterSpeed speed = node as DEElementCharacterSpeed;

            form.CreateHeader("Character Speed");


           
            form.CreateInput("Minimum Speed", speed.MinSpeed.ToString(), delegate (string val) { speed.MinSpeed = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Maximum Speed", speed.MaxSpeed.ToString(), delegate (string val) { speed.MaxSpeed = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);         

            form.CreateButton("Curve", delegate
            {
                CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(speed.Animation,
                    delegate (byte[] outCurve)
                    {
                        speed.Animation = outCurve;
                    });
            });
        }
    }
}
