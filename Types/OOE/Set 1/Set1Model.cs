using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class ObjectModel : ObjectBase
    {
        public string DevPath;
        public string ModelLabel;
        public string ModelFile;
        public override string GetName()
        {
            return ModelFile;
        }

        internal override void ProcessNodeData(DataReader reader)
        {
            base.ProcessNodeData(reader);

            DevPath = StringTable[0];
            ModelLabel = StringTable[1];
            ModelFile = StringTable[2];
        }

        internal override void WriteSetData(DataWriter writer)
        {
            base.WriteSetData(writer);
        }
    }
}
