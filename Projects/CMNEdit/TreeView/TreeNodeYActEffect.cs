using System;
using System.Windows.Forms;
using HActLib;
using HActLib.YAct;

namespace CMNEdit
{
    internal class TreeNodeYActEffect : TreeNode
    {
        public YActEffect Effect;

        public TreeNodeYActEffect()
        {

        }

        public TreeNodeYActEffect(YActEffect effect)
        {
            Effect = effect;
            Refresh();

            ImageIndex = SetIcon(effect.Type);
            SelectedImageIndex = ImageIndex;
        }

        public void Refresh()
        {
            var effect = Effect;

            switch ((YActEffectType)effect.Type)
            {
                default:
                    Text = ((YActEffectType)effect.Type).ToString();
                    break;
                case YActEffectType.HActEvent:
                    if (string.IsNullOrEmpty(effect.Name))
                        Text = $"EFFECT";
                    else
                        Text = effect.Name;
                    break;
                case YActEffectType.CameraEffect:
                    Text = "Camera Effect";
                    break;
                case YActEffectType.Sound:
                    var yactSound = effect as YActEffectSound;

                    Text = "Sound (" + yactSound.CuesheetID + " - " + yactSound.SoundID + ")";
                    break;
                case YActEffectType.Particle:
                    var yactPtc = effect as YActEffectParticle;
                    if (Form1.curGame == Game.Y1)
                        Text = "Particle (" + (ParticleIDY1)yactPtc.Particle + ")";
                    else
                        Text = "Particle (" + yactPtc.Particle + ")";
                    break;

            }
        }

        public static int SetIcon(int type)
        {
            switch((YActEffectType)type)
            {
                case YActEffectType.Sound:
                    return 2;
                case YActEffectType.Particle:
                    return 7;
            }

            return 1;
        }
    }
}
