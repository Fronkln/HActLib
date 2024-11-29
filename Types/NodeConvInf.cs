using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class NodeConvInf
    {
        public BinaryFormat format;
        public uint version;
        public int expectedSize;
        public long endAddress;
        public GameVersion gameVersion;
        public AuthFile file;
    }
}
