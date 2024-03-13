using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib
{
    public class CSVHAct
    {
        public string Name = "HACT";
        public uint ID = 0;
        public int Unk1 = 0;
        public int Unk2 = 500;
        public int Flags = 8192;
        public byte[] UnkSection1 = new byte[44]; //not padding, checked.

        public List<CSVCharacter> Characters = new List<CSVCharacter>();
        public List<CSVHActEvent> SpecialNodes = new List<CSVHActEvent>();
        public List<CSVSection4> Section4 = new List<CSVSection4>(); //Unknown;

        public override string ToString()
        {
            return Name;
        }

        public CSVHActEvent TryGetHActEventData(string type)
        {
            return SpecialNodes.FirstOrDefault(x => x.Type == type);
        }

        public CSVCharacter TryGetHActCharacter(string name)
        {
            return Characters.FirstOrDefault(x => x.Name == name);
        }
    }
}
