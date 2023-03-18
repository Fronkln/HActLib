using System;
using System.Linq;
using System.Collections.Generic;
using HActLib;
using CMNEdit.Windows;

namespace CMNEdit
{
    internal static class DEElementExpressionTargetWindow
    {
        public static void Draw(Form1 form, Node node)
        {
            DEElementExpressionTarget expTarget = node as DEElementExpressionTarget;

            form.CreateHeader("Random Settings");

            form.CreateButton("Add New",
                delegate
                {
                    ExpressionTargetData targetDat = new ExpressionTargetData();
                    targetDat.Animation = Enumerable.Repeat<byte>(0, 256).ToArray();

                    expTarget.Data.Add(targetDat);
                }
            );

            form.CreateButton("Set All to 0",
                delegate
                {
                    foreach (ExpressionTargetData dat in expTarget.Data)
                        for (int i = 0; i < dat.Animation.Length; i++)
                            dat.Animation[i] = 0;
                }
            );

            form.CreateButton("Set All to 0.5",
                delegate
                {
                    foreach (ExpressionTargetData dat in expTarget.Data)
                        for (int i = 0; i < dat.Animation.Length; i++)
                            dat.Animation[i] = 127;
                }
            );

            form.CreateButton("Set All to 1",
                delegate
                {
                    foreach (ExpressionTargetData dat in expTarget.Data)
                        for (int i = 0; i < dat.Animation.Length; i++)
                            dat.Animation[i] = 255;
                }
            );



            form.CreateButton("Lerp All to 1",
                delegate
                {
                    foreach (ExpressionTargetData dat in expTarget.Data)
                    {
                        for (int i = 0; i < dat.Animation.Length; i++)
                            dat.Animation[i] = (byte)i;
                    }
                }
            );


            form.CreateButton("RANDOMIZE EVERYTHING!!!",
                delegate
                {
                    Random rnd = new Random();

                    foreach (ExpressionTargetData dat in expTarget.Data)
                    {
                        for (int i = 0; i < dat.Animation.Length; i++)
                            dat.Animation[i] = (byte)rnd.Next(0, 256);
                    }
                }
            );
        }
    }
}
