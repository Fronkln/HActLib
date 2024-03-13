using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.FileFormat;


namespace HActLib
{
    public class NodePathBase : Node
    {
        public Matrix4x4 Matrix = Matrix4x4.Default;

        public NodePathBase()
        {
            Category = AuthNodeCategory.Path;
        }

        internal override void ReadNodeData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            Matrix = (Matrix4x4)ConvertFormat.With<Matrix4x4Convert>(
                new ConversionInf(new BinaryFormat(reader.Stream), CMN.IsDE(version) ? EndiannessMode.LittleEndian : EndiannessMode.BigEndian));
        }

        internal override void WriteNodeData(DataWriter writer, GameVersion version, uint hactVer)
        {
            base.WriteNodeData(writer, version, hactVer);

            Matrix.Write(writer);
        }

        internal override int GetSize(GameVersion version, uint hactVer)
        {
            return 64;
        }
    }
}
