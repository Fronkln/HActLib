using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class CustomDENodeAuthExtendedElement : NodeElement
    {
        public virtual float LastRevision => 0.1f;
        public float customNodeVersion;

        internal override void ReadElementData(DataReader reader, NodeConvInf inf, GameVersion version)
        {
            customNodeVersion = reader.ReadSingle();
        }

        internal override void WriteElementData(DataWriter writer, GameVersion version)
        {
            writer.Write(LastRevision);
        }
    }
}
