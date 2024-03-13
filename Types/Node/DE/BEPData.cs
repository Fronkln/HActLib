using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    public class BEPData
    {
        public Guid Guid2 = Guid.Empty;
        public PXDHash Bone = new PXDHash();
        public ushort PropertyType; //Only used for elemeents
        public int Unk;
        public int DataUnk;

        /// <summary>
        /// Only in BEP elements
        /// </summary>
        public uint ElementUnk;
    }
}
