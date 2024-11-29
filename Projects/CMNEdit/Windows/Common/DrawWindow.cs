using CMNEdit.Windows.Common.DE;
using CMNEdit.Windows;
using HActLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNEdit
{
    internal static class DrawWindow
    {
        public static void Draw(NodeElement element)
        {
            //use curver to display things accordingly.

            string elemName = HActLib.Internal.Reflection.GetElementNameByID(element.ElementKind, Form1.curGame);

            if (Form1.curVer == GameVersion.Y0_K1)
            {
                switch (elemName)
                {
                    case "e_auth_element_particle":
                        OEParticleWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_damage":
                        OEDamageWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_heat_change":
                        OEHeatChangeWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_fade":
                        OEFadeWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_sound":
                        OENodeElementSEWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_hact_input":
                        OEHActInputWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_hact_input_barrage":
                        OEHActInputBarrageWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_face_expression":
                        OEFaceExpressionWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_rim_light_scale":
                        OERimlightScaleWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_picture":
                        OEPictureWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_noise":
                        OENoiseWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_subtitles":
                        OESubtitleWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_person_caption":
                        OEPersonCaptionWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_hact_branching":
                        OEHActBranchWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_hact_end":
                        OEHActEndWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_hact_stop_end":
                        OEHActStopEndWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_gradation":
                        OEGradationWindow.Draw(Form1.Instance, element);
                        break;
                }
            }
            else if (CMN.IsDE(Form1.curVer))
            {
                switch (element.ElementKind)
                {
                    //AuthExtended: System Speed
                    case 1337:
                        DECustomElementSystemSpeedWindow.Draw(Form1.Instance, element);
                        break;
                    //Like a Brawler: Transit HAct
                    case 60010:
                        DECustomElementY7BTransitEXFollowupWindow.Draw(Form1.Instance, element);
                        break;
                    //Like a Brawler 8: Character Selection
                    case 70010:
                        DECustomElementLAB8CharacterSelectionWindow.Draw(Form1.Instance, element);
                        break;
                }

                switch (elemName)
                {
                    case "e_auth_element_camera_param":
                        DEElementCameraParamWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_apply_ubik":
                        DEElementUBikApplyWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_se":
                        DENodeElementSEWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_speech":
                        DEElementSpeechWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_ui_fade":
                        DEElementUIFadeWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_ui_texture":
                        DEElementUITextureWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_particle":
                        DEElementParticleWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_hact_input":
                        DEHActInputWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_hact_input_barrage":
                        DEHActInputBarrageWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_battle_damage":
                        DENodeBattleDamageWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_battle_heat":
                        DEElementBattleHeatWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_equip_asset_hide":
                        DEElementHideAssetWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_battle_followup_window":
                        DEEelementFollowupWindowWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_color_correction":
                        DEElementColorCorrectionWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_color_correction_v2":
                        DEElementColorCorrection2Window.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_flow_dust_gen":
                        DEElementFlowdustWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_rim_flash":
                        DEElementRimflashWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_color_correction_mask":
                        DEElementColorCorrectionMaskWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_battle_muteki":
                        DEElementTimingInfoWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_battle_attack":
                        DEElementBattleAttackWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_connect_chara_out":
                        DEElementCharaOutWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_stage_warp":
                        DEElementStageWarpWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_face_anim":
                        DEElementFaceAnimWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_expression_target":
                        DEElementExpressionTargetWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_grain_noise":
                        DEElementGrainNoiseWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_character_node_scale":
                        DEElementCharacterNodeScaleWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_movie":
                        DEElementMovieWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_post_effect_gradation":
                        DEElementGradationWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_post_effect_dof":
                        DEElementDOFWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_post_effect_dof2":
                        DEElementDOF2Window.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_post_effect_motion_blur":
                        DEElementMotionBlurWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_post_effect_motion_blur2":
                        DEElementMotionBlur2Window.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_particle_ground":
                        DEElementParticleGroundWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_fullscreen_auth_movie":
                        DEElementFullscreenAuthMovieWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_equip_asset":
                        DEElementAssetEquipWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_speed_control":
                        DEElementSpeedControlWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_character_speed":
                        DEElementCharacterSpeedWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_div_play":
                        DEElementDivPlayWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_character_change":
                        DEElementCharacterChangeWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_asset_break_uid":
                        DEElementAssetBreakUIDWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_connect_camera":
                        DEElementCameraLinkWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_path_offset":
                        Form1.Instance.CreateHeader("Path Offset");
                        MatrixWindow.Draw(Form1.Instance, (element as DEElementPathOffset).Matrix);
                        break;
                    case "e_auth_element_scenario_timeline":
                        DEElementScenarioTimelineWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_camera_shake":
                        DEEElementCameraShakeWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_battle_command_special":
                        DEElementBattleCommandSpecialWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_battle_transit_stun":
                        DEElementTimingInfoStunWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_asset_arms_reduce_use_count":
                        DEElementArmsReduceAssetCountWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_talk_text":
                        DEElementTalkTextWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_battle_slide":
                        DEElementBattleSlideWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_spot_light":
                        DEElementLightSpotWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_point_light":
                        DEElementLightPointWindow.Draw(Form1.Instance, element);
                        break;
                    case "e_auth_element_chromatic_aberration":
                        DEElementChromaticAberrationWindow.Draw(Form1.Instance, element);
                        break;
                }
            }
        }
    }
}
