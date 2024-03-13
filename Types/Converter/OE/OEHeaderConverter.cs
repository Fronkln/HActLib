using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.FileFormat;


namespace HActLib
{
    public class OEHeaderConverter : IConverter<BinaryFormat, OECMN.Header>
    {
        public OECMN.Header Convert(BinaryFormat format)
        {
            DataReader reader = new DataReader(format.Stream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };
            OECMN.Header header = new OECMN.Header();

            header.Version = reader.ReadUInt32();
            header.Flags = reader.ReadUInt32();
            header.Start = reader.ReadSingle();
            header.End = reader.ReadSingle();

            header.NodeDrawNum = reader.ReadInt32();

            header.CutInfoPointer = reader.ReadUInt32();
            header.DisableFrameInfoPointer = reader.ReadUInt32();
            header.ResourceCutInfoPointer = reader.ReadUInt32();

            if(header.Version != 10)
                header.SoundInfoPointer = reader.ReadUInt32();

            header.NodeInfoPointer = reader.ReadUInt32();

            header.ChainCameraIn = reader.ReadSingle();
            header.ChainCameraOut = reader.ReadSingle();

            return header;
        }
    }
}
