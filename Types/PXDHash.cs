using Yarhl.IO.Serialization.Attributes;

namespace HActLib
{
    [Serializable]
    public class PXDHash
    {
        public ushort Checksum { get; private set; } = 0;
        private string _Text = "";

        [BinaryString(FixedSize = 30, MaxSize = 30)]
        public string Text { get { return _Text.Split(new[] { '\0' }, 2)[0]; } set { Set(value); } }

        public void Set(string text)
        {
            string txt = text.ToLength(30);
            ushort sum = 0;

            foreach (char ch in txt)
                sum += (byte)ch;

            _Text = txt;
            Checksum = sum;
        }

        public static implicit operator string(PXDHash hash)
        {
            return hash.Text;
        }

        public static implicit operator PXDHash(string str)
        {
            var hash = new PXDHash();
            hash.Set(str);

            return hash;
        }
    };
}
