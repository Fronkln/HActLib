using System;
using System.Runtime.CompilerServices;
using Yarhl.IO;

namespace HActLib
{
    public class CSVHActEventHeatGauge : CSVHActEvent
    {
        public int Change = 0;

        public CSVHActEventHeatGauge()
        {
            Type = CSVHActEventType.HeatChange;
        }

        internal override void ReadData(DataReader reader)
        {
            Change = reader.ReadInt32();
        }

        internal override void WriteData(DataWriter writer)
        {
            base.WriteData(writer);

            writer.Write(Change);
        }
    }
}
