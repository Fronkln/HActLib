using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMNEdit.Windows;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementUITextureWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            if (Form1.curVer < GameVersion.DE2)
                return;

            DEElementUITexture uiTex = node as DEElementUITexture;

            form.CreateHeader("UI Texture");
            form.CreateInput("Flags", uiTex.UITexFlags.ToString(), delegate (string val) { uiTex.UITexFlags = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("In Frame", uiTex.InFrame.ToString(), delegate (string val) { uiTex.InFrame = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Out Frame", uiTex.OutFrame.ToString(), delegate (string val) { uiTex.OutFrame = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Texture", uiTex.TextureName, delegate (string val) { uiTex.TextureName = val; });
            form.CreateInput("Texture ID", uiTex.TextureID.ToString(), delegate (string val) { uiTex.TextureID = uint.Parse(val); }, NumberBox.NumberMode.UInt);

            form.CreateButton("Curve", delegate
            {
                CurveView curveForm = new CurveView();
                curveForm.Visible = true;
                curveForm.Init(uiTex.Animation,
                    delegate (float[] outCurve)
                    {
                        uiTex.Animation = outCurve;
                    });
            });
        }
    }
}
