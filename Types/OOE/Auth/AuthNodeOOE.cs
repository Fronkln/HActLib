using HActLib.OOE;
using System;
using System.Collections.Generic;
using Yarhl.IO;

namespace HActLib
{
    public class AuthNodeOOE
    {
        public AuthNodeTypeOOE Type;
        public int Unknown1;

        public Guid Guid = new Guid();

        public float[] Unknown2 = new float[3];

        public int Unknown3 = 0;

        public float[] Unknown4 = new float[4];
        public float[] Unknown5 = new float[3];
        public float[] Unknown6 = new float[6];

        public int Unknown7 = 0;

        public AuthNodeOOEAnimationData AnimationData = null;

        public AuthNodeOOE Parent = null;

        public List<AuthNodeOOE> Children = new List<AuthNodeOOE>();
        public List<EffectBase> Effects = new List<EffectBase>();

        public override string ToString()
        {
            return Guid.ToString();
        }

        internal virtual void Read(DataReader reader)
        {

        }
    }
}
