using HActLib.PS2;
using System;
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

            yact.ReadCharacterAnimations(yactReader, yactHeader.CharacterAnimations);
            yact.ReadCameras(yactReader, yactHeader.CameraAnimations);

            for(int i = 0; i < yact.CharacterAnimations.Count; i++)
            {
                YActCharacter chara = new YActCharacter();
                yact.Characters.Add(chara);
            }

            return yact; 
        }
    }
}
