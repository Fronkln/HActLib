using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.FileFormat;


namespace HActLib
{
    [ElementID(Game.Y6, 0x2C)]
    [ElementID(Game.YK2, 0x2C)]
    [ElementID(Game.JE, 0x2C)]
    [ElementID(Game.YLAD, 0x2A)]
    [ElementID(Game.LJ, 0x2A)]
    [ElementID(Game.LAD7Gaiden, 0x2A)]
    [ElementID(Game.LADIW, 0x2A)]
    public class DEElementPathOffset : NodeElement
    {
        public Matrix4x4 Matrix = Matrix4x4.Default;
        public bool IsPreview = false;
        public bool IsOverwrite = true;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Matrix = (Matrix4x4)ConvertFormat.With<Matrix4x4Convert>(
                new ConversionInf(new BinaryFormat(reader.Stream), CMN.IsDE(version) ? EndiannessMode.LittleEndian : EndiannessMode.BigEndian));

            IsPreview = reader.ReadInt32() == 1;
            IsOverwrite = reader.ReadInt32() == 1;

            reader.ReadBytes(8);
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            Matrix.Write(writer);

            writer.Write(Convert.ToInt32(IsPreview));
            writer.Write(Convert.ToInt32(IsOverwrite));

            writer.WriteTimes(0, 8);
        }
    }
}
