using System;
using System.Runtime.CompilerServices;
using Yarhl.IO;

namespace HActLib
{
    internal class CSVHActEventHeatGauge : CSVHActEvent
    {
        public int Change = 0;

        internal override void ReadData(DataReader reader)
        {
            Change = reader.ReadInt32();
        }
    }
}
