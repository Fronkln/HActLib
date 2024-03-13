using HActLib.PS2;
using HActLib.YAct;
using System;
using System.Collections.Generic;
using System.IO;
using Yarhl.IO;

namespace HActLib
{
    public class BaseYAct
    {
        public List<YActCharacter> Characters = new List<YActCharacter>();
        public List<YActCamera> Cameras = new List<YActCamera>();
        public List<YActEffect> Effects = new List<YActEffect>();

        public List<YActFile> CharacterAnimations = new List<YActFile>();
        public List<YActFile> CameraAnimations = new List<YActFile>();

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

        private static YActVersion DetermineVersion(byte[] buf)
        {
            if (BitConverter.ToInt32(buf, 0) != buf.Length)
                return YActVersion.Y2;
            else
                return YActVersion.Y1;
        }

        protected virtual void ReadCharacters(DataReader yactReader, SizedPointer characterChunk)
        {
        }

        protected virtual void ReadCameras(DataReader yactReader, SizedPointer cameraChunk)
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
