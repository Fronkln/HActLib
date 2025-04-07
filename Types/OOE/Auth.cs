using HActLib.Internal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Yarhl.IO;
using HActLib.OOE;

namespace HActLib
{
    public class Auth
    {
        public float Length = 0;

        public List<AuthNodeOOE> Nodes = new List<AuthNodeOOE>();
        public List<AuthResourceCut> ResourceCuts = new List<AuthResourceCut>();
        public List<AuthResourceCut> CameraCuts = new List<AuthResourceCut>();
        public List<AuthReference> References = new List<AuthReference>();

        public AuthNodeOOE[] AllNodes
        {
            get
            {
                List<AuthNodeOOE> nodes = new List<AuthNodeOOE>();

                void Process(AuthNodeOOE node)
                {
                    nodes.Add(node);

                    foreach (AuthNodeOOE child in node.Children)
                        Process(child);
                }

                foreach (var node in Nodes)
                    Process(node);

                return nodes.ToArray();
            }
        }

        public static Auth Read(string path)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path));
        }

        public static Auth Read(byte[] buffer)
        {
            Reflection.Process();

            Auth auth = new Auth();

            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader authReader = new DataReader(readStream) { Endianness = EndiannessMode.BigEndian, DefaultEncoding = Encoding.GetEncoding(932) };
            authReader.Stream.Position += 16;

            int fileSize = authReader.ReadInt32();

            SizedPointer resourceCutPtr = authReader.Read<SizedPointer>();
            uint entriesStart = authReader.ReadUInt32();
            SizedPointer referencesPtr = authReader.Read<SizedPointer>();

            auth.Length = authReader.ReadSingle();

            SizedPointer cameraCutPtr = authReader.Read<SizedPointer>();

            authReader.Stream.Position = resourceCutPtr.Pointer;

            for (int i = 0; i < resourceCutPtr.Size; i++)
            {
                auth.ResourceCuts.Add(new AuthResourceCut() { Start = authReader.ReadSingle(), End = authReader.ReadSingle() });
                authReader.Stream.Position += 8;
            }

            //flipped for some reason
            authReader.Stream.Position = cameraCutPtr.Size;

            for (int i = 0; i < cameraCutPtr.Pointer; i++)
            {
                auth.CameraCuts.Add(new AuthResourceCut() { Start = authReader.ReadSingle(), End = authReader.ReadSingle() });
                authReader.Stream.Position += 8;
            }

            authReader.Stream.Position = referencesPtr.Pointer;

            for (int i = 0; i < referencesPtr.Size; i++)
            {
                auth.References.Add(new AuthReference() { Unknown = authReader.ReadInt32(), Guid = new Guid(authReader.ReadBytes(16)) });
                authReader.Stream.Position += 12;
            }

            //Reads one entry
            AuthNodeOOE EntryProcedure(uint child, uint next, AuthNodeOOE parent)
            {
                bool isChild = false;

                uint entryStart = child;

                if (child == 0)
                {
                    entryStart = next;
                    next = 0;
                }
                else
                    isChild = true;

                authReader.Stream.Position = entryStart;

                AuthNodeOOE node = new AuthNodeOOE();
                node.Parent = parent;

                if(node.Parent != null)
                    node.Parent.Children.Add(node);

                if(!isChild)
                    auth.Nodes.Add(node);

                node.Type = (AuthNodeTypeOOE)authReader.ReadInt32();
                node.Unknown1 = authReader.ReadInt32();

                node.Guid = new Guid(authReader.ReadBytes(16));
                node.Unknown2 = new float[3] { authReader.ReadSingle(), authReader.ReadSingle(), authReader.ReadSingle() };

                node.Unknown3 = authReader.ReadInt32();
                node.Unknown4 = new float[4] { authReader.ReadSingle(), authReader.ReadSingle(), authReader.ReadSingle(), authReader.ReadSingle() };
                node.Unknown5 = new float[3] { authReader.ReadSingle(), authReader.ReadSingle(), authReader.ReadSingle() };

                int childCount = authReader.ReadInt32();

                if (childCount > 1)
                    throw new Exception("Child count greater than 1? this is new");

                node.Unknown6 = new float[6] { authReader.ReadSingle(), authReader.ReadSingle(), authReader.ReadSingle(), authReader.ReadSingle(), authReader.ReadSingle(), authReader.ReadSingle() };

                int animationDataPointer = authReader.ReadInt32();
                node.Unknown7 = authReader.ReadInt32();
                int effectsPtr = authReader.ReadInt32();

                uint childNodeOffset = authReader.ReadUInt32();
                uint nextNodeOffset = authReader.ReadUInt32();

                if (animationDataPointer != 0)
                {
                    node.AnimationData = new AuthNodeOOEAnimationData();
                    authReader.Stream.RunInPosition(delegate
                    {
                        node.AnimationData.Unknown1 = authReader.ReadInt32();
                        node.AnimationData.Unknown2 = authReader.ReadInt32();
                        node.AnimationData.Guid = new Guid(authReader.ReadBytes(16));

                        node.AnimationData.StartFrame = authReader.ReadSingle();
                        node.AnimationData.EndFrame = authReader.ReadSingle();
                        node.AnimationData.UnknownData = authReader.ReadBytes(96);
                    }, animationDataPointer);
                }

                if (effectsPtr != 0)
                {
                    authReader.Stream.RunInPosition(delegate
                    {
                        if (authReader.ReadInt32() == -1)
                            return;

                        authReader.Stream.Position -= 4;

                        while (true)
                        {
                            var result = EffectBase.ReadFromMemory(authReader, false);

                            node.Effects.Add(result.Item1);

                            if (result.Item2)
                                break;
                        }
                    }, effectsPtr);
                }


                bool addingChild = false;

                child = childNodeOffset;

                if (nextNodeOffset != 0)
                    next = nextNodeOffset;

                if (child != 0 || next != 0)
                {
                    if (child != 0)
                        addingChild = true;

                    var result = EntryProcedure(child, next, node.Parent);

                    if (addingChild)
                    {
                        result.Parent = node;
                        node.Children.Add(result);
                    }

                    if(isChild)
                    {
                        if (result.Parent != null)
                            result.Parent.Children.Add(result);
                    }
                }

                return node;
            }

            authReader.Stream.Position = entriesStart;
            EntryProcedure(0, entriesStart, null);

            return auth;
        }

        public static void Write(Auth file, string path)
        {
            //Write order = Nodes > Resource Cut > Camera Cut > References > Animation Data > Effect Data

            var binary = new BinaryFormat();
            var writer = new DataWriter(new DataStream());
            writer.DefaultEncoding = Encoding.GetEncoding(932);

            writer.Endianness = EndiannessMode.BigEndian;

            writer.Write(1096111176);
            writer.Write(33619968);
            writer.Write(16777216);
            writer.Write(0);

            long headerStart = writer.Stream.Position;
            writer.WriteTimes(0, 48);

            long entriesStart = writer.Stream.Position;

            Dictionary<AuthNodeOOE, uint> nodeLocs = new Dictionary<AuthNodeOOE, uint>();

            //multiple nodes can reference one animation data
            Dictionary<Guid, uint> animDataLocs = new Dictionary<Guid, uint>();
            Dictionary<AuthNodeOOE, uint> effectDataLocs = new Dictionary<AuthNodeOOE, uint>();

            AuthNodeOOE[] nodes = file.AllNodes;

            foreach (var node in nodes)
            {
                long start = writer.Stream.Position;
                nodeLocs[node] = (uint)start;

                writer.Write((uint)node.Type);
                writer.Write(node.Unknown1);

                writer.Write(node.Guid.ToByteArray());

                foreach (float f in node.Unknown2)
                    writer.Write(f);

                writer.Write(node.Unknown3);

                foreach (float f in node.Unknown4)
                    writer.Write(f);

                foreach (float f in node.Unknown5)
                    writer.Write(f);

                writer.Write(node.Children.Count);

                foreach (float f in node.Unknown6)
                    writer.Write(f);

                writer.Write(0);
                writer.Write(node.Unknown7);
                writer.Write(0);

                writer.Write(0);
                writer.Write(0);

                writer.WriteTimes(0, 12);

                long diff = writer.Stream.Position - start;

                if (diff != 128)
                    throw new Exception("you fucked up my face");
            }

            long resourceCutStart = writer.Stream.Position;

            foreach (AuthResourceCut cut in file.ResourceCuts)
            {
                writer.Write(cut.Start);
                writer.Write(cut.End);

                writer.WriteTimes(0, 8);
            }

            long cameraCutStart = writer.Stream.Position;

            foreach (AuthResourceCut cut in file.CameraCuts)
            {
                writer.Write(cut.Start);
                writer.Write(cut.End);

                writer.WriteTimes(0, 8);
            }

            long referencesStart = writer.Stream.Position;

            foreach (AuthReference reference in file.References)
            {
                writer.Write(reference.Unknown);
                writer.Write(reference.Guid.ToByteArray());
                writer.WriteTimes(0, 12);
            }

            //write animation data
            foreach (var node in nodes)
            {
                if (node.AnimationData != null)
                {
                    if (animDataLocs.ContainsKey(node.AnimationData.Guid))
                    {
                        continue;
                    }

                    animDataLocs[node.AnimationData.Guid] = (uint)writer.Stream.Position;
                    writer.Write(node.AnimationData.Unknown1);
                    writer.Write(node.AnimationData.Unknown2);
                    writer.Write(node.AnimationData.Guid.ToByteArray());

                    writer.Write(node.AnimationData.StartFrame);
                    writer.Write(node.AnimationData.EndFrame);

                    writer.Write(node.AnimationData.UnknownData);
                }
            }

            //and now. effect data
            foreach (var node in nodes)
            {
                effectDataLocs[node] = (uint)writer.Stream.Position;

                foreach (var effect in node.Effects)
                {
                    effect.WriteToStream(writer, false);
                }

                writer.WriteTimes(0xFF, 16);
                writer.WriteTimes(0, 16);
            }

            //now lets go back and correct our pointers
            for (int i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];

                writer.Stream.Seek(nodeLocs[node] + 96);
                writer.Write(animDataLocs[node.AnimationData.Guid]);
                writer.Stream.Position += 4;
                writer.Write(effectDataLocs[node]);


                uint childPtr = 0;
                uint nextPtr = 0;

                if (node.Children.Count > 0)
                    childPtr = nodeLocs[node.Children[0]];
                else
                    childPtr = 0;

                if (i == 0)
                {
                    if (nodes.Length > 1)
                        nextPtr = nodeLocs[nodes[2]];
                    else
                        nextPtr = 0;
                }
                else
                {

                    if(node.Parent != null)
                    {
                        int myIdx = node.Parent.Children.IndexOf(node);

                        if(myIdx == node.Parent.Children.Count - 1)
                        {
                            nextPtr = 0;
                        }
                        else
                        {
                            nextPtr = nodeLocs[node.Parent.Children[myIdx + 1]];
                        }
                    }
                    /*

                    if (i == nodes.Length - 1)
                        writer.Write(0);
                    else
                        writer.Write(nodeLocs[nodes[i + 1]]);
                    ü*/
                }


                writer.Write(childPtr);
                writer.Write(nextPtr);
            }


            writer.Stream.Position = headerStart;
            writer.Write((uint)writer.Stream.Length);

            writer.Write((uint)resourceCutStart);
            writer.Write(file.ResourceCuts.Count);

            writer.Write((uint)entriesStart);

            writer.Write((uint)referencesStart);
            writer.Write(file.References.Count);

            writer.Write(file.Length);

            writer.Write(file.CameraCuts.Count);
            writer.Write((uint)cameraCutStart);

            File.WriteAllBytes(path, writer.Stream.ToArray());
        }

    }
}
