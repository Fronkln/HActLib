using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using Yarhl.FileFormat;


namespace HActLib
{
    public class OECMNConverter : IConverter<OECMN, BinaryFormat>
    {
        public BinaryFormat Convert(OECMN cmn)
        {
            var binary = new BinaryFormat();
            var writer = new DataWriter(binary.Stream);
            writer.Endianness = EndiannessMode.BigEndian;

            //we will return to this later
            writer.WriteTimes(0, 48);

            //First write: cut info
            uint cutInfoPointer = (uint)writer.Stream.Position;

            #region Cut Info

            //exclude the unknown 3 frames on length (its how the hact does it)
            writer.Write(cmn.CutInfo.Length);
            writer.WriteTimes(0, 12);

            for (int i = 0; i < cmn.CutInfo.Length; i++)
                writer.Write(cmn.CutInfo[i]);

            #endregion

            //Third write: disable frame info
            uint disableFramePointer = (uint)writer.Stream.Position;

            #region Disable Frame Info

            writer.Write(cmn.DisableFrameInfo.Count);
            writer.WriteTimes(0, 12);

            foreach (DisableFrameInfo inf in cmn.DisableFrameInfo)
            {
                if (cmn.Version > 10)
                    writer.WriteOfType(inf);
                else
                    writer.Write(inf.Start);
            }

            #endregion

            //Fourth write: resource cut info
            uint resourceCutInfo = 0;

            #region Resource Cut Info
            if (cmn.CMNHeader.Version > 10)
            {
                resourceCutInfo = (uint)writer.Stream.Position;

                writer.Write(cmn.ResourceCutInfo.Length);
                writer.WriteTimes(0, 12);

                //start end probably not single
                foreach (float f in cmn.ResourceCutInfo)
                    writer.Write(f);
            }

            #endregion

            uint soundInfo = (uint)writer.Stream.Position;

            #region Sound Info

                writer.Write(cmn.SoundInfo.Length);
                writer.WriteTimes(0, 12);

                foreach (uint i in cmn.SoundInfo)
                    writer.Write(i);

            #endregion

            uint nodeInfoPointer = (uint)writer.Stream.Position;

            #region Node Info

            if (cmn.Root == null)
                throw new Exception("A HAct CMN has to have atleast one root node!");

            cmn.Root.Write(writer, GameVersion.Y0_K1, cmn.CMNHeader.Version);

            #endregion

            cmn.CMNHeader.CutInfoPointer = cutInfoPointer;
            cmn.CMNHeader.DisableFrameInfoPointer = disableFramePointer;
            cmn.CMNHeader.ResourceCutInfoPointer = resourceCutInfo;
            cmn.CMNHeader.SoundInfoPointer = soundInfo;
            cmn.CMNHeader.NodeInfoPointer = nodeInfoPointer;

            writer.Stream.Seek(0, SeekMode.Start);


            writer.Write(cmn.CMNHeader.Version);
            writer.Write(cmn.CMNHeader.Flags);
            writer.Write(cmn.CMNHeader.Start);
            writer.Write(cmn.CMNHeader.End);
            writer.Write(cmn.CMNHeader.NodeDrawNum);

            writer.Write(cutInfoPointer);
            writer.Write(disableFramePointer);

            if (resourceCutInfo > 0)
                writer.Write(resourceCutInfo);

            writer.Write(soundInfo);
            writer.Write(nodeInfoPointer);

            writer.Write(cmn.CMNHeader.ChainCameraIn);
            writer.Write(cmn.CMNHeader.ChainCameraOut);

            return binary;
        }
    }
}
