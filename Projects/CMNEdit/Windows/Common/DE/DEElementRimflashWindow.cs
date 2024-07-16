using System;
using System.Drawing;
using System.Windows.Forms;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;

namespace CMNEdit.Windows.Common.DE
{
    internal static class DEElementRimflashWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementRimflash inf = node as DEElementRimflash;

            form.CreateSpace(25);
            form.CreateHeader("Rimflash");

            form.CreateInput("Rimflash Version", inf.RimflashVersion.ToString(), null, readOnly: true);
            form.CreateInput("Parameter Version", inf.ParamVersion.ToString(), null, readOnly: true);
            form.CreateInput("Fade Time", inf.FadeOutTime.ToString(), delegate (string val) { inf.FadeOutTime = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Root Value", inf.RootValue.ToString(), delegate (string val) { inf.RootValue = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

            if (inf.RimflashVersion < 5)
                return;

            form.CreateInput("Parameter ID", inf.ParamID.ToString(), delegate (string val) { inf.ParamID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateComboBox("Attachment Slot", (int)inf.AttachmentSlot, Enum.GetNames(typeof(AttachmentSlot)), delegate (int id) { inf.AttachmentSlot = (AttachmentSlot)id; });

            if (inf.RimflashParams != null)
            {
                if (inf.ParamVersion >= 3)
                {
                    RimflashParamsV3 v3params = inf.RimflashParams as RimflashParamsV3;

                    Panel color00Panel = null;
                    Panel color01Panel = null;

                    Panel color10Panel = null;
                    Panel color11Panel = null;

                    form.CreateHeader("Parameters Data");

                    form.CreateInput("Pattern ID", v3params.Pattern.ToString(), delegate (string val) { v3params.Pattern = int.Parse(val); }, NumberBox.NumberMode.Int);

                    form.CreateInput("Color 0 Intensity", v3params.Color0Intensity.ToString(), delegate (string val) { v3params.Color0Intensity = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Color 1 Intensity", v3params.Color1Intensity.ToString(), delegate (string val) { v3params.Color1Intensity = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    color00Panel = form.CreatePanel("Color 00", v3params.Color00.Clamp(),
                        delegate (Color col)
                        {
                            v3params.Color00 = col;
                            color00Panel.BackColor = ((RGB32)col).Clamp();
                        });
                    color01Panel = form.CreatePanel("Color 01", v3params.Color01,
                        delegate (Color col)
                        {
                            v3params.Color01 = col;
                            color01Panel.BackColor = ((RGB32)col).Clamp();
                        });

                    form.CreateInput("Color 01 Fresnel", v3params.Color01Fresnel.ToString(), delegate (string val) { v3params.Color01Fresnel = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    color10Panel = form.CreatePanel("Color 10", v3params.Color10.Clamp(),
                        delegate (Color col)
                        {
                            v3params.Color10 = col;
                            color10Panel.BackColor = ((RGB32)col).Clamp();
                        });
                    color11Panel = form.CreatePanel("Color 11", v3params.Color11.Clamp(),
                        delegate (Color col)
                        {
                            v3params.Color11 = col;
                            color11Panel.BackColor = ((RGB32)col).Clamp();
                        });

                    form.CreateInput("Color 1 Fresnel", v3params.Color1Fresnel.ToString(), delegate (string val) { v3params.Color1Fresnel = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateInput("Alpha", v3params.Alpha.ToString(), delegate (string val) { v3params.Alpha = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Color Intensity Power", v3params.ColorIntensityPower.ToString(), delegate (string val) { v3params.ColorIntensityPower = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Chroma Power", v3params.ChromaPower.ToString(), delegate (string val) { v3params.ChromaPower = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateInput("Noise Coordinate Size", v3params.NoiseCoordinateSize.ToString(), delegate (string val) { v3params.NoiseCoordinateSize = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Turbulence Coordinate Size", v3params.TurbulenceCoordinateSize.ToString(), delegate (string val) { v3params.TurbulenceCoordinateSize = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Turbulence Scale Base", v3params.TurbulenceScaleBase.ToString(), delegate (string val) { v3params.TurbulenceScaleBase = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Turbulence Scale Speed", v3params.TurbulanceScaleSpd.ToString(), delegate (string val) { v3params.TurbulanceScaleSpd = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Normal Lerp", v3params.NormalLerp.ToString(), delegate (string val) { v3params.NormalLerp = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateHeader("Flow");

                    form.CreateInput("Flow Gravity", v3params.flow_grav.ToString(), delegate (string val) { v3params.flow_grav = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateInput("Flow WX", v3params.flow_wx.ToString(), delegate (string val) { v3params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Flow WY", v3params.flow_wy.ToString(), delegate (string val) { v3params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Flow WZ", v3params.flow_wz.ToString(), delegate (string val) { v3params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateSpace(20);

                    form.CreateInput("Flow LX", v3params.flow_lx.ToString(), delegate (string val) { v3params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Flow LY", v3params.flow_ly.ToString(), delegate (string val) { v3params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Flow LZ", v3params.flow_lz.ToString(), delegate (string val) { v3params.flow_wx = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                    form.CreateSpace(20);

                    form.CreateInput("Flow Node Speed", v3params.flow_node_spd.ToString(), delegate (string val) { v3params.flow_node_spd = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    form.CreateInput("Flow Friction", v3params.flow_fric.ToString(), delegate (string val) { v3params.flow_fric = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                }
            }
        }
    }
}
