using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;

namespace CMNEdit
{
    public static class Utils
    {
        public static float InvariantParse(string val)
        {
            return float.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {

            BitmapData bmpdata = null;

            try
            {
                bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                int numbytes = bmpdata.Stride * bitmap.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr ptr = bmpdata.Scan0;

                Marshal.Copy(ptr, bytedata, 0, numbytes);

                return bytedata;
            }
            finally
            {
                if (bmpdata != null)
                    bitmap.UnlockBits(bmpdata);
            }

        }
    }

    public static class NodeExtensions
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(this Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(this Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }
    }

    public static class ObjectExtensions
    {

        public static List<TreeNode> GetAllNodes(this TreeView _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (TreeNode child in _self.Nodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }

        public static List<TreeNode> GetAllNodes(this TreeNode _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            result.Add(_self);
            foreach (TreeNode child in _self.Nodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }

    }
}