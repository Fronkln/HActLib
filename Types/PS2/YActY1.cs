using HActLib.YAct;
using System;
using System.IO;
using System.Collections.Generic;
using Yarhl.IO;

namespace HActLib.YAct
{
    public class YActY1 : BaseYAct
    {
        internal static new YActY1 Read(byte[] buffer)
        {
            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader yactReader = new DataReader(readStream) { Endianness = EndiannessMode.LittleEndian, DefaultEncoding = System.Text.Encoding.GetEncoding(932) };

            YActY1 yact = new YActY1();
            YActHeaderY1 yactHeader = yactReader.Read<YActHeaderY1>();

            //YACT Y1: Info chunks for character and camera do not exist, we are reading the animations header directly
            //They are defined in CSV in Y1, until we read that as well, this is the only way.
            yact.ReadCharacterAnimations(yactReader, yactHeader.CharacterAnimations);
            yact.ReadCameras(yactReader, yactHeader.CameraAnimations, new List<YActFile>());
            yact.ReadEffects(yactReader, yactHeader.Effects);
            yact.ReadUnk2s(yactReader, yactHeader.Chunk2);
            yact.ReadUnk3s(yactReader, yactHeader.Chunk3);

            for (int i = 0; i < yact.CharacterAnimations.Count; i++)
            {
                YActCharacter chara = new YActCharacter();
                yact.Characters.Add(chara);
            }

            return yact; 
        }

        protected override void ReadEffects(DataReader yactReader, SizedPointer effectChunk)
        {
            Effects = new List<YActEffect>();

            if (effectChunk.Size <= 0)
                return;

            yactReader.Stream.Seek(effectChunk.Pointer);

            for (int i = 0; i < effectChunk.Size; i++)
            {
                yactReader.Stream.RunInPosition(delegate
                {
                    int type = 0;
                    yactReader.Stream.RunInPosition(delegate { type = yactReader.ReadInt32(); }, 0x2C, SeekMode.Current);

                    YActEffect effect = null;

                    switch((YActEffectType)type)
                    {
                        default:
                            effect = new YActEffect();
                            break;
                        case YActEffectType.Sound:
                            effect = new YActEffectSound();
                            break;
                        case YActEffectType.Particle:
                            effect = new YActEffectParticle();
                            break;
                    }

                    effect.ReadData(yactReader);
                    Effects.Add(effect);
                }, yactReader.ReadUInt32());
            }
        }

        public static new void Write(string path, BaseYAct yact)
        {
            DataWriter writer = new DataWriter(new DataStream()) { Endianness = EndiannessMode.LittleEndian };
            long headerPos = writer.Stream.Position;

            writer.WriteTimes(0, 64);

            long effectsPointerTableStart = writer.Stream.Position;
            writer.WriteTimes(0, yact.Effects.Count * 4);

            long chunk2PointerTableStart = writer.Stream.Position;
            writer.WriteTimes(0, yact.Unks2.Count * 4);

            long chunk3PointerTableStart = writer.Stream.Position;
            writer.WriteTimes(0, yact.Unks3.Count * 4);

            long cameraPointerTableStart = writer.Stream.Position;
            writer.WriteTimes(0, yact.Cameras.Count * 4);

            //unknown
            long chunk5TableStart = writer.Stream.Position;
            writer.WriteTimes(0, 0);

            long characterPointerTableStart = writer.Stream.Position;
            writer.WriteTimes(0, yact.CharacterAnimations.Count * 4);

            //unknown
            long chunk7TableStart = writer.Stream.Position;
            writer.WriteTimes(0, 0);

            writer.Align(16);

            long effectDataStart = writer.Stream.Position; 
            uint[] effectLocations = new uint[yact.Effects.Count];

            for(int i = 0; i < yact.Effects.Count; i++)
            {
                long end = writer.Stream.Position + 96;

                effectLocations[i] = (uint)writer.Stream.Position;
                yact.Effects[i].WriteData(writer);

                if (writer.Stream.Position != end)
                    throw new Exception($"Write position mismatch on effect IDX {i} ({(YActEffectType)yact.Effects[i].Type}), expected {end}, got {writer.Stream.Position}");
            }

            long unk2DataStart = writer.Stream.Position;
            uint[] unk2Locations = new uint[yact.Unks2.Count];

            for(int i = 0; i < yact.Unks2.Count; i++)
            {
                unk2Locations[i] = (uint)writer.Stream.Position;

                foreach(float f in yact.Unks2[i].Data)
                    writer.Write(f);
            }

            long unk3DataStart = writer.Stream.Position;
            uint[] unk3Locations = new uint[yact.Unks3.Count];

            for (int i = 0; i < yact.Unks3.Count; i++)
            {
                unk3Locations[i] = (uint)writer.Stream.Position;

                foreach (float f in yact.Unks3[i].Data)
                    writer.Write(f);
            }


            long camAnimDataStart = writer.Stream.Position;
            uint[] camAnimLocations = new uint[yact.Cameras.Count];

            for(int i = 0; i < yact.Cameras.Count; i++)
            {
                var anim = yact.Cameras[i];

                camAnimLocations[i] = (uint)writer.Stream.Position;
                writer.WriteTimes(0, 16);

                long fileStart = writer.Stream.Position;
                writer.Write(anim.MTBWFile);

                long end = writer.Stream.Position;
                writer.Stream.Seek(camAnimLocations[i]);
                writer.Write((uint)fileStart);
                writer.Write(anim.MTBWFile.Length);
                writer.Stream.Seek(end);
            }

            long charaAnimDataStart = writer.Stream.Position;
            uint[] charaAnimLocations = new uint[yact.CharacterAnimations.Count];

            for (int i = 0; i < yact.CharacterAnimations.Count; i++)
            {
                var anim = yact.CharacterAnimations[i];

                charaAnimLocations[i] = (uint)writer.Stream.Position;
                writer.WriteTimes(0, 16);

                long fileStart = writer.Stream.Position;
                writer.Write(anim.Buffer);

                long end = writer.Stream.Position;
                writer.Stream.Seek(charaAnimLocations[i]);
                writer.Write((uint)fileStart);
                writer.Write(anim.Buffer.Length);
                writer.Stream.Seek(end);
            }


            writer.Stream.Seek(effectsPointerTableStart);

            foreach (uint i in effectLocations)
                writer.Write(i);

            writer.Stream.Seek(chunk2PointerTableStart);

            foreach (uint i in unk2Locations)
                writer.Write(i);

            writer.Stream.Seek(chunk3PointerTableStart);

            foreach (uint i in unk3Locations)
                writer.Write(i);

            writer.Stream.Seek(cameraPointerTableStart);

            foreach (uint i in camAnimLocations)
                writer.Write(i);

            writer.Stream.Seek(characterPointerTableStart);

            foreach (uint i in charaAnimLocations)
                writer.Write(i);

            //Finish the header
            writer.Stream.Position = headerPos;
            writer.Write((uint)writer.Stream.Length);

            writer.Write((uint)effectsPointerTableStart);
            writer.Write(yact.Effects.Count);

            writer.Write((uint)chunk2PointerTableStart);
            writer.Write(yact.Unks2.Count);

            writer.Write((uint)chunk3PointerTableStart);
            writer.Write(yact.Unks3.Count);

            writer.Write((uint)cameraPointerTableStart);
            writer.Write((uint)yact.Cameras.Count);

            writer.Write((uint)chunk5TableStart);
            writer.Write(0);

            writer.Write((uint)characterPointerTableStart);
            writer.Write((uint)yact.CharacterAnimations.Count);

            writer.Write((uint)chunk7TableStart);
            writer.Write(0);

            //PAD
            writer.Write(0);
            File.WriteAllBytes(path, writer.Stream.ToArray());
        }


    }
}
