using System;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementAssetBreakUIDWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementAssetBreakUID breakAsset = node as DEElementAssetBreakUID;

            form.CreateHeader("Asset Break UID");

            form.CreateInput("Version", breakAsset.Ver.ToString(), delegate (string val) { breakAsset.Ver = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Is Blast", breakAsset.Blast.ToString(), delegate (string val) { breakAsset.Blast = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateInput("Is Replace", breakAsset.IsReplace.ToString(), delegate (string val) { breakAsset.IsReplace = int.Parse(val); }, NumberBox.NumberMode.Int);
            form.CreateInput("UID", breakAsset.UID.ToString(), delegate (string val) { breakAsset.UID = ulong.Parse(val); }, NumberBox.NumberMode.Long);
        }
    }
}
