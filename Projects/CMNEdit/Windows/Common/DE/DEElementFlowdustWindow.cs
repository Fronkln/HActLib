using System;
using System.Drawing;
using System.Windows.Forms;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementFlowdustWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementFlowdust inf = node as DEElementFlowdust;

            PropertyEditorBuilder.CreateSpace(25);
            PropertyEditorBuilder.CreateHeader("Flowdust");

            PropertyEditorBuilder.CreateInput("Flowdust Version", inf.FlowVersion.ToString(), null, readOnly: true);
            PropertyEditorBuilder.CreateInput("Parameter Version", inf.SetParamVersion.ToString(), null, readOnly: true);

            if (!inf.ParameterFlowdust)
            {
                PropertyEditorBuilder.CreateInput("Radius", inf.Radius.ToString(), delegate (string val) { inf.Radius = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Amount", inf.Amount.ToString(), delegate (string val) { inf.Amount = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                Panel color00Panel = null;
                Panel color01Panel = null;
                color00Panel = PropertyEditorBuilder.CreatePanelFI("Color 0", inf.Col0,
                    delegate (RGB col)
                    {
                        inf.Col0 = col;
                        color00Panel.BackColor = col.Clamp();
                    });

                PropertyEditorBuilder.CreateInput("Color 0 Intensity", inf.Col0I.ToString(), delegate (string val) { inf.Col0I = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                color01Panel = PropertyEditorBuilder.CreatePanelFI("Color 1", inf.Col1,
                    delegate (RGB col)
                    {
                        inf.Col1 = col;
                        color01Panel.BackColor = col.Clamp();
                    });

                PropertyEditorBuilder.CreateInput("Color 1 Intensity", inf.Col1I.ToString(), delegate (string val) { inf.Col1I = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                PropertyEditorBuilder.CreateInput("Lifetime", inf.Lifetime.ToString(), delegate (string val) { inf.Lifetime = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Lifetime Range", inf.LifetimeRange.ToString(), delegate (string val) { inf.LifetimeRange = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                PropertyEditorBuilder.CreateInput("Radius", inf.Radius2.ToString(), delegate (string val) { inf.Radius2 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Radius Range", inf.RadiusRange.ToString(), delegate (string val) { inf.RadiusRange = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            }
        }
    }
}
