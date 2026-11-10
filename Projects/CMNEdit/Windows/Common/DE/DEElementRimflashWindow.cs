using System;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;
using static System.Windows.Forms.LinkLabel;

namespace CMNEdit.Windows.Common.DE
{
    internal static class DEElementRimflashWindow
    {
        public static byte[] CopiedCommand = null;
        public static RimflashParams CopiedParam;

        public static void Draw(Form1 form, Node node)
        {
            DEElementRimflash inf = node as DEElementRimflash;

            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("Rimflash");


            if (inf.ParamID == 0 && inf.RimflashParams != null)
            {
                PropertyEditorBuilder.CreateButton("Import Param", delegate
                {
                    OpenFileDialog wind = new OpenFileDialog();

                    if (wind.ShowDialog() != DialogResult.OK)
                        return;

                    inf.ImportParams(wind.FileName);
                });

                PropertyEditorBuilder.CreateButton("Export Param", delegate
                {
                    SaveFileDialog wind = new SaveFileDialog();

                    if (wind.ShowDialog() != DialogResult.OK)
                        return;

                    inf.ExportParams(wind.FileName);
                });

                PropertyEditorBuilder.CreateButton("Copy Param", delegate
                {
                    CopiedParam = inf.RimflashParams;
                });

            }

            PropertyEditorBuilder.CreateButton("Paste Param", delegate
            {
                if (CopiedParam != null)
                    inf.RimflashParams = CopiedParam;
                if (inf.ParamID != 0)
                    inf.ParamID = 0;
            });

            PropertyEditorBuilder.CreateButton("Import DBRef String", delegate
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("DB Ref string from effect_rim_flash_param.bin",
                "DB Ref Import",
                "",
                0,
                0);

                if (string.IsNullOrEmpty(input))
                    return;

                input = input.Trim();

                byte[] dat = DBBinaryString.Decode(input);

                inf.ImportParams(dat);
            });


            if(inf.RimflashParams != null)
            {
                PropertyEditorBuilder.CreateButton("Export DBRef String", delegate
                {
                    string output = DBBinaryString.Encode(inf.ExportParams());
                    System.Windows.Forms.Clipboard.SetText(output);

                    MessageBox.Show("Copied DBRef string to clipboard.");
                });
            }

            PropertyEditorBuilder.CreateInput("Rimflash Version", inf.RimflashVersion.ToString(), null, readOnly: true);
            PropertyEditorBuilder.CreateInput("Parameter Version", inf.ParamVersion.ToString(), null, readOnly: true);
            PropertyEditorBuilder.CreateInput("Fade Time", inf.FadeOutTime.ToString(), delegate (string val) { inf.FadeOutTime = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            PropertyEditorBuilder.CreateInput("Root Value", inf.RootValue.ToString(), delegate (string val) { inf.RootValue = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            
            if(inf.CommandData != null)
            {
                PropertyEditorBuilder.CreateButton("Copy Command", delegate
                {
                    CopiedCommand = inf.CommandData;
                });

                PropertyEditorBuilder.CreateButton("Clear Command", delegate
                {
                    inf.CommandData = null;
                });
            }

            if(CopiedCommand != null)
            {
                PropertyEditorBuilder.CreateButton("Paste Command", delegate
                {
                    inf.CommandData = CopiedCommand;
                });
            }


            if (inf.Curve != null)
            {
                PropertyEditorBuilder.CreateButton("Curve", delegate
                {
                    CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                    myNewForm.Visible = true;
                    myNewForm.Init(inf.Curve.ToArray(),
                        delegate (float[] outCurve)
                        {
                            inf.Curve = outCurve.ToList();
                        });
                });

                PropertyEditorBuilder.CreateButton("New Curve (8 values)", delegate
                {
                    float[] newCurve = new float[8];

                    for (int i = 0; i < 8; i++)
                        newCurve[i] = 1f;

                    inf.Curve = newCurve.ToList();
                });

                PropertyEditorBuilder.CreateButton("New Curve (16 values)", delegate
                {
                    float[] newCurve = new float[16];

                    for (int i = 0; i < 16; i++)
                        newCurve[i] = 1f;

                    inf.Curve = newCurve.ToList();
                });
            }

            PropertyEditorBuilder.CreateInput("Parameter ID", inf.ParamID.ToString(), delegate (string val) { inf.ParamID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            PropertyEditorBuilder.CreateComboBox("Attachment Slot", (int)inf.AttachmentSlot, Enum.GetNames(typeof(AttachmentSlot)), delegate (int id) { inf.AttachmentSlot = (AttachmentSlot)id; });

            if (inf.RimflashParams != null)
            {

                RimflashParams v1params = inf.RimflashParams as RimflashParams;
                RimflashParamsV2 v2params = inf.RimflashParams as RimflashParamsV2;

                Panel color00Panel = null;
                Panel color01Panel = null;

                Panel color10Panel = null;
                Panel color11Panel = null;

                PropertyEditorBuilder.CreateHeader("Parameters Data");

                if(inf.ParamVersion >= 2)             
                    PropertyEditorBuilder.CreateInput("Pattern ID", v2params.Pattern.ToString(), delegate (string val) { v2params.Pattern = int.Parse(val); }, NumberBox.NumberMode.Int);

                PropertyEditorBuilder.CreateInput("Color 0 Intensity", v1params.Color0Intensity.ToString(), delegate (string val) { v1params.Color0Intensity = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Color 1 Intensity", v1params.Color1Intensity.ToString(), delegate (string val) { v1params.Color1Intensity = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                color00Panel = PropertyEditorBuilder.CreatePanel("Color 00", v1params.Color00.Clamp(),
                    delegate (Color col)
                    {
                        v1params.Color00 = col;
                        color00Panel.BackColor = ((RGB32)col).Clamp();
                    });
                color01Panel = PropertyEditorBuilder.CreatePanel("Color 01", v1params.Color01,
                    delegate (Color col)
                    {
                        v1params.Color01 = col;
                        color01Panel.BackColor = ((RGB32)col).Clamp();
                    });

                PropertyEditorBuilder.CreateInput("Color 01 Fresnel", v1params.Color01Fresnel.ToString(), delegate (string val) { v1params.Color01Fresnel = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                color10Panel = PropertyEditorBuilder.CreatePanel("Color 10", v1params.Color10.Clamp(),
                    delegate (Color col)
                    {
                        v1params.Color10 = col;
                        color10Panel.BackColor = ((RGB32)col).Clamp();
                    });
                color11Panel = PropertyEditorBuilder.CreatePanel("Color 11", v1params.Color11.Clamp(),
                    delegate (Color col)
                    {
                        v1params.Color11 = col;
                        color11Panel.BackColor = ((RGB32)col).Clamp();
                    });

                PropertyEditorBuilder.CreateInput("Color 1 Fresnel", v1params.Color1Fresnel.ToString(), delegate (string val) { v1params.Color1Fresnel = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                PropertyEditorBuilder.CreateInput("Alpha", v1params.Alpha.ToString(), delegate (string val) { v1params.Alpha = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                if (inf.ParamVersion >= 3)
                {
                    RimflashParamsV3 v3params = v2params as RimflashParamsV3;
                    PropertyEditorBuilder.CreateInput("Color Intensity Power", v3params.ColorIntensityPower.ToString(), delegate (string val) { v3params.ColorIntensityPower = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    PropertyEditorBuilder.CreateInput("Chroma Power", v3params.ChromaPower.ToString(), delegate (string val) { v3params.ChromaPower = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                }

                PropertyEditorBuilder.CreateInput("Noise Coordinate Size", v1params.NoiseCoordinateSize.ToString(), delegate (string val) { v1params.NoiseCoordinateSize = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Turbulence Coordinate Size", v1params.TurbulenceCoordinateSize.ToString(), delegate (string val) { v1params.TurbulenceCoordinateSize = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Turbulence Scale Base", v1params.TurbulenceScaleBase.ToString(), delegate (string val) { v1params.TurbulenceScaleBase = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Turbulence Scale Speed", v1params.TurbulanceScaleSpd.ToString(), delegate (string val) { v1params.TurbulanceScaleSpd = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Normal Lerp", v1params.NormalLerp.ToString(), delegate (string val) { v1params.NormalLerp = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                PropertyEditorBuilder.CreateHeader("Flow");

                PropertyEditorBuilder.CreateInput("Flow Gravity", v1params.flow_grav.ToString(), delegate (string val) { v1params.flow_grav = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                PropertyEditorBuilder.CreateInput("Flow WX", v1params.flow_wx.ToString(), delegate (string val) { v1params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Flow WY", v1params.flow_wy.ToString(), delegate (string val) { v1params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Flow WZ", v1params.flow_wz.ToString(), delegate (string val) { v1params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                PropertyEditorBuilder.CreateSpace(20);

                PropertyEditorBuilder.CreateInput("Flow LX", v1params.flow_lx.ToString(), delegate (string val) { v1params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Flow LY", v1params.flow_ly.ToString(), delegate (string val) { v1params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Flow LZ", v1params.flow_lz.ToString(), delegate (string val) { v1params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                PropertyEditorBuilder.CreateSpace(20);

                PropertyEditorBuilder.CreateInput("Flow Node Speed", v1params.flow_node_spd.ToString(), delegate (string val) { v1params.flow_node_spd = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Flow Friction", v1params.flow_fric.ToString(), delegate (string val) { v1params.flow_fric = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            }
        }
    }
}
