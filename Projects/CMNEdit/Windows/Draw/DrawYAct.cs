using HActLib;
using HActLib.YAct;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CMNEdit
{
    internal static class DrawYAct
    {
        public static void Draw(TreeNode node)
        {
            Be.Windows.Forms.DynamicByteProvider provider = null;

            if (node is TreeNodeYActY2Animation)
            {
                var y2Anim = node as TreeNodeYActY2Animation;
                PropertyEditorBuilder.CreateHeader($"Animation ({y2Anim.Animation.Format})");
                PropertyEditorBuilder.CreateInput("Start", y2Anim.Animation.Start.ToString(CultureInfo.InvariantCulture), delegate (string val) { y2Anim.Animation.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("End", y2Anim.Animation.End.ToString(CultureInfo.InvariantCulture), delegate (string val) { y2Anim.Animation.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);

                PropertyEditorBuilder.CreateButton("Export", delegate
                {
                    string format = y2Anim.Animation.Format.ToString().ToLowerInvariant();

                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = $"PS2 Animation File (*.{format})|*.{format}";
                    dialog.FileName = $"{y2Anim.Parent.Text}.{format}";
                    dialog.InitialDirectory = AppRegistry.GetFileExtractOpenPath();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        AppRegistry.Root.SetValue("LastHActAssetExportDir", Path.GetDirectoryName(dialog.FileName));
                        File.WriteAllBytes(dialog.FileName, y2Anim.Animation.File.Buffer);
                    }
                });
            }

            if (node is TreeNodeYActCameraMotion)
            {
                PropertyEditorBuilder.CreateHeader("Camera Animation");
                PropertyEditorBuilder.CreateButton("Import", delegate
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "PS2 Object/Camera Animation (*.mtbw)|*.mtbw";

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var animNode = (node as TreeNodeYActCameraMotion);
                        animNode.File.Buffer = File.ReadAllBytes(dialog.FileName);

                        if (animNode.OrigCamera != null)
                            animNode.OrigCamera.MTBWFile = animNode.File.Buffer;
                    }
                });
                PropertyEditorBuilder.CreateButton("Export", delegate
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "PS2 Camera Animation (*.mtbw)|*.mtbw";
                    dialog.FileName = "camera.mtbw";
                    dialog.InitialDirectory = AppRegistry.GetFileExtractOpenPath();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        AppRegistry.Root.SetValue("LastHActAssetExportDir", Path.GetDirectoryName(dialog.FileName));
                        File.WriteAllBytes(dialog.FileName, (node as TreeNodeYActCameraMotion).File.Buffer);
                    }
                });
            }
            else if (node is TreeNodeYActCharacterMotion)
            {
                PropertyEditorBuilder.CreateHeader("Character Animation");
                PropertyEditorBuilder.CreateButton("Import", delegate
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "Character Animation|*.omt;*.dat";

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        (node as TreeNodeYActCharacterMotion).File.Buffer = File.ReadAllBytes(dialog.FileName);
                    }
                });
                PropertyEditorBuilder.CreateButton("Export", delegate
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "PS2 Animation (*.omt)|*.omt";
                    dialog.FileName = node.Parent.Text + ".omt";
                    dialog.InitialDirectory = AppRegistry.GetFileExtractOpenPath();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        AppRegistry.Root.SetValue("LastHActAssetExportDir", Path.GetDirectoryName(dialog.FileName));
                        File.WriteAllBytes(dialog.FileName, (node as TreeNodeYActCharacterMotion).File.Buffer);
                    }
                });
            }
            else if (node is TreeNodeYActEffect)
            {
                var effectNode = node as TreeNodeYActEffect;
                PropertyEditorBuilder.CreateHeader("YAct Effect");

                if (!string.IsNullOrEmpty(effectNode.Effect.Name))
                    PropertyEditorBuilder.CreateInput("Name", effectNode.Effect.Name.ToString(), delegate (string val) { effectNode.Effect.Name = val; });

                PropertyEditorBuilder.CreateInput("Start", effectNode.Effect.Start.ToString(CultureInfo.InvariantCulture), delegate (string val) { effectNode.Effect.Start = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("End", effectNode.Effect.End.ToString(CultureInfo.InvariantCulture), delegate (string val) { effectNode.Effect.End = Utils.InvariantParse(val); }, NumberBox.NumberMode.Float);
                PropertyEditorBuilder.CreateInput("Bone ID", effectNode.Effect.BoneID.ToString(), delegate (string val) { effectNode.Effect.BoneID = int.Parse(val); }, NumberBox.NumberMode.Int);


                switch ((YActEffectType)effectNode.Effect.Type)
                {
                    case YActEffectType.Particle:
                        var ptcEffect = effectNode.Effect as YActEffectParticle;
                        PropertyEditorBuilder.CreateHeader("Particle");
                        PropertyEditorBuilder.CreateInput("Particle ID", ptcEffect.Particle.ToString(), delegate (string val) { ptcEffect.Particle = int.Parse(val); effectNode.Refresh(); }, NumberBox.NumberMode.Int);
                        break;
                    case YActEffectType.Sound:
                        var soundEffect = effectNode.Effect as YActEffectSound;
                        PropertyEditorBuilder.CreateHeader("Sound");
                        PropertyEditorBuilder.CreateInput("Cuesheet ID", soundEffect.CuesheetID.ToString(), delegate (string val) { soundEffect.CuesheetID = ushort.Parse(val); effectNode.Refresh(); }, NumberBox.NumberMode.Ushort);
                        PropertyEditorBuilder.CreateInput("Sound ID", soundEffect.SoundID.ToString(), delegate (string val) { soundEffect.SoundID = ushort.Parse(val); effectNode.Refresh(); }, NumberBox.NumberMode.Ushort);
                        break;
                }


                provider = new Be.Windows.Forms.DynamicByteProvider(effectNode.Effect.UnknownData);
            }

            PropertyEditorBuilder.CreateHeader("");
            Form1.Instance.unkBytesBox.ByteProvider = provider;
            Form1.Instance.varPanel.ResumeLayout();
        }
    }
}
