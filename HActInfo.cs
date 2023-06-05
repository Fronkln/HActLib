using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Yarhl.FileSystem;
using Yarhl.FileFormat;
using ParLibrary;
using ParLibrary.Converter;

namespace HActLib
{
    public struct HActInfo
    {
        public bool IsTEV;

        public string MainPath;
        public string[] ResourcesPaths;

        public Yarhl.FileSystem.Node Par;
        public string ParPath;

        private static string GetCMNPath(string path)
        {
            string def = Path.Combine(path, "cmn");
            string alt2 = Path.Combine(path, "cmn.par.unpack", "cmn");
            string alt1 = Path.Combine(path, "cmn.par.unpack");

            string cmnPath = null;

            if (Directory.Exists(def))
                cmnPath = def;

            else if (Directory.Exists(alt2))
                cmnPath = alt2;

            else if (Directory.Exists(alt1))
                cmnPath = alt1;

            if (Directory.Exists(Path.Combine(cmnPath, "cmn")))
                cmnPath = Path.Combine(cmnPath, "cmn");

            if (cmnPath != null)
                return Path.Combine(cmnPath, "cmn.bin");

            return null;
        }

        private static string GetTEVPath(string path)
        {
            string def = null;

            if (new DirectoryInfo(path).Name == "tmp")
                def = path;
            if(new DirectoryInfo(path).GetDirectories().FirstOrDefault(x => x.Name == "tmp") != null)
            {
                def = Path.Combine(path, "tmp");
            }


            if (Directory.Exists(def))
                return Path.Combine(def, "hact_tev.bin");

            return null;
        }

        private static string TryResPath(string path, string res)
        {
            string def = Path.Combine(path, res);
            string alt2 = Path.Combine(path, $"{res}.par.unpack", res);
            string alt1 = Path.Combine(path, $"{res}.par.unpack");

            string resPath = null;


            if (Directory.Exists(def))
                resPath = def;

            else if (Directory.Exists(alt2))
                resPath = alt2;

            else if (Directory.Exists(alt1))
                resPath = alt1;

            if(!string.IsNullOrEmpty(resPath))
                if(Directory.Exists(Path.Combine(resPath, res)))
                    resPath = Path.Combine(resPath, res);

            if (resPath != null)
                return Path.Combine(resPath, "res.bin");
            else
                return null;
        }

        private static string[] GetRESPath(string path)
        {
            int count = 0;
            string foundPath = null;

            List<string> paths = new List<string>();

            do
            {
                foundPath = TryResPath(path, count.ToString("000"));

                if (!string.IsNullOrEmpty(foundPath))
                    paths.Add(foundPath);

                count++;

            } while (!string.IsNullOrEmpty(foundPath));

            return paths.ToArray();
        }


        public void ProcessPar(string parDir)
        {
            ParPath = parDir;
            Par = NodeFactory.FromFile(ParPath, "par");
            Par.TransformWith<ParArchiveReader, ParArchiveReaderParameters>(new ParArchiveReaderParameters() { Recursive = true });

            Yarhl.FileSystem.Node cmn = Navigator.IterateNodes(Par).FirstOrDefault(x => x.Path.EndsWith("cmn.bin") || x.Path.EndsWith("hact_tev.bin"));

            if (cmn == null)
                return;

            /*
            if (cmn.Path.EndsWith(".par"))
            {
                var cmnFile = cmn.GetFormatAs<ParFile>();

                if (cmnFile.IsCompressed)
                    cmn.TransformWith<ParLibrary.Sllz.Decompressor>();

                var cmnPar = NodeFactory.FromSubstream("cmnpar", cmnFile.Stream, 0, cmnFile.Stream.Length);
                MainPath = "cmn.bin";
            }
            */

            MainPath = cmn.Path;

            //  string[] paths = Navigator.IterateNodes(par).Select(x => x.Path.Replace(@"/par/./", "")).ToArray();
        }

        public byte[] GetCmnBuffer()
        {
            if (string.IsNullOrEmpty(MainPath) || (Par == null && !File.Exists(MainPath)))
                return new byte[0];

            if (Par == null)
                return File.ReadAllBytes(MainPath);

            string cmnPath = MainPath;
            Yarhl.FileSystem.Node cmn = Navigator.IterateNodes(Par).FirstOrDefault(x => x.Path == cmnPath);

            if (cmn != null)
            {
                var cmnFile = cmn.GetFormatAs<ParFile>();

                if (cmnFile.IsCompressed)
                    cmn.TransformWith<ParLibrary.Sllz.Decompressor>();

                byte[] buf = new byte[cmn.Stream.Length];
                cmn.Stream.Read(buf, 0, (int)cmn.Stream.Length);

                return buf;
            }

            return new byte[0];
        }

        public HActInfo(string folder)
        {
            IsTEV = false;
            Par = null;
            ParPath = null;

            if (folder.EndsWith("hact_tev.bin"))
            {
                IsTEV = true;
                MainPath = folder;
                ResourcesPaths = new string[0];
                return;
            }
            else if (folder.EndsWith(".bin"))
            {
                MainPath = folder;
                ResourcesPaths = new string[0];
                return;
            }
            else if (folder.EndsWith(".par"))
            {
                MainPath = "";
                ResourcesPaths = new string[0];

                ProcessPar(folder);
                return;
            }

            if (!Directory.Exists(folder))
                MainPath = "";

            MainPath = GetCMNPath(folder);

            if (string.IsNullOrEmpty(MainPath))
            {
                MainPath = GetTEVPath(folder);

                if (!string.IsNullOrEmpty(MainPath))
                    IsTEV = true;

                ResourcesPaths = new string[0];
            }
            else
            {
                ResourcesPaths = GetRESPath(folder);
            }

        }
    }
}
