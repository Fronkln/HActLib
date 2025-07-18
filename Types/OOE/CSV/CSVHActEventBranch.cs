using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class CSVHActEventBranch : CSVHActEvent
    {
        public byte[] Nothing = new byte[28];

        public CSVHActEventBranch()
        {
            Type = CSVHActEventType.Branch;
            Name = "HE_BRANCH_00";
        }

        internal override void ReadData(DataReader reader)
        {
            Nothing = reader.ReadBytes(28);
        }

        internal override void WriteData(DataWriter writer)
        {
            base.WriteData(writer);

            writer.Write(Nothing);
        }
    }
}
