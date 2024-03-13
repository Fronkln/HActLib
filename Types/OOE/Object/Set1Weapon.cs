using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class ObjectWeapon : ObjectBase
    {
        public string Name;
        public string WeaponType;
        public string HoldingArm;

        public override string GetName()
        {
            return Name;
        }

        internal override void ProcessNodeData(DataReader reader)
        {
            base.ProcessNodeData(reader);

            Name = StringTable[0];
            WeaponType = StringTable[1];
            HoldingArm = StringTable[2];
        }
    }
}
