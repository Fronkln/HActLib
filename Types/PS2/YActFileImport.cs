using Yarhl.IO;

namespace HActLib.YAct
{
    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class YActFileImport
    {
        public int Pointer { get; set; }
        public int Size { get; set; }

        public long Padding { get; set; }

        internal YActFile ReadFile(DataReader reader)
        {
            YActFile file = null;

            reader.Stream.RunInPosition(delegate 
            {
                file = new YActFile() { Buffer = reader.ReadBytes(Size) };
            }, Pointer);

            return file;
        }
    }
}
