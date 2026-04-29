using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    public static class DBBinaryString
    {
        private const string MAP = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string Encode(byte[] input)
        {
            var sb = new StringBuilder();
            sb.Append('1');

            for (int i = 0; i < input.Length; i += 8)
            {
                ulong v2 = ((ulong)BitConverter.ToUInt32(input, i) << 32) | BitConverter.ToUInt32(input, i + 4);
                char[] chunk = new char[11];
                for (int j = 0; j < 11; j++)
                {
                    chunk[j] = MAP[(int)(v2 % 62)];
                    v2 /= 62;
                }
                sb.Append(chunk);
            }

            return sb.ToString();
        }

        public static byte[] Decode(string input)
        {
            string ptr = input.Substring(1);
            var bin = new List<byte>();
            int off = 0, remaining = ptr.Length;

            while (remaining >= 11)
            {
                string chunk = ptr.Substring(off, 11);
                ulong v2 = 0;
                for (int i = 10; i >= 0; i--)
                    v2 = GetCharVal(chunk[i]) + 62UL * v2;

                bin.AddRange(BitConverter.GetBytes((uint)(v2 >> 32)));
                bin.AddRange(BitConverter.GetBytes((uint)(v2 & 0xFFFFFFFF)));
                off += 11; remaining -= 11;
            }
            

            return bin.ToArray();
        }

        private static byte GetCharVal(char c)
        {
            if (c >= '0' && c <= '9') return (byte)(c - '0');
            if (c >= 'A' && c <= 'Z') return (byte)(c - 'A' + 10);
            if (c >= 'a' && c <= 'z') return (byte)(c - 'a' + 36);
            return 0;
        }
    }
}
