using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class UserElementField
    {
        public string Name;
        public object Value;
        public int Length;
        public UserElementFieldType FieldType;
        public List<string> Args = new List<string>();

        public void Write(DataWriter writer)
        {
            switch(FieldType)
            {
                case UserElementFieldType.Byte:
                    writer.Write((byte)Value);
                    break;
                case UserElementFieldType.Short: 
                    writer.Write((short)Value);
                    break;
                case UserElementFieldType.Int32: 
                    writer.Write((int)Value);
                    break;
                case UserElementFieldType.Int64:
                    writer.Write((long)Value);
                    break;
                case UserElementFieldType.UShort:
                    writer.Write((ushort)Value);
                    break;
                case UserElementFieldType.UInt32:
                    writer.Write((uint)Value);
                    break;
                case UserElementFieldType.UInt64:
                    writer.Write((ulong)Value);
                    break;
                case UserElementFieldType.RGB32:
                    writer.Write((RGB32)Value);
                    break;
                case UserElementFieldType.BRG32:
                    RGB32 brg = (RGB32)Value;
                    writer.Write(brg.B);
                    writer.Write(brg.R);
                    writer.Write(brg.G);
                    break;
                case UserElementFieldType.RGBF32:
                    RGB rgbf = (RGB)Value;
                    writer.Write(rgbf.R);
                    writer.Write(rgbf.G);
                    writer.Write(rgbf.B);
                    break;
                case UserElementFieldType.BRGF32:
                    RGB brgf = (RGB)Value;
                    writer.Write(brgf.B);
                    writer.Write(brgf.R);
                    writer.Write(brgf.G);
                    break;
                case UserElementFieldType.RGBA32:
                    writer.Write((RGBA32)Value);
                    break;
                case UserElementFieldType.Float:
                    writer.Write((float)Value);
                    break;
                case UserElementFieldType.FAnimationCurve:
                    foreach (float f in (float[])Value)
                        writer.Write(f);
                    break;
                case UserElementFieldType.AnimationCurve:
                    foreach (byte b in (byte[])Value)
                        writer.Write(b);
                    break;
                case UserElementFieldType.ByteArray:
                    foreach (byte b in (byte[])Value)
                        writer.Write(b);
                    break;
                case UserElementFieldType.FixedString:
                    writer.Write(Value.ToString().ToLength(Length));
                    break;
            }
        }

        public void Read(DataReader reader)
        {
            switch(FieldType)
            {
                case UserElementFieldType.Byte:
                    Value = reader.Read<byte>(); 
                    break;
                case UserElementFieldType.Short:
                    Value = reader.Read<short>();
                    break;
                case UserElementFieldType.Int32:
                    Value = reader.Read<int>();
                    break;
                case UserElementFieldType.Int64:
                    Value = reader.Read<long>();
                    break;
                case UserElementFieldType.UShort:
                    Value = reader.Read<ushort>();
                    break;
                case UserElementFieldType.UInt32:
                    Value = reader.Read<uint>();
                    break;
                case UserElementFieldType.UInt64:
                    Value = reader.Read<ulong>();
                    break;
                case UserElementFieldType.RGB32:
                    Value = new RGB32(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32());
                    break;
                case UserElementFieldType.RGBA32:
                    Value = new RGBA32(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32());
                    break;
                case UserElementFieldType.BRG32:
                    uint bb = reader.ReadUInt32();
                    uint br = reader.ReadUInt32();
                    uint bg = reader.ReadUInt32();
                    Value = new RGB32(br, bg, bb);
                    break;
                case UserElementFieldType.RGBF32:
                    Value = new RGB(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    break;
                case UserElementFieldType.BRGF32:
                    float bbf = reader.ReadSingle();
                    float brf = reader.ReadSingle();
                    float bgf = reader.ReadSingle();
                    Value = new RGB(brf, bgf, bbf);
                    break;

                case UserElementFieldType.Float:
                    Value = reader.ReadSingle();
                    break;
                case UserElementFieldType.FAnimationCurve:
                    float[] fcurveval = new float[int.Parse(Args[0])];

                    for (int i = 0; i < fcurveval.Length; i++)
                        fcurveval[i] = reader.ReadSingle();

                    Value = fcurveval;
                    break;

                case UserElementFieldType.AnimationCurve:
                    byte[] curveval = new byte[int.Parse(Args[0])];

                    for (int i = 0; i < curveval.Length; i++)
                        curveval[i] = reader.ReadByte();
                    Value = curveval;
                    break;
                case UserElementFieldType.ByteArray:
                    byte[] barray = new byte[int.Parse(Args[0])];

                    for (int i = 0; i < barray.Length; i++)
                        barray[i] = reader.ReadByte();

                    Value = barray;
                    break;

                case UserElementFieldType.FixedString:
                    Length = int.Parse(Args[0]);

                    Value = reader.ReadString(Length);
                    break;
            }
        }
    }
}
