using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class OEFaceExpressionWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            OEFaceExpression exp = node as OEFaceExpression;

            form.CreateHeader("Face Expression");

            form.CreateInput("Unknown", exp.Unk1.ToString(), delegate (string val) { exp.Unk1 = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Expression ID (IFA)", exp.ExpressionID.ToString(), delegate (string val) { exp.ExpressionID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Unknown", exp.Unk2.ToString(), delegate (string val) { exp.Unk2  = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
            form.CreateInput("Unknown", exp.Unk3.ToString(), delegate (string val) { exp.Unk3 = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

        }
    }
}
