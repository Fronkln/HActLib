using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace HActLib
{
    public class CSV
    {
        public int RevisionDate;

        //You couldn't imagine my disappointment when i found out IDs are not unique.
        //Two instances of 3220 exist in Y3 hact_csv, which means i can't make this a dictionary.
        public List<CSVHAct> Entries = new List<CSVHAct>();

        public CSVHAct TryGetEntry(uint hactID)
        {
            return Entries.FirstOrDefault(x => x.ID == hactID);
        }
        public CSVHAct[] TryGetEntries(uint hactID)
        {
            return Entries.Where(x => x.ID == hactID).ToArray();
        }


        public static CSV Read(string path)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path));
        }

        public static void Write(CSV csv, string path)
        {
            if (csv == null)
                return;

            BinaryFormat output = (BinaryFormat)ConvertFormat.With<CSVWriter>(csv);
            File.WriteAllBytes(path, output.Stream.ToArray());
        }

        public static CSV Read(byte[] buffer)
        {
            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader csvReader = new DataReader(readStream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };

            CSV tev = (CSV)ConvertFormat.With<CSVReader>(new BinaryFormat(csvReader.Stream));

            return tev;
        }
    }
}
