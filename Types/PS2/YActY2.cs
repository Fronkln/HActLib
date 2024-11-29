using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Yarhl.IO;
using System.Security.Cryptography;

namespace HActLib.YAct
{
    public class YActY2 : BaseYAct
    {
        internal new static YActY2 Read(byte[] buffer)
        {
            DataStream readStream = DataStreamFactory.FromArray(buffer, 0, buffer.Length);
            DataReader yactReader = new DataReader(readStream) { Endianness = EndiannessMode.LittleEndian, DefaultEncoding = System.Text.Encoding.GetEncoding(932) };

            yactReader.Stream.Position += 32;

            YActY2 yact = new YActY2();
            YActHeaderY2 yactHeader = yactReader.Read<YActHeaderY2>();

            yact.ReadCharacterAnimations(yactReader, yactHeader.CharacterAnimations);
            yact.ReadCameraAnimations(yactReader, yactHeader.CameraAnimations);

            yact.ReadCharacters(yactReader, yactHeader.Character, yact.CharacterAnimations);
            yact.ReadCameras(yactReader, yactHeader.Camera, yact.CameraAnimations);
            yact.ReadEffects(yactReader, yactHeader.Effect);

            return yact;
        }

        protected override void ReadCameras(DataReader yactReader, SizedPointer cameraChunk, List<YActFile> cameraFiles)
        {
            Cameras = new List<YActCamera>();

            if (cameraChunk.Size <= 0)
                return;

            yactReader.Stream.Seek(cameraChunk.Pointer);

            for (int i = 0; i < cameraChunk.Size; i++)
            {
                YActY2Camera cam = new YActY2Camera();
                cam.Unk1Y2 = yactReader.ReadInt32();
                cam.Unk2Y2 = yactReader.ReadInt32();
                cam.Unk3Y2 = yactReader.ReadInt32();

                SizedPointer animPtr = yactReader.Read<SizedPointer>();

                cam.Unk4Y2 = yactReader.ReadBytes(12);

                if (animPtr.Size > 0)
                {
                    yactReader.Stream.RunInPosition(delegate
                    {
                        for (int i = 0; i < animPtr.Size; i++)
                        {
                            YActY2AnimationData animationData = new YActY2AnimationData();
                            animationData.Read(yactReader);
                            animationData.File = cameraFiles[animationData.AnimationID];
                            animationData.Format = PS2.OGREAnimationFormat.MTBW;

                            cam.AnimationData.Add(animationData);
                        }
                    }, animPtr.Pointer, SeekMode.Start);
                }

                Cameras.Add(cam);
            }
        }

        protected override void ReadCharacters(DataReader yactReader, SizedPointer characterChunk, List<YActFile> characterFiles)
        {
            Characters = new List<YActCharacter>();

            if (characterChunk.Size <= 0)
                return;

            yactReader.Stream.Seek(characterChunk.Pointer);

            for(int i = 0; i < characterChunk.Size; i++)
            {
                YActY2Character character = new YActY2Character();
                character.Name = yactReader.ReadStringPointer(yactReader.ReadInt32());

                SizedPointer animPtr = yactReader.Read<SizedPointer>();

                character.UnknownY2 = new int[2];
                character.UnknownY2[0] = yactReader.ReadInt32();
                character.UnknownY2[1] = yactReader.ReadInt32();
                
                if(animPtr.Size > 0)
                {
                    yactReader.Stream.RunInPosition(delegate
                    {
                        for(int i = 0; i < animPtr.Size; i++)
                        {
                            YActY2AnimationData animationData = new YActY2AnimationData();
                            animationData.Read(yactReader);
                            character.AnimationData.Add(animationData);
                            animationData.File = characterFiles[animationData.AnimationID];
                            animationData.Format = PS2.OGREAnimationFormat.OMT;
                        }
                    }, animPtr.Pointer, SeekMode.Start);
                }

                Characters.Add(character);
            }
        }

        protected override void ReadEffects(DataReader yactReader, SizedPointer effectChunk)
        {
            Effects = new List<YActEffect>();

            if (effectChunk.Size <= 0)
                return;

            yactReader.Stream.Seek(effectChunk.Pointer);

            for(int i = 0; i < effectChunk.Size; i++)
            {

                int namePtr = yactReader.ReadInt32();
                int unk = yactReader.ReadInt32();
                int unknown1 = yactReader.ReadInt32();
                int dataPointer = yactReader.ReadInt32();


                YActEffect effect = null;

                //effect.Name = yactReader.ReadStringPointer(yactReader.ReadInt32());
               // int unk = yactReader.ReadInt32();
                //effect.Unknown1 = yactReader.ReadStringPointer(yactReader.ReadInt32());

                yactReader.Stream.RunInPosition(
                    delegate 
                    {
                        int type = 0;
                        yactReader.Stream.RunInPosition(delegate { type = yactReader.ReadInt32(); }, 0x2C, SeekMode.Current);

                        switch ((YActEffectType)type)
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
                    }, dataPointer);

                effect.Name = yactReader.ReadStringPointer(namePtr);
                effect.Unknown1 = yactReader.ReadStringPointer(unknown1);

                Effects.Add(effect);
            }
        }
    }
}
