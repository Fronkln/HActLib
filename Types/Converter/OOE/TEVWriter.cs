﻿using System;
using System.Collections.Generic;
using Yarhl.IO;
using Yarhl.FileFormat;
using HActLib.OOE;
using System.Data;
using System.Linq;
using System.Diagnostics;

namespace HActLib
{
    public class TEVWriter : IConverter<TEV, BinaryFormat>
    {
        public bool Optimize = true;

        public BinaryFormat Convert(TEV tev)
        {
            Dictionary<ObjectBase, long> h_set1Addresses = new Dictionary<ObjectBase, long>();
            Dictionary<Set2, long> h_set2Addresses = new Dictionary<Set2, long>();
            Dictionary<EffectBase, long> h_set3Addresses = new Dictionary<EffectBase, long>();
            Dictionary<string, long> h_strTableAddresses = new Dictionary<string, long>();

            var binary = new BinaryFormat();
            var writer = new DataWriter(binary.Stream);
            writer.DefaultEncoding = System.Text.Encoding.GetEncoding(932);

            writer.Endianness = EndiannessMode.BigEndian;

            //Write core information about hact_tev
            writer.Write(new byte[] { 0x54, 0x43, 0x41, 0x48 });
            writer.Write(0x102);
            writer.Write(20081118);
            writer.WriteTimes(0, 4);

            //We will return to this later once we are ready to finish it up
            //Header offset = 0x16
            long pointersArea = writer.Stream.Position;

            //Fill it with empty bytes for now
            writer.WriteTimes(0, 128);

            long set1Start = writer.Stream.Position;

            ObjectBase[] objects = tev.AllObjects;

            //Writing will be done in multiple passes. We will process nodes multiple times as we need it.
            //Pass 1: Write set 1 data without any regard for pointers.
            for (int i = 0; i < objects.Length; i++)
            {
                ObjectBase set = objects[i];
                h_set1Addresses[set] = writer.Stream.Position;


                if (Optimize)
                {
                    Set2[] set2s = set.GetChildSet2();

                    foreach (Set2 set2Child in set2s)
                    {
                        if (set2Child is Set2Element)
                        {
                            Set2Element set2Elem = set2Child as Set2Element;

                            if(set2Elem.Effect != null && (set2Elem.EffectID != EffectID.Dummy && set2Elem.EffectID == set2Elem.Effect.ElementKind))
                                set.Children.Remove(set2Child);
                        }
                    }
                }

                writer.Write(i);
                set.WriteSetData(writer);
            }

            long set2Start = writer.Stream.Position;

            Set2[] set2 = tev.AllSet2;

            //Pass 1: Write set 2 data without any regard for pointers.
            for (int i = 0; i < set2.Length; i++)
            {
                Set2 set = set2[i];

                h_set2Addresses[set] = writer.Stream.Position;
#if DEBUG
                Debug.WriteLine("Writing Set2" + set.GetName() + "at " + writer.Stream.Position);
#endif

                writer.Write(i);
                set.WriteSetData(writer, true);
            }

            long set3Start = writer.Stream.Position;

            EffectBase[] effects = tev.AllEffects;

            //Write set 3 data.


            for(int i = 0; i < objects.Length; i++)
            {
                ObjectBase tevObj = objects[i];
                EffectBase[] objEffects = tevObj.GetChildEffects();

                for(int j = 0; j < objEffects.Length; j++)
                {
                    EffectBase effect = objEffects[j];
                    h_set3Addresses[effect] = writer.Stream.Position;

                    effect.WriteToStream(writer, false);

                    int idx = Array.IndexOf(effects, effect);

                    if (j == objEffects.Length - 1 || idx >= effects.Length - 1)
                    {
                        writer.WriteTimes(255, 16);
                        writer.WriteTimes(0, 16);
                    }
                }
            }

            int unkPtr1 = (int)writer.Stream.Position;
            //writer.Align(512);

            int setData1Start = (int)writer.Stream.Position;

            //Write set 1 data section 1 (and resources which is part of data)

            foreach (ObjectBase set in objects)
            {
                set._InternalInfo.DataPtr1 = set.WriteData(writer, set.UnkFloatDats[0]);
            }

            if(tev.TEVHeader.DataPadding != 0)
                writer.WriteTimes(0, tev.TEVHeader.DataPadding); //try to preserve TEV as much as possible
            else
                writer.Align(512);

            int weirdSpaceStart = 0;

            if (tev.TEVHeader.DataPadding == 0)
            {
                weirdSpaceStart = (int)writer.Stream.Position;
                writer.WriteTimes(0, 44);
            }

            int setData2Start = (int)writer.Stream.Position;

            if(tev.TEVHeader.DataPadding != 0)
            {
                weirdSpaceStart = setData2Start - tev.TEVHeader.WeirdSpaceOffset;
            }

            foreach (ObjectBase set in objects)
                set._InternalInfo.DataPtr2 = set.WriteData(writer, set.UnkFloatDats[1]);

            int stringsStart = (int)writer.Stream.Position;

            writer.Endianness = EndiannessMode.LittleEndian;

            if (tev.CuesheetIDs.Count < 0)
                writer.Write(0);
            else
                foreach (int cuesheet in tev.CuesheetIDs)
                    writer.Write(cuesheet);

            writer.Endianness = EndiannessMode.BigEndian;

            //Adjust pointers after everything was written
            for(int i = 0; i < objects.Length; i++)
            {
                ObjectBase set = objects[i];

                int nodeIdx = Array.IndexOf(objects, set);
                bool isFirstNode = nodeIdx == 0;
                bool isLastNode = nodeIdx >= objects.Length - 1;
                bool isMainNode = nodeIdx == 0 || objects[0].Children.Contains(set);
                bool isFirstChild = !isMainNode && set.Parent != null && set.Parent.Children.IndexOf(set) >= set.Parent.Children.Count - 1;
                bool isLastChild = !isMainNode && set.Parent != null && set.Parent.Children.IndexOf(set) >= set.Parent.Children.Count - 1;

                /*
                    Previous Node: Previous node in hierarchy. -1 for main nodes.
                    Example:          
                        Path (None)
                            Human (None)
                                Bone 1 (None)
                                Bone (Bone 1)                  
                */
                /*
                    Next Node: Next node in hierarchy. -1 if no next node in the sub node.
                    Example:          
                        Path (Human)
                            Human (Bone 1)
                                Bone 1 (Bone 2)
                                Bone 2 (None) (end of sub hierarchy)                  
                */
                /*
                    Next Main Node: Next main node in hierarchy. -1 if no more main nodes are left.
                    Example:          
                        Path (None)
                            Human (Human 2)
                                Bone 1 (None)
                                Bone 2 (None)
                            Human 2 (None)
                                Bone 1 (None)
                                Bone 2 (None)               
                */

                ObjectBase[] objChildren = set.GetChildObjects();

                set._InternalInfo.Parent = (int)(set.Parent == null ? -1 : h_set1Addresses[set.Parent]);

                #region Next Node

                if (isFirstNode)
                    set._InternalInfo.NextNode = (int)(objects.Length <= 1 ? -1 : h_set1Addresses[objects[i + 1]]);
                else
                {
                    if(isMainNode)
                        set._InternalInfo.NextNode = (int)(objChildren.Length == 0 ? -1 : h_set1Addresses[objChildren[0]]);
                    else
                    {
                        set._InternalInfo.NextNode = -1;

                        /*
                        //Subnode, example: Bone 1
                        ObjectBase[] parentChildren = set.Parent.GetChildObjects();
                        int parentIdx = Array.IndexOf(parentChildren, set);

                        set._InternalInfo.NextNode = (int)(parentIdx >= parentChildren.Length - 1 ? -1 : h_set1Addresses[parentChildren[parentIdx + 1]]);
                        */
                    }
                }

                #endregion

                #region Previous Node

                if (isFirstNode || isMainNode)
                {
                    if(isFirstNode)
                        set._InternalInfo.PreviousNode = -1;
                    else
                    {
                        ObjectBase[] parentChildren = set.Parent.GetChildObjects();
                        int parentIdx = Array.IndexOf(parentChildren, set);

                        set._InternalInfo.PreviousNode = (int)((parentIdx == 0 || parentChildren.Length == 1) ? -1 : h_set1Addresses[parentChildren[parentIdx - 1]]);
                    }
                }
                else
                {
                    ObjectBase[] parentChildren = set.Parent.GetChildObjects();
                    int parentIdx = Array.IndexOf(parentChildren, set);

                    set._InternalInfo.PreviousNode = (int)((parentIdx == 0 || parentChildren.Length == 1) ? -1 : h_set1Addresses[parentChildren[parentIdx - 1]]);
                }

                #endregion

                #region Next Main Node

                if (isMainNode || isFirstNode)
                {
                    if(isFirstNode)
                        set._InternalInfo.NextMainNode = -1;
                    else
                    {
                        ObjectBase[] parentChildren = set.Parent.GetChildObjects();
                        int parentIdx = Array.IndexOf(parentChildren, set);

                        set._InternalInfo.NextMainNode = (int)(parentIdx >= parentChildren.Length - 1 ? -1 : h_set1Addresses[parentChildren[parentIdx + 1]]);
                    }
                }
                else
                {
                    if (!isLastChild)
                    {
                        ObjectBase[] parentChildren = set.Parent.GetChildObjects();
                        int parentIdx = Array.IndexOf(parentChildren, set);

                        set._InternalInfo.NextMainNode = (int)(parentIdx >= parentChildren.Length - 1 ? -1 : h_set1Addresses[parentChildren[parentIdx + 1]]);
                    }
                    else
                        set._InternalInfo.NextMainNode = -1;

                }

                #endregion

                set._InternalInfo.StringTables = set.WriteStringTable(writer, h_strTableAddresses);


                EffectBase[] effectChildren = set.GetChildEffects();
                Set2[] set2Children = set.GetChildSet2();

                set._InternalInfo.Set2Ptr = (set2Children.Length <= 0 ? -1 : (int)h_set2Addresses[set2Children[0]]);
                set._InternalInfo.UnkNum1 = (uint)set2Children.Length;
                set._InternalInfo.Set3Ptr = (effectChildren.Length <= 0 ? - 1 :  (int)h_set3Addresses[effectChildren[0]]);


                foreach(Set2 set2Obj in set.GetChildSet2())
                {
                   set2Obj._InternalInfo.resourcePtr = set2Obj.WriteResource(writer);
                }
                //if (set.Set2Object != null)
                    //set.Set2Object._InternalInfo.resourcePtr = set.Set2Object.WriteResource(writer);
            }

            writer.Stream.Seek(set1Start, SeekMode.Start);

            for (int i = 0; i < objects.Length; i++)
            {
                ObjectBase set = objects[i];
                h_set1Addresses[set] = writer.Stream.Position;

                writer.Write(i);
                set.WriteSetData(writer);
            }

            for (int i = 0; i < set2.Length; i++)
            {
                Set2 set = set2[i];
                h_set2Addresses[set] = writer.Stream.Position;

                writer.Write(i);
                set.WriteSetData(writer, true);
            }

            tev.TEVHeader.CameraCount = tev.AllObjects.Where(x => x is ObjectCamera).Count();
            tev.TEVHeader.CameraCount2 = tev.TEVHeader.CameraCount;
            tev.TEVHeader.CameraCount3 = tev.TEVHeader.CameraCount;

            tev.TEVHeader.CharacterCount = tev.AllObjects.Where(x => x is ObjectHuman).Count();
            tev.TEVHeader.CharacterCount2 = tev.TEVHeader.CharacterCount;

            tev.TEVHeader.SpecialElementCount = tev.AllSet2.Where(x => x is Set2Element1019).Count();
            tev.TEVHeader.UseSoundACB = tev.CuesheetIDs.Count;

            //Dont know what these do yet
            if (tev.TEVHeader.UnkVal2 == 0)
                tev.TEVHeader.UnkVal2 = 1;

            if (tev.TEVHeader.UnkVal3 == 0)
                tev.TEVHeader.UnkVal3 = 1;

            if (tev.TEVHeader.UnkVal4 == 0)
                tev.TEVHeader.UnkVal4 = 25;

            if (tev.TEVHeader.UnkVal6 == 0)
                tev.TEVHeader.UnkVal6 = 19;

            //finish header
            writer.Stream.Seek(pointersArea, SeekMode.Start);

            writer.Write((int)set1Start);
            writer.Write(objects.Length);

            writer.Write((int)set2Start);
            writer.Write(set2.Length);

            writer.Write((int)set3Start);
            writer.Write(setData1Start);

            writer.Write(objects.Length);

           
            writer.Write(setData2Start);

            writer.Write(tev.TEVHeader.CameraCount3);
            writer.Write(stringsStart);

            writer.Write(tev.TEVHeader.UseSoundACB);
            writer.Write(weirdSpaceStart);

            writer.Write(tev.TEVHeader.CameraCount);
            writer.Write(tev.TEVHeader.CameraCount2);
            writer.Write(tev.TEVHeader.CharacterCount);
            writer.Write(tev.TEVHeader.CharacterCount2);

            writer.Write(tev.TEVHeader.UnkRegion1);
            writer.Write(tev.TEVHeader.UnkVal1);
            writer.Write(tev.TEVHeader.UnkVal2);
            writer.Write(tev.TEVHeader.UnkVal3);
            writer.Write(tev.TEVHeader.UnkVal4);
            writer.Write(tev.TEVHeader.SpecialElementCount);
            writer.Write(tev.TEVHeader.UnkVal6);
            writer.Write(tev.TEVHeader.UnkVal7);


            writer.Write(stringsStart + 4);

            //writer.Write(unkPtr1);

            writer.Stream.Seek(writer.Stream.Length - 1);
            writer.Align(16);
            

            return binary;
        }
    }
}
