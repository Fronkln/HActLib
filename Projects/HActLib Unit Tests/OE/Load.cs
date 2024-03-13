using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit;
using NUnit.Framework;

namespace HActLib.UnitTests.OE
{
    internal class Load
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public static void LoadVersion10()
        {
            OECMN cmn = OECMN.Read(@"H:\steamapps2\steamapps\common\Yakuza 5\main\data\hact\A31100_chase_tsukamari.par.unpack\a31100_chase_tsukamari\cmn\cmn.bin");
            Assert.AreEqual(10, cmn.CMNHeader.Version, "Version is not 10.");
            Assert.IsNotNull(cmn);
        }

        [Test]
        public static void LoadVersion10_2()
        {
            OECMN cmn = OECMN.Read(@"C:\Users\orhan\source\repos\HActLib\Projects\HActLib Unit Tests\OE\Samples\Y5\h11130_hyakuretu_duki\cmn.par.unpack\cmn\cmn.bin");
            Assert.AreEqual(10, cmn.CMNHeader.Version, "Version is not 10.");
            Assert.IsNotNull(cmn);
        }
        [Test]
        public static void LoadVersion15()
        {
            OECMN cmn = OECMN.Read(@"C:\Users\orhan\Downloads\Ishin hact\hact.par.unpack\h60010_furo.par.unpack\cmn.par.unpack\cmn.bin");
            Assert.AreEqual(15, cmn.CMNHeader.Version, "Version is not 15.");
            Assert.IsNotNull(cmn);
        }

        [Test]
        public static void LoadVersion16()
        {
            OECMN cmn = OECMN.Read(@"G:\Program Dosyaları (x86)\SteamLibrary\steamapps\common\Yakuza 0\media\data\hact.par.unpack\h23140_makoto_guard.par.unpack\cmn.par.unpack\cmn.bin");

            foreach (NodeElement element in cmn.AllElements)
                if (element is OEElementSE)
                    if ((element as OEElementSE).IsFighterSound())
                    {
                        Console.WriteLine("wowza");
                    }
                    else
                        Console.WriteLine("nooo");

            Assert.AreEqual(16, cmn.CMNHeader.Version, "Version is not 16.");
            Assert.IsNotNull(cmn);
        }
    }
}
