using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y6, 1337)]
    [ElementID(Game.YK2, 1337)]
    [ElementID(Game.JE, 1337)]
    [ElementID(Game.YLAD, 1337)]
    [ElementID(Game.LJ, 1337)]
    [ElementID(Game.LAD7Gaiden, 1337)]
    [ElementID(Game.LADIW, 1337)]
    public class DECustomElementSystemSpeed : CustomDENodeAuthExtendedElement
    {
        public uint Type;
        public float Speed;

        public override float LastRevision => 0.1f;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            base.ReadElementData(reader, inf, version);

            Type = reader.ReadUInt32();
            Speed = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            base.WriteElementData(writer, version, hactVer);

            writer.Write(Type);
            writer.Write(Speed);
        }
    }
}
