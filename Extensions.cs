﻿using System;
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

        public static Vector2 ReadVector2(this DataReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        public static Vector3 ReadVector3(this DataReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static Vector4 ReadVector4(this DataReader reader)
        {
            return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static RGB ReadRGB(this DataReader reader)
        {
            return new RGB(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public static RGB32 ReadRGB32(this DataReader reader)
        {
            return new RGB32(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32());
        }

        public static RGBA32 ReadRGBA32(this DataReader reader)
        {
            return new RGBA32(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32());
        }

        public static Matrix4x4 ReadMatrix4x4(this DataReader reader)
        {
            Matrix4x4 mtx = new Matrix4x4();
            mtx.VM0 = reader.ReadVector4();
            mtx.VM1 = reader.ReadVector4();
            mtx.VM2 = reader.ReadVector4();
            mtx.VM3 = reader.ReadVector4();

            return mtx;
        }

        public static string ReadStringPointer(this DataReader reader, int addr)
        {
            if (addr <= 0)
                return null;
            else
            {
                string str = null;
                reader.Stream.RunInPosition(delegate { str = reader.ReadString(); }, addr, SeekMode.Start);

                return str;
            }
        }

        public static void Write(this DataWriter writer, Vector2 vec)
        {
            writer.Write(vec.x);
            writer.Write(vec.y);
        }

        public static void Write(this DataWriter writer, Vector3 vec)
        {
            writer.Write(vec.x);
            writer.Write(vec.y);
            writer.Write(vec.z);
        }
        public static void Write(this DataWriter writer, Vector4 vec)
        {
            writer.Write(vec.x);
            writer.Write(vec.y);
            writer.Write(vec.z);
            writer.Write(vec.w);
        }
        public static void Write(this DataWriter writer, RGB col)
        {
            writer.Write(col.R);
            writer.Write(col.G);
            writer.Write(col.B);
        }


        public static void Write(this DataWriter writer, RGB32 col)
        {
            writer.Write(col.R);
            writer.Write(col.G);
            writer.Write(col.B);
        }


        public static void Write(this DataWriter writer, RGBA32 col)
        {
            writer.Write(col.R);
            writer.Write(col.G);
            writer.Write(col.B);
            writer.Write(col.A);
        }

        public static void Write(this DataWriter writer, RGBA col)
        {
            writer.Write(col.r);
            writer.Write(col.g);
            writer.Write(col.b);
            writer.Write(col.a);
        }

        public static byte[] ToArray(this DataStream stream)
        {
            long pos = stream.Position;
            stream.Position = 0;

            byte[] buf = new byte[stream.Length];
            stream.Read(buf, 0, buf.Length);

            stream.Position = pos;

            return buf;
        }


    }
}

namespace System.Reflection
{
    internal static class ReflectionExtensions
    {
        //make this better, too much argumentexception
        public static void CopyFields(this object source, Object destination)
        {
            // copy base class properties.

            foreach (FieldInfo prop in source.GetType().GetFields())
            {
                try
                {
                    FieldInfo prop2 = source.GetType().GetField(prop.Name);
                    prop2.SetValue(destination, prop.GetValue(source));
                }
                catch { continue; }
            }
        }

        public static void CopyProperties(this object source, Object destination)
        {
            // copy base class properties.

            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                try
                {
                    PropertyInfo prop2 = source.GetType().GetProperty(prop.Name);
                    prop2.SetValue(destination, prop.GetValue(source));
                }
                catch { continue; }
            }
        }
    }
}