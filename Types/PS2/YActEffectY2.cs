﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;

namespace HActLib.YAct
{
    public class YActEffectY2 : YActEffect
    {
        internal override void ReadData(DataReader reader)
        {
            Unknown2 = reader.ReadInt32();

            Start = reader.ReadSingle();
            End = reader.ReadSingle();
            Unknown3 = reader.ReadSingle();
            UnknownData = reader.ReadBytes(84);
        }

        internal override void WriteData(DataWriter writer)
        {

        }
    }
}