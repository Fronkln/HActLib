using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OEPictureWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OENodePicture picture = node as OENodePicture;

            form.CreateHeader("Picture");

            form.CreateInput("Unknown", picture.Unknown.ToString(), delegate (string val) { picture.Unknown = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", picture.Unknown2.ToString(), delegate (string val) { picture.Unknown2 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", picture.Unknown3.ToString(), delegate (string val) { picture.Unknown3 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", picture.Unknown4.ToString(), delegate (string val) { picture.Unknown4 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", picture.Unknown5.ToString(), delegate (string val) { picture.Unknown5 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", picture.Unknown6.ToString(), delegate (string val) { picture.Unknown6 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Unknown", picture.Unknown7.ToString(), delegate (string val) { picture.Unknown7 = int.Parse(val); }, NumberBox.NumberMode.Int);
            
            form.CreateInput("Before Center X", picture.BeforeCenterX.ToString(), delegate (string val) { picture.BeforeCenterX = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Before Center Y", picture.BeforeCenterY.ToString(), delegate (string val) { picture.BeforeCenterY = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Before Size X", picture.BeforeSizeX.ToString(), delegate (string val) { picture.BeforeSizeX = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Before Size Y", picture.BeforeSizeY.ToString(), delegate (string val) { picture.BeforeSizeY = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Unknown", picture.Unknown8.ToString(), delegate (string val) { picture.Unknown8 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Unknown", picture.Unknown9.ToString(), delegate (string val) { picture.Unknown9 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("After Center X", picture.AfterCenterX.ToString(), delegate (string val) { picture.AfterCenterX = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("After Center Y", picture.AfterCenterY.ToString(), delegate (string val) { picture.AfterCenterY = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("After Size X", picture.AfterSizeX.ToString(), delegate (string val) { picture.AfterSizeX = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("After Size Y", picture.AfterSizeY.ToString(), delegate (string val) { picture.AfterSizeY = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Unknown", picture.Unknown10.ToString(), delegate (string val) { picture.Unknown10 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Unknown", picture.Unknown11.ToString(), delegate (string val) { picture.Unknown11 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            form.CreateInput("Picture", picture.PictureName, delegate (string val) { picture.PictureName = val; });

            form.CreateButton("Curve", delegate
            {
                CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                myNewForm.Visible = true;
                myNewForm.Init(picture.Animation,
                    delegate (byte[] outCurve)
                    {
                        picture.Animation = outCurve;
                    });
            });

        }
    }
}
