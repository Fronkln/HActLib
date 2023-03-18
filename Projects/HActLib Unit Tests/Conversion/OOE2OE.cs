using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace HActLib.UnitTests.Conversion
{
    internal class OOE2OE
    {
        [Test]
        public void OOE_TO_OE_1()
        {
            string path = System.IO.Directory.GetCurrentDirectory();

            string hactDir = @"H:\steamapps2\steamapps\common\Yakuza 4\data\hact\hact_all\1200.par.unpack";
            string tevPath = @"H:\steamapps2\steamapps\common\Yakuza 4\data\hact\hact_csv.bin";

            bool conversionResult = HActFactory.ConvertOOEToOE(hactDir, "ooe_to_oe_16", 16, tevPath);
            Assert.IsTrue(conversionResult);
        }
    }
}
