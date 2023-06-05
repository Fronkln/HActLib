using System;
using Yarhl.IO;

namespace HActLib
{
    public class MepEffect
    {
        internal virtual void Read(DataReader reader, MEPVersion version)
        {
        }

        internal virtual void Write(DataWriter writer, MEPVersion version)
        {

        }
    }
}
