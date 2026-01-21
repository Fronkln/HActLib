using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yarhl.FileFormat;
using Yarhl.IO;

namespace HActLib
{
    [ElementID(Game.Y5, 0x40)]
    [ElementID(Game.Y0, 0x3E)]
    [ElementID(Game.YK1, 0x3E)]
    [ElementID(Game.FOTNS, 0x3F)]
    public class OEPathAdjustment : NodeElement
    {
        public Matrix4x4 Adjustment;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Adjustment = (Matrix4x4)ConvertFormat.With<Matrix4x4Convert>(
                new ConversionInf(new BinaryFormat(reader.Stream), CMN.IsDE(version) ? EndiannessMode.LittleEndian : EndiannessMode.BigEndian));
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            Adjustment.Write(writer);
        }
    }
}
