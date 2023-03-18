using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 11)]
    [ElementID(Game.Ishin, 11)]
    [ElementID(Game.Y0, 11)]
    public class OENodePicture : NodeElement
    {
        private byte[] Unk;
        public byte[] Animation;
        public string PictureName;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Unk = reader.ReadBytes(104);
            Animation = reader.ReadBytes(32);
            PictureName = reader.ReadString(32).Split(new[] { '\0' }, 2)[0]; ;
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Unk);
            writer.Write(Animation);
            writer.Write(PictureName.ToLength(32), false, maxSize: 32);
        }
    }
}
