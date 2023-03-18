using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class Set2Element1019 : Set2Element
    {
        public string Type1019 = "";

        public override string GetName()
        {
            return Type1019;
        }

        internal override void ReadArgs(DataReader reader)
        {
            Type1019 = reader.ReadString(16).Split(new[] { '\0' }, 2)[0];
            Unk2 = reader.ReadBytes(236);
        }

        internal override void WriteArgs(DataWriter writer)
        {
            writer.Write(Type1019.ToLength(16), false, maxSize: 16);

            base.WriteArgs(writer);
        }
    }
}
