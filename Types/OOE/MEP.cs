using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace HActLib.OOE
{
    /// <summary>
    /// OOE MEP
    /// </summary>
    public class MEP
    {
        public List<EffectBase> Effects = new List<EffectBase>();

        public static MEP Read(string path)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path));
        }

        public static MEP Read(byte[] buffer)
        {
            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader mepReader = new DataReader(readStream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };

            mepReader.Stream.Seek(16, SeekMode.Start);

            MEP mep = new MEP();


            bool CheckMepOver()
            {
                if (mepReader.Stream.Position >= mepReader.Stream.Length)
                    return true;

                bool mepOver = false;

                mepReader.Stream.RunInPosition(
                    delegate
                    {
                        for (int i = 0; i < 3; i++)
                            if (mepReader.ReadInt32() == -1)
                            {
                                mepOver = true;
                                break;
                            }
                    }, 0, SeekMode.Current);


                return mepOver;
            }

            while(!CheckMepOver())
            {
                mep.Effects.Add(EffectBase.ReadFromMemory(mepReader));
            }


            return mep;
        }

        public static void Write(MEP mep, string path)
        {
            var binary = new BinaryFormat();
            var writer = new DataWriter(binary.Stream);
            writer.DefaultEncoding = Encoding.GetEncoding(932);

            writer.Endianness = EndiannessMode.BigEndian;

            writer.Write("_MEP", false, maxSize: 4);
            writer.Write(33619968);
            writer.Write(16777216);
            writer.Write(0);

            foreach (EffectBase effect in mep.Effects)
                effect.WriteToStream(writer);

            writer.Stream.WriteTo(path);
        }
    }
}
