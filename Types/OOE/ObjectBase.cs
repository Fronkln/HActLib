using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public string[] StringTable = new string[4] { null, null, null, null };
        public ObjectNodeCategory Type;
        public uint Category;

        public uint Unk2 = 1;

        public byte[] Unk1;
        public float[] UnkFloatArray = new float[4] { 1f, 1f, 1f, 1f };

        public float[][] UnkFloatDats = new float[2][];

        public Set2 Set2Object = null;
        public EffectBase Set3Object = null;

        public ObjectBase Parent = null;
        public ObjectBase PreviousNode = null;
        public ObjectBase NextNode = null;
        public ObjectBase NextMainNode = null;

        //Don't ever use this for writing the object
        //Actually, please do
        public List<ITEVObject> Children = new List<ITEVObject>();

        public _Set1Internal _InternalInfo = new _Set1Internal();

        public virtual string GetName() => "Unknown Set 1";


        public ObjectBase()
        {
            _InternalInfo = new _Set1Internal();
            _InternalInfo.StringTables = new int[4] {-1, -1, -1, -1};

            UnkFloatDats[0] = new float[]
            {
                0.580971062f,
                1.01288545f,
                0.185546815f,
                0.7408517f,
                0.9738375f,
                0.7094903f
            };
        }


        public ObjectBase[] GetChildObjects()
        {
            return Children.Where(x => x is ObjectBase).Cast<ObjectBase>().ToArray();
        }

        public EffectBase[] GetChildEffects()
        {
            return Children.Where(x => x is EffectBase).Cast<EffectBase>().ToArray();
        }

        public Set2[] GetChildSet2()
        {
            List<Set2> children = new List<Set2>();

            foreach (ITEVObject child in Children)
                if (child as Set2 != null)
                    children.Add(child as Set2);

            return children.ToArray();
        }

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


        internal static ObjectBase ReadFromMemory(DataReader reader)
        {
            uint type = 0;
            uint category = 0;
            reader.Stream.RunInPosition
                (
                    delegate
                    {
                        type = reader.ReadUInt32();
                        category = reader.ReadUInt32();
                    }, 20, SeekMode.Current
                );

            ObjectBase set = GetObjectType(type, category);
            set.ReadBasicSet1Info(reader);
 
            return set;
        }

        private static ObjectBase GetObjectType(uint type, uint category)
        {
            switch (type)
            {
                default:
                    return new ObjectBase();

                case 1:
                    return new ObjectCamera();
                case 2:
                    return new ObjectPath();
                case 3:
                    if (category >= 0 && category <= 1)
                        return new ObjectHuman();
                    else if (category == 2)
                        return new ObjectWeapon();
                    goto default;
                case 4:
                    return new ObjectModel();

                case 5:
                    return new ObjectBone();
                case 6:
                    return new ObjectItem();
            }

        }

        private void ReadBasicSet1Info(DataReader reader)
        {
            string[] ReadStringTable(int[] table)
            {
                long curPos = reader.Stream.Position;

                List<string> stringTbl = new List<string>();

                foreach (int i in table)
                {
                    if (i != -1)
                    {
                        reader.Stream.Seek(i);
                        stringTbl.Add(reader.ReadString());
                    }
                    else
                        stringTbl.Add(null);
                }

                reader.Stream.Seek(curPos, SeekMode.Start);

                return stringTbl.ToArray();
            }

            _InternalInfo.Addr = (uint)reader.Stream.Position;
            uint index = reader.ReadUInt32();

            int[] stringTable = new int[]
            {
                reader.ReadInt32(),
                reader.ReadInt32(),
                reader.ReadInt32(),
                reader.ReadInt32()
            };

            _InternalInfo.StringTables = stringTable;

            StringTable = ReadStringTable(stringTable);
            Type = (ObjectNodeCategory)reader.ReadUInt32();
            Category = reader.ReadUInt32();
            Unk2 = reader.ReadUInt32();

            long start = reader.Stream.Position;
            long target = reader.Stream.Position + 292;

            ReadObjectData(reader);

            if (reader.Stream.Position < target)
                Unk1 = reader.ReadBytes((int)(target - reader.Stream.Position));
            else if (reader.Stream.Position > target)
                reader.Stream.Position = target;

            //Unk1 = reader.ReadBytes(296);
           
            UnkFloatArray = new float[]
            {
               reader.ReadSingle(),
               reader.ReadSingle(),
               reader.ReadSingle(),
               reader.ReadSingle(),
            };

            _InternalInfo.Parent = reader.ReadInt32();
            _InternalInfo.NextNode = reader.ReadInt32();
            _InternalInfo.PreviousNode = reader.ReadInt32();
            _InternalInfo.NextMainNode = reader.ReadInt32();


            _InternalInfo.Set2Ptr = reader.ReadInt32();
            _InternalInfo.UnkNum1 = reader.ReadUInt32();

            _InternalInfo.Set3Ptr = reader.ReadInt32();
            _InternalInfo.DataPtr1 = reader.ReadInt32();
            _InternalInfo.DataPtr2 = reader.ReadInt32();

            reader.ReadBytes(8);

            long curReadPos = reader.Stream.Position;
            ProcessNodeData(reader);

            ReadSet2(reader, _InternalInfo.Set2Ptr, _InternalInfo.UnkNum1);
            ReadEffect(reader, _InternalInfo.Set3Ptr);

            reader.Stream.Seek(curReadPos);
        }

        internal virtual void ReadObjectData(DataReader reader)
        {

        }

        internal virtual void WriteObjectData(DataWriter writer)
        {

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

        private void ReadSet2(DataReader reader, int pos, uint count)
        {
            if (pos <= 0)
                return;

            reader.Stream.Seek(pos);

            for(int i = 0; i < count; i++)
            {
                Set2 set = Set2.ReadFromMemory(reader);

                //We can just generate this from Effect elements
                //Idk what the point of set 2 elements are.
                //if(set.Type != Set2NodeCategory.Element || set.EffectID == EffectID.Special)

                //TEV Optimization theory
                if(set.Type == Set2NodeCategory.Element)
                {
                    Set2Element set2Element = set as Set2Element;

                    if (set2Element.Effect != null && ((set2Element.EffectID == EffectID.Dummy && set2Element.Type == Set2NodeCategory.Element) || set2Element.EffectID == set2Element.Effect.ElementKind))
                        continue;
                }
                
                Children.Add(set);
            }
        }

        private void ReadEffect(DataReader reader, int pos)
        {
            if (pos <= 0)
                return;

            reader.Stream.Seek(pos);

            bool over = false;

            while (!over)
            {
                (EffectBase, bool) result = EffectBase.ReadFromMemory(reader, false);
                over = result.Item2;

                Children.Add(result.Item1);
            }
        }

        internal virtual void WriteSetData(DataWriter writer)
        {
            foreach (int i in _InternalInfo.StringTables)
                writer.Write(i);

            writer.Write((uint)Type);
            writer.Write(Category);
            writer.Write(Unk2);

            long start = writer.Stream.Position;
            long target = writer.Stream.Position + 292;

            WriteObjectData(writer);

            if(Unk1 != null)
                writer.Write(Unk1);

            if (writer.Stream.Position > target)
                new Exception("Overwrite on Object!");
            else if(writer.Stream.Position < target)
                new Exception("Underwrite on Object!");

            foreach (float f in UnkFloatArray)
                writer.Write(f);

            writer.Write(_InternalInfo.Parent);
            writer.Write(_InternalInfo.NextNode);
            writer.Write(_InternalInfo.PreviousNode);
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
            /*
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


            List<Set2> set2Child = new List<Set2>();
            List<EffectBase> effectChild = new List<EffectBase>();

            if (Set2Object != null)
            {
                int startIndex = Array.IndexOf(tev.Set2, Set2Object);
                uint goal = (uint)startIndex + _InternalInfo.UnkNum1;

                for (int i = startIndex; i < goal; i++)
                {
                    set2Child.Add(tev.Set2[i]);
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

                    effectChild.Add(tev.Effects[curIndex]);

                    curIndex++;

                    if (curIndex < tev.Effects.Length)
                    {
                        curEffect = tev.Effects[curIndex];

                        if (tev.LastChildSet3.Contains(curEffect))
                        {
                            finish = true;
                            continue;
                        }
                    }

                    if (finish)
                        break;
                }
            }

            foreach (ITEVObject obj in set2Child)
                Children.Add(obj);

            foreach (ITEVObject obj in effectChild)
                Children.Add(obj);
            */
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
