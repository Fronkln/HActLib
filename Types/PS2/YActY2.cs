using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Yarhl.IO;
using System.Security.Cryptography;
using HActLib.PS2;

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

            yact.ReadCharacters(yactReader, yactHeader.Character);
            yact.ReadCameras(yactReader, yactHeader.Camera);
            yact.ReadEffects(yactReader, yactHeader.Effect);

            yact.ReadCharacterAnimations(yactReader, yactHeader.CharacterAnimations);
            yact.ReadCameraAnimations(yactReader, yactHeader.CameraAnimations);

            return yact;
        }

        protected override void ReadCharacters(DataReader yactReader, SizedPointer characterChunk)
        {
            Characters = new List<YActCharacter>();

            if (characterChunk.Size <= 0)
                return;

            yactReader.Stream.Seek(characterChunk.Pointer);

            for(int i = 0; i < characterChunk.Size; i++)
            {
                YActCharacter character = new YActCharacter();
                character.Name = yactReader.ReadStringPointer(yactReader.ReadInt32());

                yactReader.Stream.Position += 16; //Unknown data i dont care about yet

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
                YActEffect effect = new YActEffect();

                effect.Name = yactReader.ReadStringPointer(yactReader.ReadInt32());
                int unk = yactReader.ReadInt32();
                effect.Unknown1 = yactReader.ReadStringPointer(yactReader.ReadInt32());

                int dataPointer = yactReader.ReadInt32();

                yactReader.Stream.RunInPosition(delegate { effect.ReadData(yactReader); }, dataPointer);

                Effects.Add(effect);
            }
        }
    }
}
