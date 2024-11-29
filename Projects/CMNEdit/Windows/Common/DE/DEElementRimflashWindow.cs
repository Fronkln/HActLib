using System;
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
        public static void Draw(Form1 form, Node node)
        {
            DEElementRimflash inf = node as DEElementRimflash;

            form.CreateSpace(25);
            form.CreateHeader("Rimflash");


            if (inf.ParamID == 0 && inf.RimflashParams != null)
            {
                form.CreateButton("Import Param", delegate
                {
                    OpenFileDialog wind = new OpenFileDialog();

                    if (wind.ShowDialog() != DialogResult.OK)
                        return;

                    inf.ImportParams(wind.FileName);
                });

                form.CreateButton("Export Param", delegate
                {
                    SaveFileDialog wind = new SaveFileDialog();

                    if (wind.ShowDialog() != DialogResult.OK)
                        return;

                    inf.ExportParams(wind.FileName);
                });
            }

            form.CreateInput("Rimflash Version", inf.RimflashVersion.ToString(), null, readOnly: true);
            form.CreateInput("Parameter Version", inf.ParamVersion.ToString(), null, readOnly: true);
            form.CreateInput("Fade Time", inf.FadeOutTime.ToString(), delegate (string val) { inf.FadeOutTime = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Root Value", inf.RootValue.ToString(), delegate (string val) { inf.RootValue = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            if(inf.Curve != null)
            {
                form.CreateButton("Curve", delegate
                {
                    CMNEdit.Windows.CurveView myNewForm = new CMNEdit.Windows.CurveView();
                    myNewForm.Visible = true;
                    myNewForm.Init(inf.Curve.ToArray(),
                        delegate (float[] outCurve)
                        {
                            inf.Curve = outCurve.ToList();
                        });
                });

                form.CreateButton("New Curve (8 values)", delegate
                {
                    float[] newCurve = new float[8];

                    for (int i = 0; i < 8; i++)
                        newCurve[i] = 1f;

                    inf.Curve = newCurve.ToList();
                });

                form.CreateButton("New Curve (16 values)", delegate
                {
                    float[] newCurve = new float[16];

                    for (int i = 0; i < 16; i++)
                        newCurve[i] = 1f;

                    inf.Curve = newCurve.ToList();
                });
            }

            form.CreateInput("Parameter ID", inf.ParamID.ToString(), delegate (string val) { inf.ParamID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateComboBox("Attachment Slot", (int)inf.AttachmentSlot, Enum.GetNames(typeof(AttachmentSlot)), delegate (int id) { inf.AttachmentSlot = (AttachmentSlot)id; });

            if (inf.RimflashParams != null)
            {
                if (inf.ParamVersion >= 2)
                {
                    RimflashParamsV2 v2params = inf.RimflashParams as RimflashParamsV2;

                    Panel color00Panel = null;
                    Panel color01Panel = null;

                    Panel color10Panel = null;
                    Panel color11Panel = null;

                    form.CreateHeader("Parameters Data");

                    form.CreateInput("Pattern ID", v2params.Pattern.ToString(), delegate (string val) { v2params.Pattern = int.Parse(val); }, NumberBox.NumberMode.Int);

                    form.CreateInput("Color 0 Intensity", v2params.Color0Intensity.ToString(), delegate (string val) { v2params.Color0Intensity = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Color 1 Intensity", v2params.Color1Intensity.ToString(), delegate (string val) { v2params.Color1Intensity = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    color00Panel = form.CreatePanel("Color 00", v2params.Color00.Clamp(),
                        delegate (Color col)
                        {
                            v2params.Color00 = col;
                            color00Panel.BackColor = ((RGB32)col).Clamp();
                        });
                    color01Panel = form.CreatePanel("Color 01", v2params.Color01,
                        delegate (Color col)
                        {
                            v2params.Color01 = col;
                            color01Panel.BackColor = ((RGB32)col).Clamp();
                        });

                    form.CreateInput("Color 01 Fresnel", v2params.Color01Fresnel.ToString(), delegate (string val) { v2params.Color01Fresnel = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    color10Panel = form.CreatePanel("Color 10", v2params.Color10.Clamp(),
                        delegate (Color col)
                        {
                            v2params.Color10 = col;
                            color10Panel.BackColor = ((RGB32)col).Clamp();
                        });
                    color11Panel = form.CreatePanel("Color 11", v2params.Color11.Clamp(),
                        delegate (Color col)
                        {
                            v2params.Color11 = col;
                            color11Panel.BackColor = ((RGB32)col).Clamp();
                        });

                    form.CreateInput("Color 1 Fresnel", v2params.Color1Fresnel.ToString(), delegate (string val) { v2params.Color1Fresnel = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateInput("Alpha", v2params.Alpha.ToString(), delegate (string val) { v2params.Alpha = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    if (inf.ParamVersion >= 3)
                    {
                        RimflashParamsV3 v3params = v2params as RimflashParamsV3;
                        form.CreateInput("Color Intensity Power", v3params.ColorIntensityPower.ToString(), delegate (string val) { v3params.ColorIntensityPower = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                        form.CreateInput("Chroma Power", v3params.ChromaPower.ToString(), delegate (string val) { v3params.ChromaPower = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    }

                    form.CreateInput("Noise Coordinate Size", v2params.NoiseCoordinateSize.ToString(), delegate (string val) { v2params.NoiseCoordinateSize = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Turbulence Coordinate Size", v2params.TurbulenceCoordinateSize.ToString(), delegate (string val) { v2params.TurbulenceCoordinateSize = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Turbulence Scale Base", v2params.TurbulenceScaleBase.ToString(), delegate (string val) { v2params.TurbulenceScaleBase = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Turbulence Scale Speed", v2params.TurbulanceScaleSpd.ToString(), delegate (string val) { v2params.TurbulanceScaleSpd = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Normal Lerp", v2params.NormalLerp.ToString(), delegate (string val) { v2params.NormalLerp = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateHeader("Flow");

                    form.CreateInput("Flow Gravity", v2params.flow_grav.ToString(), delegate (string val) { v2params.flow_grav = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateInput("Flow WX", v2params.flow_wx.ToString(), delegate (string val) { v2params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Flow WY", v2params.flow_wy.ToString(), delegate (string val) { v2params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Flow WZ", v2params.flow_wz.ToString(), delegate (string val) { v2params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateSpace(20);

                    form.CreateInput("Flow LX", v2params.flow_lx.ToString(), delegate (string val) { v2params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Flow LY", v2params.flow_ly.ToString(), delegate (string val) { v2params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Flow LZ", v2params.flow_lz.ToString(), delegate (string val) { v2params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateSpace(20);

                    form.CreateInput("Flow Node Speed", v2params.flow_node_spd.ToString(), delegate (string val) { v2params.flow_node_spd = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Flow Friction", v2params.flow_fric.ToString(), delegate (string val) { v2params.flow_fric = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                }
            }
        }
    }
}
