using System;
using CMNEdit;
using CMNEdit.Windows.Common.DE;
using HActLib;

namespace CMNEdit.Windows.Common.DE
{
    internal static class DEElementHideAssetWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementHideAsset inf = node as DEElementHideAsset;

            form.CreateSpace(25);
            form.CreateHeader("Hide Asset");

            form.CreateComboBox("Asset Slot", (int)inf.ID, Enum.GetNames(typeof(AttachmentSlot)), delegate (int id) { inf.ID = (AttachmentSlot)id; });
        }
    }
}
