using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HActLib
{
    public enum Y5NodeIDs : uint
    {
        e_auth_element_particle = 2,
        e_auth_element_particle_opacity = 4,
        e_auth_element_ground_particle = 5,
        e_auth_element_fade = 6,
        e_auth_element_color_correction = 7,
        e_auth_element_blur = 8,
        e_auth_element_gradation = 9,
        e_auth_element_depth_of_field = 10,
        e_auth_element_picture = 11,
        e_auth_element_shadow_display_control = 15,
        e_auth_element_shadow_drawing_area_correction = 16,
        e_auth_element_clip_plane_setting = 18,
        e_auth_element_eye_brightness_correction = 19,
        e_auth_element_wrinkle_blend_ratio = 20,
        e_auth_element_mouth_brightness_correction = 21,
        e_auth_element_face_expression = 22,
        e_auth_element_draw_off = 23,
        e_auth_element_body_flash = 28,
        e_auth_element_blood = 31,
        e_auth_element_damage = 32,
        e_auth_element_sound = 33,
        e_auth_element_hact_input = 34,
        e_auth_element_character_parallel_light_source = 35,
        e_auth_element_hact_end = 37,
        e_auth_element_hact_branching = 38,
        e_auth_element_tone_mapping = 39,
        e_auth_element_stage_parallel_light_source = 41,
        e_auth_element_point_light_source = 43,
        e_auth_element_hact_input_barrage = 44,
        e_auth_element_hact_end_state_change = 48,
        //Matches up to this point on all OE games
        //somewhere between 49-69, they killed off two nodes
        e_auth_element_heat_change = 50,
        e_auth_element_look_to = 62,
        e_auth_element_rim_light = 69,
        e_auth_element_fresnel_coefficient = 71,
        e_auth_element_hact_hide = 72,
        e_auth_element_hact_stop_end = 73,
        e_auth_element_character_parallel_light_source2 = 74,
        e_auth_element_hact_general_purpose_timer = 80,
        e_auth_element_controller_vibration = 82,
        e_auth_element_light_shaft = 87
    }
}
