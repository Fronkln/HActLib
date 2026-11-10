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
                    PropertyEditorBuilder.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { }, NumberBox.NumberMode.Text, true);
                    break;
                case UserElementFieldType.ByteArray:
                    break;
                case UserElementFieldType.FixedString:
                    PropertyEditorBuilder.CreateInput(fieldName, field.Value.ToString(), delegate(string val) { field.Value = val; });
                    break;
                case UserElementFieldType.Byte:
                    PropertyEditorBuilder.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = byte.Parse(val); }, NumberBox.NumberMode.Byte);
                    break;
                case UserElementFieldType.Short:
                    PropertyEditorBuilder.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = short.Parse(val); }, NumberBox.NumberMode.Ushort);
                    break;
                case UserElementFieldType.Int32:
                    PropertyEditorBuilder.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = int.Parse(val); }, NumberBox.NumberMode.Int);
                    break;
                case UserElementFieldType.Int64:
                    PropertyEditorBuilder.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = long.Parse(val); }, NumberBox.NumberMode.Long);
                    break;
                case UserElementFieldType.UShort:
                    PropertyEditorBuilder.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = ushort.Parse(val); }, NumberBox.NumberMode.Ushort);
                    break;
                case UserElementFieldType.UInt32:
                    PropertyEditorBuilder.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = uint.Parse(val); }, NumberBox.NumberMode.UInt);
                    break;
                case UserElementFieldType.UInt64:
                    PropertyEditorBuilder.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = ulong.Parse(val); }, NumberBox.NumberMode.ULong);
                    break;
                case UserElementFieldType.RGB32:
                    Panel rgb32Panel = null;

                    rgb32Panel = PropertyEditorBuilder.CreatePanel(fieldName, (RGB32)field.Value,
                        delegate (Color col)
                        {
                            field.Value = (RGB32)col;
                            rgb32Panel.BackColor = col;
                        });
                    break;

                case UserElementFieldType.BRG32:
                    Panel brg32Panel = null;

                    brg32Panel = PropertyEditorBuilder.CreatePanel(fieldName, (RGB32)field.Value,
                        delegate (Color col)
                        {
                            field.Value = (RGB32)col;
                            brg32Panel.BackColor = col;
                        });
                    break;

                case UserElementFieldType.RGBF32:
                    Panel rgbf32Panel = null;

                    rgbf32Panel = PropertyEditorBuilder.CreatePanelFI2(fieldName, ((RGB)field.Value).Clamp(), (RGB)field.Value,
                        delegate (RGB col)
                        {
                            field.Value = (RGB)col;
                            rgbf32Panel.BackColor = col.Clamp();
                        });
                    break;

                case UserElementFieldType.BRGF32:
                    Panel brgf32Panel = null;

                    brg32Panel = PropertyEditorBuilder.CreatePanel(fieldName, (RGB)field.Value,
                        delegate (Color col)
                        {
                            field.Value = (RGB)col;
                            brgf32Panel.BackColor = col;
                        });
                    break;

                case UserElementFieldType.RGBA32:
                    Panel rgba32Panel = null;

                    rgba32Panel = PropertyEditorBuilder.CreatePanel(fieldName, (RGBA32)field.Value,
                        delegate (Color col)
                        {
                            field.Value = (RGBA32)col;
                            rgba32Panel.BackColor = col;
                        });
                    break;

                case UserElementFieldType.Float:
                    PropertyEditorBuilder.CreateInput(fieldName, field.Value.ToString(), delegate (string val) { field.Value = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                    break;

                case UserElementFieldType.FAnimationCurve:
                    PropertyEditorBuilder.CreateButton(fieldName, delegate
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

                case UserElementFieldType.AnimationCurve:
                    PropertyEditorBuilder.CreateButton(fieldName, delegate
                    {
                        CurveView myNewForm = new CurveView();
                        myNewForm.Visible = true;
                        myNewForm.Init((byte[])field.Value,
                            delegate (byte[] outCurve)
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

            PropertyEditorBuilder.CreateHeader(nodeUser.UserData.NodeName.Replace("_", " "));

            for(int i = 0; i < nodeUser.Fields.Count; i++)
                DrawField(form, nodeUser, i);
        }
    }
}
