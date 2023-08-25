using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.YLAD, 60010)]
    [ElementID(Game.LJ, 60010)]
    public class DECustomY7BTransitEXFollowup : CustomDENodeAuthExtendedElement
    {
        public string YHCName;
        public string YHCAttackName;
        public uint JobRestriction;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            base.ReadElementData(reader, inf, version);

            YHCName = reader.ReadString(32);
            YHCAttackName = reader.ReadString(32);
            JobRestriction = reader.ReadUInt32();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            base.WriteElementData(writer, version);

            writer.Write(YHCName.ToLength(32), true);
            writer.Write(YHCAttackName.ToLength(32), true);
            writer.Write(JobRestriction);
        }
    }
}
