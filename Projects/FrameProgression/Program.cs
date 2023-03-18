using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using HActLib;

namespace FrameProgression
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            CMN hact = CMN.Read(args[0], Game.YLAD);
            bool clear = args.Length > 1 && (args[1] == "clear" || args[1] == "clean");
            bool add = args.Length > 1 && args[1] == "add";
            bool fuck = args.Length > 1 && args[1] == "fuck";
            bool copy = args.Length > 1 && args[1] == "copy";

            if (hact == null)
                return;


            if(fuck)
            {
                string inp = Console.ReadLine();

                string[] split = inp.Split(' ');

                int start = int.Parse(split[0]);
                int end = int.Parse(split[1]);
                float increment = float.Parse(split[2], System.Globalization.CultureInfo.InvariantCulture);


                foreach (NodeCamera camera in hact.AllCameras)
                {
                    int cur = start;
                    int prev = cur;

                    if (increment == 1)
                        camera.FrameProgression[cur]--;

                    while (cur < end)
                    {
                        float var = camera.FrameProgression[cur];

                        camera.FrameProgression[cur] = camera.FrameProgression[prev] + increment;

                        prev = cur;
                        cur++;
                    }

                }

                CMN.Write(hact, Path.Combine(new FileInfo(args[0]).Directory.FullName, @"cmn_outie.bin"));
                return;
            }

            else if(copy)
            {
                string inp = Console.ReadLine();

                string[] split = inp.Split(' ');

                int start = int.Parse(split[0]);
                int end = int.Parse(split[1]);

                int copyStart = int.Parse(split[2]);

                foreach (NodeCamera camera in hact.AllCameras)
                {
                    for(int i = start; i < end + 1; i++)
                    {
                        float diff = camera.FrameProgression[i + 1] - camera.FrameProgression[i];
                        float frameProg = camera.FrameProgression[copyStart];
                        frameProg += diff;
                        camera.FrameProgression[copyStart + 1] = frameProg;
                        camera.FrameProgressionSpeed[copyStart + 1] = camera.FrameProgressionSpeed[i];
                        copyStart++;
                    }
                }

                CMN.Write(hact, Path.Combine(new FileInfo(args[0]).Directory.FullName, @"cmn_outie.bin"));
                return;
            }


            else if (!clear)
            {
                if (add)
                {
                    foreach (NodeCamera cam in hact.AllCameras)
                    {
                        List<float> progression = cam.FrameProgression.ToList();
                        List<float> speed = cam.FrameProgressionSpeed.ToList();

                        float startPoint = (cam.FrameProgression.Length <= 0 ? 0 : cam.FrameProgression[cam.FrameProgression.Length - 1]);
                        int numToAdd = int.Parse(args[2]);

                        for(int i = 0; i < numToAdd; i++)
                        {
                            startPoint += 1;
                            progression.Add(startPoint);
                            speed.Add(1);
                        }

                        cam.FrameProgression = progression.ToArray();
                        cam.FrameProgressionSpeed = speed.ToArray();
                    }
                }
                else
                {
                    foreach (NodeCamera cam in hact.AllCameras)
                    {
                        cam.FrameProgression = new float[(int)hact.Header.End];
                        cam.FrameProgressionSpeed = new float[(int)hact.Header.End];

                        for (int i = 0; i < cam.FrameProgression.Length; i++)
                            cam.FrameProgression[i] = i;

                        for (int i = 0; i < cam.FrameProgressionSpeed.Length; i++)
                            cam.FrameProgressionSpeed[i] = 1;

                        cam.ProgressionEnd = hact.Header.End + 1;
                    }
                }
            }
            else
            {
                foreach (NodeCamera cam in hact.AllCameras)
                {
                    cam.FrameProgression = new float[0];
                    cam.FrameProgressionSpeed = new float[0];

                    cam.ProgressionEnd = hact.Header.End;
                }
            }


            CMN.Write(hact, Path.Combine(new FileInfo(args[0]).Directory.FullName, @"cmn_outie.bin"));
        }
    }
}
