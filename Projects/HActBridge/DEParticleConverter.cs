using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HActLib;
using HActLib.Internal;
using PIBLib;

namespace HActBridge
{
    /// <summary>
    /// Find all pibs used on a DE hact and copy, convert them to OE
    /// </summary>
    public static class DEParticleConverter
    {
        public static bool Convert(Game game, HActInfo infy, string outputDir)
        {
            if (game != Game.Y6)
                return false;

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            DirectoryInfo inf = new DirectoryInfo(infy.MainPath).Parent.Parent;


            if (inf.Parent.Name.StartsWith("hact"))
            {
                DirectoryInfo ptcDir = inf.Parent.Parent.GetDirectories().FirstOrDefault(x => x.Name.StartsWith("particle"));
                FileInfo[] ptcFiles = ptcDir.GetFiles("*.pib", SearchOption.AllDirectories);

                CMN convertedDE = CMN.Read(infy.MainPath, Game.Y6);

                if (ptcDir != null)
                {
                    NodeElement[] ptcs = convertedDE.AllElements.Where(x => x.ElementKind == Reflection.GetElementIDByName("e_auth_element_particle", Game.Y6)).ToArray();
                    HashSet<uint> foundPtcs = new HashSet<uint>();

                    foreach (DEElementParticle ptc in ptcs)
                    {
                        if (foundPtcs.Contains(ptc.ParticleID))
                            continue;

                        string ptcName = ptc.Name.Substring(0, 7);
                        FileInfo ptcFile = ptcFiles.FirstOrDefault(x => x.Name == $"{ptcName}.pib");

                        if (ptcFile != null)
                        {
                            foundPtcs.Add(ptc.ParticleID);
                            try
                            {
                                Pib29 pib = (Pib29)PIB.Read(ptcFile.FullName);
                                PIB.Write(pib.ToV27(), Path.Combine(outputDir, $"{ptcName}.pib"));

                                foreach (BasePibEmitter emitter in pib.Emitters)
                                {
                                    foreach (string texture in emitter.Textures)
                                    {
                                        string ddsPath = TryFetchTexture(ptcFile.Directory.FullName, texture); //Path.Combine(inpDir, texture);

                                        if (!string.IsNullOrEmpty(ddsPath))
                                        {
                                            string ddsName = new FileInfo(ddsPath).Name;
                                            try
                                            {
                                                File.Copy(ddsPath, Path.Combine(outputDir, ddsName), true);
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                Console.WriteLine("PIB Error ", ptcName);
                            }
                        }

                        Console.WriteLine(ptcName);
                    }
                }
            }

            return true;
        }


        public static string TryFetchTexture(string startDir, string textureName)
        {
            DirectoryInfo dirInf = new DirectoryInfo(startDir);
            string path = "";

            if (!textureName.EndsWith(".dds"))
                textureName += ".dds";

            path = Path.Combine(startDir, textureName);

            if (File.Exists(path))
                return path;

            if (dirInf.Name == "pib")
            {
                path = Path.Combine(dirInf.Parent.FullName, "tex", textureName);

                if (File.Exists(path))
                    return path;
            }

            return "";
        }
    }
}
