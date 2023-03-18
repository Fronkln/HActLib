using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HActLib.OOE;
using Yarhl.IO;

namespace HActLib.OOE
{
    public class ObjectBase : ITEVObject
    {
        //Used only for reads
        public struct _Set1Internal
        {
            public uint Addr;

            //50% certain this is hierarchy
            public int Parent;
            public int PreviousNode;
            public int NextNode;
            public int NextMainNode; //next node that is not a parent of our parent (camera 0 pointing to camera 1 etc)

            public int Set2Ptr;
            public uint UnkNum1;

            public int[] StringTables;

            public int Set3Ptr;
            public int DataPtr1;
            public int DataPtr2;

            public byte[] Unk2;
        }

        public string[] StringTable = new string[4];
        public ObjectNodeCategory Type;
        public uint Category;

        public byte[] Unk1 = new byte[296];
        public float[] UnkFloatArray = new float[4];

        public float[][] UnkFloatDats;

        public Set2 Set2Object = null;
        public EffectBase Set3Object = null;

        public ObjectBase Parent = null;
        public ObjectBase PreviousNode = null;
        public ObjectBase NextNode = null;
        public ObjectBase NextMainNode = null;

        //Don't ever use this for writing the object
        public List<ITEVObject> Children = new List<ITEVObject>();

        public _Set1Internal _InternalInfo = new _Set1Internal();

        public virtual string GetName() => "Unknown Set 1";

        public T GetChildOfType<T>() where T : ITEVObject
        {
            return (T)Children.FirstOrDefault(x => x is T);
        }

        public Set2 GetSet2ChildOfCategory(Set2NodeCategory category)
        {
            return Children.Where(x => x is Set2).Cast<Set2>().FirstOrDefault(x => x.Type == category);
        }

        public Set2[] GetSet2ChildsOfCategory(Set2NodeCategory category)
        {
            return Children.Where(x => x is Set2).Cast<Set2>().Where(x => x.Type == category).ToArray();
        }

        private float[] ReadDataSection(DataReader reader, int position)
        {
            if (position < 0)
                return null;

            reader.Stream.Seek(position, SeekMode.Start);
            float[] dat = new float[]
            {
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle(),
            };

            return dat;
        }

        //Process the pointers and variables etc after read.
        internal virtual void ProcessNodeData(DataReader reader)
        {
            UnkFloatDats = new float[2][];

            if (_InternalInfo.DataPtr1 > -1)
                UnkFloatDats[0] = ReadDataSection(reader, _InternalInfo.DataPtr1);

            if (_InternalInfo.DataPtr1 > 1)
                UnkFloatDats[1] = ReadDataSection(reader, _InternalInfo.DataPtr2);


        }

        internal virtual void WriteSetData(DataWriter writer)
        {
            foreach (int i in _InternalInfo.StringTables)
                writer.Write(i);

            writer.Write((uint)Type);
            writer.Write(Category);
            writer.Write(Unk1);

            foreach (float f in UnkFloatArray)
                writer.Write(f);

            writer.Write(_InternalInfo.Parent);
            writer.Write(_InternalInfo.PreviousNode);
            writer.Write(_InternalInfo.NextNode);
            writer.Write(_InternalInfo.NextMainNode);
            writer.Write(_InternalInfo.Set2Ptr);
            writer.Write(_InternalInfo.UnkNum1);

            writer.Write(_InternalInfo.Set3Ptr);
            writer.Write(_InternalInfo.DataPtr1);
            writer.Write(_InternalInfo.DataPtr2);

            writer.WriteTimes(0, 8);
        }


        //A important moment to find any elements/parents we have.
        internal virtual void OnLoadComplete(TEV tev)
        {
            if (_InternalInfo.Parent > -1)
            {
                Parent = tev.PointerSet1[_InternalInfo.Parent];
                Parent.Children.Add(this);
            }

            if (_InternalInfo.PreviousNode > -1)
                PreviousNode = tev.PointerSet1[_InternalInfo.PreviousNode];

            if (_InternalInfo.NextNode > -1)
                NextNode = tev.PointerSet1[_InternalInfo.NextNode];

            if (_InternalInfo.NextMainNode > -1)
                NextMainNode = tev.PointerSet1[_InternalInfo.NextMainNode];

            if (_InternalInfo.Set2Ptr > -1)
                Set2Object = tev.PointerSet2[_InternalInfo.Set2Ptr];

            if (_InternalInfo.Set3Ptr != -1)
                Set3Object = tev.PointerSet3[_InternalInfo.Set3Ptr];


            if (Set2Object != null)
            {
                int startIndex = Array.IndexOf(tev.Set2, Set2Object);
                uint goal = (uint)startIndex + _InternalInfo.UnkNum1;

                for (int i = startIndex; i < goal; i++)
                {
                    Children.Add(tev.Set2[i]);
                }
            }

            if (Set3Object != null)
            {
                int curIndex = Array.IndexOf(tev.Effects, Set3Object);
                EffectBase curEffect = Set3Object;
                bool finish = false;

                while (true)
                {
                    //HAD TO ADD THIS LINE BECAUSE OF 6090!
                    if (curIndex == -1 || curIndex >= tev.Effects.Length)
                        break;

                    Children.Add(tev.Effects[curIndex]);

                    curIndex++;

                    if (curIndex < tev.Effects.Length)
                    {
                        curEffect = tev.Effects[curIndex];

                        if (curEffect.OptionalUnk != null)
                        {
                            finish = true;
                            continue;
                        }
                    }

                    if (finish)
                        break;
                }
            }
        }

        internal virtual int WriteData(DataWriter writer, float[] dat)
        {
            if (dat == null || dat.Length == 0)
                return -1;

            int position = (int)writer.Stream.Position;

            foreach (float f in dat)
                writer.Write(f);

            writer.WriteTimes(0, 8);

            return position;
        }

        internal int[] WriteStringTable(DataWriter writer, Dictionary<string, long> stringsDict)
        {
            int[] strTable = new int[4] { -1, -1, -1, -1 };

            for (int i = 0; i < strTable.Length; i++)
            {
                string str = StringTable[i];

                if (str != null)
                {
                    strTable[i] = (int)writer.Stream.Position;

                    if (!stringsDict.ContainsKey(str))
                    {
                        stringsDict[str] = strTable[i];
                        writer.Write(str);
                    }
                    else
                        strTable[i] = (int)stringsDict[str];
                }
                else
                    strTable[i] = -1;
            }

            return strTable;
        }
    }
}
