using System;
using Yarhl.IO;

namespace HActLib
{
    public class OMTMoveProperty : OMTBaseProperty
    {
        internal override void ReadData(DataReader reader)
        {
            UnknownData = reader.ReadBytes(16);
        }

        internal override void WriteData(DataWriter writer)
        {

        }
    }
}
