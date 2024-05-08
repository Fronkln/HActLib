using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementArmsReduceAssetCountWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementReduceAssetUse reduceUse = node as DEElementReduceAssetUse;

            form.CreateHeader("Reduce Use Count");
            form.CreateInput("Reduce Amount", reduceUse.ReduceCount.ToString(), delegate (string val) { reduceUse.ReduceCount = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("Flags", ((uint)reduceUse.Flag).ToString(), delegate (string val) { reduceUse.Flags = (DEElementReduceAssetUse.EFlag)uint.Parse(val); });
        }
    }
}
