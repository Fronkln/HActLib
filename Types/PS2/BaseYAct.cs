using System;
using System.Collections.Generic;
using System.IO;
using Yarhl.IO;

namespace HActLib.YAct
{
    public class BaseYAct
    {
        public List<YActCharacter> Characters = new List<YActCharacter>();
        public List<YActCamera> Cameras = new List<YActCamera>();
        public List<YActEffect> Effects = new List<YActEffect>();

        public List<YActFile> CharacterAnimations = new List<YActFile>();
        public List<YActFile> CameraAnimations = new List<YActFile>();

        public List<YActUnkStructure2> Unks2 = new List<YActUnkStructure2>();
        public List<YActUnkStructure2> Unks3 = new List<YActUnkStructure2>();

        public static BaseYAct Read(string path)
        {
            if (!File.Exists(path))
                return null;

            return Read(File.ReadAllBytes(path));
        }

        public static BaseYAct Read(byte[] buf)
        {
            YActVersion version = DetermineVersion(buf);

            if (version == YActVersion.Y2)
                return YActY2.Read(buf);
            else
                return YActY1.Read(buf);
        }

        public static void Write(string path, BaseYAct yact)
        {

        }

        private static YActVersion DetermineVersion(byte[] buf)
        {
            if (BitConverter.ToInt32(buf, 0) != buf.Length)
                return YActVersion.Y2;
            else
                return YActVersion.Y1;
        }

        protected virtual void ReadCharacters(DataReader yactReader, SizedPointer characterChunk, List<YActFile> characterFiles)
        {
        }

        protected virtual void ReadCameras(DataReader yactReader, SizedPointer cameraChunk, List<YActFile> cameraFiles)
        {
            Cameras = new List<YActCamera>();

            if (cameraChunk.Size <= 0)
                return;

            yactReader.Stream.Seek(cameraChunk.Pointer);

            for (int i = 0; i < cameraChunk.Size; i++)
            {
                YActCamera cam = new YActCamera();

                int fileImportPtr = yactReader.ReadInt32();
                YActFileImport fileImport = null;

                yactReader.Stream.RunInPosition(delegate { fileImport = yactReader.Read<YActFileImport>(); }, fileImportPtr);
                yactReader.Stream.RunInPosition(delegate { cam.MTBWFile = yactReader.ReadBytes(fileImport.Size); }, fileImport.Pointer);

                Cameras.Add(cam);
            }
        }

        protected virtual void ReadEffects(DataReader yactReader, SizedPointer effectChunk)
        {

        }

        protected virtual void ReadUnk2s(DataReader yactReader, SizedPointer unk2Chunk)
        {
            if (unk2Chunk.Size <= 0)
                return;

            Unks2 = new List<YActUnkStructure2>();

            yactReader.Stream.Seek(unk2Chunk.Pointer);

            for(int i = 0; i < unk2Chunk.Size; i++)
            {
                YActUnkStructure2 str2 = new YActUnkStructure2();
                str2.Data = new float[8]
                {
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                };

                Unks2.Add(str2);
            }
        }

        protected virtual void ReadUnk3s(DataReader yactReader, SizedPointer unk3Chunk)
        {
            if (unk3Chunk.Size <= 0)
                return;

            Unks3 = new List<YActUnkStructure2>();

            yactReader.Stream.Seek(unk3Chunk.Pointer);

            for (int i = 0; i < unk3Chunk.Size; i++)
            {
                YActUnkStructure2 str3 = new YActUnkStructure2();
                str3.Data = new float[8]
                {
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                    yactReader.ReadSingle(),
                };

                Unks3.Add(str3);
            }
        }

        protected virtual void ReadCharacterAnimations(DataReader yactReader, SizedPointer charaAnimChunk)
        {
            CharacterAnimations = new List<YActFile>();

            if (charaAnimChunk.Size <= 0)
                return;

            yactReader.Stream.Seek(charaAnimChunk.Pointer);

            for (int i = 0; i < charaAnimChunk.Size; i++)
            {
                int fileImportPtr = yactReader.ReadInt32();

                yactReader.Stream.RunInPosition(delegate
                {
                    CharacterAnimations.Add(yactReader.Read<YActFileImport>().ReadFile(yactReader));
                }, fileImportPtr);
            }
        }

        protected virtual void ReadCameraAnimations(DataReader yactReader, SizedPointer camAnimChunk)
        {
            CameraAnimations = new List<YActFile>();

            if (camAnimChunk.Size <= 0)
                return;

            yactReader.Stream.Seek(camAnimChunk.Pointer);

            for (int i = 0; i < camAnimChunk.Size; i++)
            {
                int fileImportPtr = yactReader.ReadInt32();

                yactReader.Stream.RunInPosition(delegate
                {
                    CameraAnimations.Add(yactReader.Read<YActFileImport>().ReadFile(yactReader));
                }, fileImportPtr);
            }
        }
    }
}
