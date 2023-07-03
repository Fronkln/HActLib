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
        public string Type;

        public int Unknown1;
        public byte[] UnknownData1 = new byte[24]; //Not padding

        public string[] Resources = new string[4];
        public string UnknownResource; //not part of the 4 resources array
        
        public byte[] UnreadData;

        internal virtual void ReadData(DataReader reader)
        {

        }

        internal virtual void WriteData(DataWriter writer)
        {
            writer.Write(0xDEADC0DE);
            writer.Write(Unknown1);
            writer.Write(UnknownData1);

            //Resource pointers will be written later.
            for (int i = 0; i < Resources.Length; i++)
                writer.Write(0xDEADC0DE);

            writer.Write(0xDEADC0DE);
        }
    }
}
