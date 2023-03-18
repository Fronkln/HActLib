using System;
using Yarhl.IO;
using Yarhl.FileFormat;

namespace HActLib
{
    public class Matrix4x4Convert : IConverter<ConversionInf, Matrix4x4>
    {
        public Matrix4x4 Convert(ConversionInf conv)
        {
            DataReader reader = new DataReader(conv.format.Stream) { Endianness = conv.endianness, DefaultEncoding = System.Text.Encoding.GetEncoding(932) };
            return Matrix4x4.Read(reader);
        }

    }

    public class Matrix4x4
    {
        public Vector4 VM0 = Vector4.zero;
        public Vector4 VM1 = Vector4.zero;
        public Vector4 VM2 = Vector4.zero;
        public Vector4 VM3 = Vector4.zero;

        public static Matrix4x4 Default
        {
            get
            {
                return new Matrix4x4()
                {
                    VM0 = new Vector4(1, 0, 0, 0),
                    VM1 = new Vector4(0, 1, 0, 0),
                    VM2 = new Vector4(0, 0, 1, 0),
                    VM3 = new Vector4(0, 0, 0, 1),
                };
            }
        }

        public void Write(DataWriter writer)
        {
            VM0.Write(writer);
            VM1.Write(writer);
            VM2.Write(writer);
            VM3.Write(writer);
        }

        public static Matrix4x4 Read(DataReader reader)
        {
            Matrix4x4 mtx = new Matrix4x4();
            mtx.VM0 = Vector4.Read(reader);
            mtx.VM1 = Vector4.Read(reader);
            mtx.VM2 = Vector4.Read(reader);
            mtx.VM3 = Vector4.Read(reader);

            return mtx;
        }
    }
}
