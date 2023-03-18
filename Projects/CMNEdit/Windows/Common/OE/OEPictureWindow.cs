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
