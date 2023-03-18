using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DENodeModelWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            NodeModel mdl = node as NodeModel;

            form.CreateHeader("Bone");
            form.CreateInput("Bone Name", mdl.BoneName.Text, delegate (string val) { mdl.BoneName.Set(val); });
        }
    }
}
