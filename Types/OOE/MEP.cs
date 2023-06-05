using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.IO;
using HActLib.OOE;
using System.Security;
using System.Reflection.PortableExecutable;
using System.Formats.Asn1;
using HActLib.Internal;

namespace HActLib
{
    public enum MEPVersion
    {
        Y3,
        Y5,
        Y0
    }

    /// <summary>
    /// MEP
    /// </summary>
    public class MEP
    {
        public MEPVersion Version;
        public List<MepEffect> Effects = new List<MepEffect>();

        public static MEP Read(string path)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path));
        }

        public static MEP Read(byte[] buffer)
        {
            Reflection.Process();

            MEP mep = new MEP();

            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader mepReader = new DataReader(readStream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };

            mepReader.ReadBytes(4); //Magic
            ushort val1 = mepReader.ReadUInt16();
            mepReader.Stream.Position += 2;

            if(val1 == 513)
            {
                mep.Version = MEPVersion.Y3;
                mepReader.Stream.Position += 8;
            }
            else
            {
                int val = mepReader.ReadInt32();
                mep.Version = (val == 0 ? MEPVersion.Y5 : MEPVersion.Y0);
                mepReader.Stream.Position += 4;
            }

            bool CheckMepOver()
            {
                if (mepReader.Stream.Position >= mepReader.Stream.Length)
                    return true;


                bool mepOver = true;

                mepReader.Stream.RunInPosition(
                    delegate
                    {
                        for(int i = 0; i < 3; i++)
                        {
                            int check = mepReader.ReadInt32();

                            if ((mep.Version < MEPVersion.Y5 && check != -1) || mep.Version >= MEPVersion.Y5 && check != 0)
                            {
                                mepOver = false;
                                break;
                            }
                        }

                    }, 0, SeekMode.Current);

                return mepOver;
            }

            while(!CheckMepOver())
            {
                MepEffect effect = null;
                
                switch(mep.Version)
                {
                    default:
                        throw new NotImplementedException();
                    case MEPVersion.Y3:
                        effect = new MepEffectY3();
                        break;
                    case MEPVersion.Y5:
                        effect = new MepEffectOE();
                        break;
                    case MEPVersion.Y0:
                        effect = new MepEffectOE();
                        break;
                }

                effect.Read(mepReader, mep.Version);
                mep.Effects.Add(effect);
            }


            return mep;
        }

        public static void Write(MEP mep, string path)
        {
            var binary = new BinaryFormat();
            var writer = new DataWriter(binary.Stream);
            writer.DefaultEncoding = Encoding.GetEncoding(932);

            CMN.LastHActDEGame = (mep.Version == MEPVersion.Y5 ? Game.Y5 : Game.Y0);

            writer.Endianness = EndiannessMode.BigEndian;

            if (mep.Version == MEPVersion.Y3)
            {
                writer.Write("_MEP", false, maxSize: 4);
                writer.Write(33619968);
                writer.Write(16777216);
                writer.Write(0);
            }
            else
            {
                writer.Write("MEP_", false, maxSize: 4);
                writer.Write(33554432);
                writer.Write(mep.Version == MEPVersion.Y5 ? 0 : 1);
                writer.Write(0);
            }

            foreach (MepEffect effect in mep.Effects)
                effect.Write(writer, mep.Version);

            if(mep.Version == MEPVersion.Y3)
            {
                writer.WriteTimes(255, 16);
                writer.WriteTimes(0, 16);
            }
            else
            {
                writer.WriteTimes(0, 64);
            }

            File.WriteAllBytes(path, writer.Stream.ToArray());
        }
    }
}
