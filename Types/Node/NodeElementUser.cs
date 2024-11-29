using System;
using System.Collections.Generic;
using Yarhl.IO;

namespace HActLib
{
    //HActLib user defined node
    public class NodeElementUser : NodeElement
    {
        public List<UserElementField> Fields = new List<UserElementField>();

        internal override void WriteElementData(DataWriter writer, GameVersion version, int hactVer)
        {
            foreach (UserElementField field in Fields)
                field.Write(writer);
        }

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            foreach (UserElementField field in Fields)
                field.Read(reader);
        }
    }
}
