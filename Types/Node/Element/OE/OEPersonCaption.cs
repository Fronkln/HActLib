using System;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.YK1, 81)]
    [ElementID(Game.Y0, 81)]
    [ElementID(Game.Y5, 83)]
    [ElementID(Game.Ishin, 83)]
    public class OEPersonCaption : NodeElement
    {
        public string Texture;

        public float PosX;
        public float PosY;

        public float SizeX;
        public float SizeY;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Texture = reader.ReadString(32);

            PosX = reader.ReadSingle();
            PosY = reader.ReadSingle();

            SizeX = reader.ReadSingle();
            SizeY = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(Texture.ToLength(32));

            writer.Write(PosX);
            writer.Write(PosY);

            writer.Write(SizeX);
            writer.Write(SizeY);
        }
    }
}
