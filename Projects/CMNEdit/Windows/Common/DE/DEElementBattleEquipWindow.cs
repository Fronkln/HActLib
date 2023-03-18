using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib;

namespace CMNEdit
{
    internal static class DEElementAssetEquipWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementEquipAsset equip = node as DEElementEquipAsset;


            form.CreateHeader("Equip Asset");

            form.CreateInput("Asset ID", equip.AssetID.ToString(), delegate (string val) { equip.AssetID = uint.Parse(val); }, NumberBox.NumberMode.UInt);
            form.CreateComboBox("Equip Slot", (int)equip.EquipSlot, Enum.GetNames(typeof(AttachmentSlot)), delegate (int id) {equip.EquipSlot = (AttachmentSlot)id; });
            form.CreateInput("Equip Flags", equip.EquipFlags.ToString(), delegate (string val) { equip.EquipFlags = uint.Parse(val); }, NumberBox.NumberMode.UInt);
        }
    }
}
