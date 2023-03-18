using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Yarhl.IO;


namespace HActLib
{
    public static class Extensions
    {
        //https://stackoverflow.com/a/45034630/14569631
        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }

        public static T Closest<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, TKey pivot) where TKey : IComparable<TKey>
        {
            return source.Where(x => pivot.CompareTo(keySelector(x)) <= 0).OrderBy(keySelector).FirstOrDefault();
        }

        public static int Align(this DataWriter writer, int alignment)
        {
            int mod = (int)writer.Stream.Position % alignment;

            if (mod == 0)
                return 0;

            int neededBytes = alignment - mod;

            writer.WriteTimes(0, neededBytes);
            return neededBytes;
        }

        public static Vector3 ReadVector3(this DataReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static RGBA32 ReadRGBA32(this DataReader reader)
        {
            return new RGBA32(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32());
        }

        public static void Write(this DataWriter writer, Vector3 vec)
        {
            writer.Write(vec.x);
            writer.Write(vec.y);
            writer.Write(vec.z);
        }

        public static void Write(this DataWriter writer, RGBA32 col)
        {
            writer.Write(col.R);
            writer.Write(col.G);
            writer.Write(col.B);
            writer.Write(col.A);
        }
    }
}
