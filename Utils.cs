using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.IO;
using System.IO;

namespace HActLib
{
    public static class Utils
    {
        public static void CopyDir(string sourceDir, string destinationDir)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method

            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDir(subDir.FullName, newDestinationDir);
            }
        }

        public static string ToLength(this string self, int length)
        {
            if (self == null)
                return null;

            if (self.Length == length)
                return self;

            if (self.Length > length)
                return self.Substring(0, length);


            StringBuilder str = new StringBuilder();
            str.Append(self);

            while (str.Length != length)
                str.Append('\0');

            return str.ToString();
        }
        public static void Write(this DataWriter writer, Matrix4x4 Matrix)
        {
            Matrix.Write(writer);
        }

        public static IEnumerable<Enum> GetUniqueFlags(this Enum flags)
        {
            ulong flag = 1;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && flags.HasFlag(value))
                {
                    yield return value;
                }
            }
        }

        public static byte[] ConvertTo256PointCurve(byte[] originalCurve)
        {
            int originalCount = originalCurve.Length;
            int expandedCount = 256;
            byte[] expandedCurve = new byte[expandedCount];

            for (int i = 0; i < expandedCount - 1; i++)
            {
                // Find the corresponding position in the original curve
                double ratio = (double)i / (expandedCount - 1) * (originalCount - 1);
                int index1 = (int)Math.Floor(ratio);   // Index of the first original point
                int index2 = (int)Math.Ceiling(ratio); // Index of the second original point

                // If the indices are the same, just copy the value
                if (index1 == index2)
                {
                    expandedCurve[i] = originalCurve[index1];
                }
                else
                {
                    // Perform linear interpolation
                    double fraction = ratio - index1;
                    int interpolatedValue = (int)(originalCurve[index1] + fraction * (originalCurve[index2] - originalCurve[index1]));

                    // Ensure the result is within the byte range (0-255)
                    expandedCurve[i] = (byte)Math.Clamp(interpolatedValue, 0, 255);
                }
            }

            // Assign the last point to the last original curve value
            expandedCurve[expandedCount - 1] = originalCurve[originalCount - 1];

            return expandedCurve;
        }

        internal static IEnumerable<Enum> GetFlags(this Enum e)
        {
            return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag);
        }

    }
}
