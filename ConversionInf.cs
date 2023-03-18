using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;


namespace HActLib
{
    public class ConversionInf
    {
        public BinaryFormat format;
        public EndiannessMode endianness;

        public ConversionInf() { }

        public ConversionInf(BinaryFormat format, EndiannessMode endianness)
        {
            this.format = format;
            this.endianness = endianness;
        }
    }
}
