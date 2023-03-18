using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    public class CSVCharacter
    {
        public string Name;
        public string ModelOverride;

        public byte[] Unk1;

        public override string ToString()
        {
            return Name;
        }
    }
}
