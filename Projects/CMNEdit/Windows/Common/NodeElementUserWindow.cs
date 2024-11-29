using CMNEdit.Windows;
using HActLib;
using ParLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit
{
    internal static class NodeElementUserWindow
    {
        private static void DrawField(Form1 form, NodeElementUser element, int idx)
        {
            var field = element.Fields[idx];

            string fieldName = field.Name.ToString().ToTitleCase().Replace("_", " ");


            switch (field.FieldType)
            {
                default:
                    form.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { }, NumberBox.NumberMode.Text, true);
                    break;
                case UserElementFieldType.Byte:
                    form.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = byte.Parse(val); }, NumberBox.NumberMode.Byte);
                    break;
                case UserElementFieldType.Short:
                    form.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = short.Parse(val); }, NumberBox.NumberMode.Ushort);
                    break;
                case UserElementFieldType.Int32:
                    form.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;
                case UserElementFieldType.Int64:
                    form.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = long.Parse(val); }, NumberBox.NumberMode.Long);
                    break;
                case UserElementFieldType.UShort:
                    form.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    break;
                case UserElementFieldType.UInt32:
                    form.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = uint.Parse(val); }, NumberBox.NumberMode.UInt);
                    break;
                case UserElementFieldType.UInt64:
                    form.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = ulong.Parse(val); }, NumberBox.NumberMode.ULong);
                    break;
                case UserElementFieldType.RGB32:
                    Panel rgb32Panel = null;

                    rgb32Panel = form.CreatePanel(fieldName, (RGB32)field.Value,
                        delegate (Color col)
                        {
                            field.Value = (RGB32)col;
                            rgb32Panel.BackColor = col;
                        });
                    break;

                case UserElementFieldType.RGBA32:
                    Panel rgba32Panel = null;

                    rgba32Panel = form.CreatePanel(fieldName, (RGBA32)field.Value,
                        delegate (Color col)
                        {
                            field.Value = (RGBA32)col;
                            rgba32Panel.BackColor = col;
                        });
                    break;

                case UserElementFieldType.Float:
                    form.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    break;

                case UserElementFieldType.FAnimationCurve:
                    form.CreateButton(fieldName, delegate
                    {
                        CurveView myNewForm = new CurveView();
                        myNewForm.Visible = true;
                        myNewForm.Init((float[])field.Value,
                            delegate (float[] outCurve)
                            {
                               field.Value = outCurve;
                            });
                    });
                    break;

            }
        }

        public static void Draw(Form1 form, Node node)
        {
            var nodeUser = node as NodeElementUser;

            form.CreateHeader(nodeUser.UserData.NodeName.Replace("_", " "));

            for(int i = 0; i < nodeUser.Fields.Count; i++)
                DrawField(form, nodeUser, i);
        }
    }
}
