using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    //HE_ prefix = HAct Event?
    public class CSVHActEvent
    {
        public string Name;

        public CSVHActEventType Type;

        public int HEUnknown2 = 0;
        public int HEUnknown3 = 0;
        public int HEUnknown4 = 0;
        public int HEUnknown5 = 0;
        public int HEUnknown6 = 0;
        public int HEUnknown7 = 0;

        public string[] Resources = new string[4];
        public string UnknownResource = ""; //not part of the 4 resources array
        
        public byte[] UnreadData;

        internal virtual void ReadData(DataReader reader)
        {

        }

        internal virtual void WriteData(DataWriter writer)
        {
            writer.Write(0xDEADC0DE);
            writer.Write((uint)Type);

            writer.Write(HEUnknown2);
            writer.Write(HEUnknown3);
            writer.Write(HEUnknown4);
            writer.Write(HEUnknown5);
            writer.Write(HEUnknown6);
            writer.Write(HEUnknown7);

            //Resource pointers will be written later.
            for (int i = 0; i < Resources.Length; i++)
                writer.Write(0xDEADC0DE);

            writer.Write(0xDEADC0DE);
        }
    }
}
