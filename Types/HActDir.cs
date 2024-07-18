using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ParLibrary;
using ParLibrary.Converter;
using Yarhl.FileSystem;

namespace HActLib
{
    public class HActDir
    {
        public bool IsPar;
        public bool IsFile;

        private string m_path;
        private string m_lang;
        public Yarhl.FileSystem.Node Par;

        ~HActDir()
        {
            if(Par != null)
                Par.Dispose();
        }

        public void Open(string path, string lang = null)
        {
            if (path.EndsWith(".par"))
            {
                IsPar = true;
                Par = NodeFactory.FromFile(path, path);

                Par.TransformWith<ParArchiveReader, ParArchiveReaderParameters>(new ParArchiveReaderParameters() { Recursive = true });
            }
            else if (File.Exists(path))
                IsFile = true;
            else
                IsPar = false;

            if (string.IsNullOrEmpty(lang))
                m_lang = "";
            else
                m_lang = lang;


            m_path = path;
        }



        public HActFile FindFile(string name)
        {
            string parFiltered = name.Replace(".par", "");

            if (IsPar)
            {
                Yarhl.FileSystem.Node file = Navigator.IterateNodes(Par).FirstOrDefault(x => x.Path.EndsWith(name) || x.Path.EndsWith(parFiltered));

                if(file != null)
                    return new HActFile() { ParEntry = file };
            }
            else
            {
                if (IsFile)
                    return new HActFile() { Path = m_path };
                else
                {
                    string file = Directory.GetFiles(m_path, "*", SearchOption.AllDirectories).FirstOrDefault(x => Path.GetFileName(x) == name);

                    if (file != null)
                        return new HActFile() { Path = file };
                }
            }

            return new HActFile();
        }

        public HActFile FindResourceFile()
        {
            string name = "";

            if (string.IsNullOrEmpty(name))
                name = "res.bin";
            else name = $"res_{m_lang}.bin";

            string parFiltered = name.Replace(".par", "");

            if (IsPar)
            {
                Yarhl.FileSystem.Node file = Navigator.IterateNodes(Par).FirstOrDefault(x => x.Path.EndsWith(name) || x.Path.EndsWith(parFiltered));

                if (file != null)
                    return new HActFile() { ParEntry = file };
            }
            else
            {
                if (IsFile)
                    return new HActFile() { Path = m_path };
                else
                {
                    string file = Directory.GetFiles(m_path, "*", SearchOption.AllDirectories).FirstOrDefault(x => Path.GetFileName(x) == name);

                    if (file != null)
                        return new HActFile() { Path = file };
                }
            }

            return new HActFile();
        }


        public HActFile[] FindFilesOfType(string extension)
        {
            List<HActFile> filesArray = new List<HActFile>();

            if(IsPar)
            {
                IEnumerable<Yarhl.FileSystem.Node> files = Navigator.IterateNodes(Par).Where(x => x.Path.EndsWith(extension));

                foreach(Yarhl.FileSystem.Node node in files)
                    filesArray.Add(new HActFile() { ParEntry =node });
            }
            else if(!IsFile)
            {
                if (!Directory.Exists(m_path))
                    return filesArray.ToArray();

                IEnumerable<string> files = Directory.GetFiles(m_path, "*" + extension, SearchOption.AllDirectories);

                foreach (string file in files)
                    filesArray.Add(new HActFile() { Path = file });
            }

            return filesArray.ToArray();
        }

        public byte[] GetCmnBuffer()
        {
            if (IsFile)
                return File.ReadAllBytes(m_path);
            else
            {
                if(!string.IsNullOrEmpty(m_lang))
                    return FindFile($"cmn_{m_lang}.bin").Read();
                else
                    return FindFile($"cmn.bin").Read();
            }
        }

        public byte[] GetFile(string path)
        {
            return null;

          //  if (m_isPar)
             //   return;
        }

        public HActDir GetParticle()
        {
            HActDir ptc = new HActDir();
            ptc.Open(Path.Combine(m_path,"ptc"));
            
            return ptc;
        }

        public HActDir[] GetResources()
        {
            int start = 0;

            List<HActDir> dirs = new List<HActDir>();

            while(true)
            {
                string format = start.ToString("000");

                if(!IsPar)
                {
                    string path = Path.Combine(m_path, format);

                    if (Directory.Exists(path))
                    {
                        HActDir dir = new HActDir() { m_path = path };
                        dirs.Add(dir);
                        start++;
                    }
                    else if(Directory.Exists(path + ".par.unpack"))
                    {
                        HActDir dir = new HActDir() { m_path = path + ".par.unpack" };
                        dirs.Add(dir);
                        start++;
                    }
                    else
                        break;
                }
                else
                    break;
            }

            return dirs.ToArray();
        }
    }
}

public struct HActFile
{
    public string Path;
    public Node ParEntry;

    public byte[] Read()
    {
        if (ParEntry != null)
        {
            var parFile = ParEntry.GetFormatAs<ParFile>();

            if (parFile.IsCompressed)
                ParEntry.TransformWith<ParLibrary.Sllz.Decompressor>();

            byte[] buf = new byte[ParEntry.Stream.Length];
            ParEntry.Stream.Read(buf, 0, (int)ParEntry.Stream.Length);

            return buf;
        }
        else if (File.Exists(Path))
            return File.ReadAllBytes(Path);

        return null;
    }

    public bool Valid()
    {
        return ParEntry != null || (!string.IsNullOrEmpty(Path) && File.Exists(Path));
    }
}