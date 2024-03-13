using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Yarhl.IO;
using Yarhl.FileFormat;

namespace HActLib
{
    public class BEP
    {
        public List<Node> Nodes = new List<Node>();

        public Node FindByGUID(Guid guid)
        {
            return Nodes.Where(x => x.BEPDat.Guid2 == guid).FirstOrDefault();
        }

        public NodeElement[] AllElements
        {
            get
            {
                List<NodeElement> elements = new List<NodeElement>();

                foreach (Node node in Nodes)
                    if (node.Category == AuthNodeCategory.Element)
                        elements.Add(node as NodeElement);

                return elements.ToArray();
            }
        }

        public static BEP Read(string path, Game game)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path), game);
        }

        public static BEP Read(byte[] buffer, Game game)
        {
            if (buffer == null || buffer.Length < 0)
                return null;

            CMN.LastFile = AuthFile.BEP;

            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader bepReader = new DataReader(readStream) { Endianness = EndiannessMode.LittleEndian, DefaultEncoding = System.Text.Encoding.GetEncoding(932) };

            if (!Internal.Reflection.Done)
                Internal.Reflection.Process();

            GameVersion version = CMN.GetVersionForGame(game);

            //Only used reading nodes etc, never used in writing
            CMN.LastGameVersion = version;
            CMN.LastHActDEGame = game;

            BEP bep = new BEP();

            if (bepReader.ReadString(4) != "_PEB")
                throw new Exception("Not a BEP file");

            bepReader.ReadBytes(12);



            while (true)
            {
                bool stop = true;

                if (bepReader.Stream.Position + 32 >= bepReader.Stream.Length)
                    break;

                bepReader.Stream.RunInPosition(delegate
                {
                    byte[] bytes = bepReader.ReadBytes(32);

                    foreach (byte b in bytes)
                        if (b != 0)
                            stop = false;
                }, 0, SeekMode.Current);

                if (stop)
                    break;

                bep.Nodes.Add((Node)ConvertFormat.With<DENodeConverter>(new NodeConvInf()
                {
                    format = new BinaryFormat(bepReader.Stream),
                    version = 18,
                    gameVersion = CMN.LastGameVersion,
                    file = CMN.LastFile
                }));
            }

            return bep;
        }

        public static void Write(BEP bep, string path, Game game)
        {
            CMN.LastHActDEGame = game;
            Write(bep, path, CMN.GetVersionForGame(game));
        }

        public static void Write(BEP bep, string path, GameVersion ver)
        {
            DataStream stream = WriteToStream(bep, ver);
            File.WriteAllBytes(path, stream.ToArray());
        }

        public static DataStream WriteToStream(BEP bep, GameVersion ver)
        {
            if (bep == null)
                return null;

            CMN.LastFile = AuthFile.BEP;
            CMN.LastGameVersion = ver;

            DataWriter writer = new DataWriter(new DataStream()) { Endianness = EndiannessMode.LittleEndian };
            writer.Write("_PEB", false, maxSize: 4);
            writer.Write(2);
            writer.Write(2);
            writer.Write(0);

            foreach (Node node in bep.Nodes)
                node.Write(writer, ver, 18);

            writer.WriteTimes(0, 80);

            return writer.Stream;
        }
    }
}
