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
    }
}
