using UnityEngine;
using System.Collections;

public static class PathHelper
{
    public const string ASSET_RESOURCES_PATH = "Assets/GameResources/";

	// Resources Path
    public const string RESOURCES_PATH = "GameResources/";//
	
	// ArtResources path
	public const string ARTRESOURCES_PATH = "ArtResources/";
	
    //Local Asset Path
    public const string LOCALASSETPATH = "LocalAssetPath/";

	// Battle Config Path
	public const string BATTLE_CONFIG_PATH = "ConfigFiles/";
	
	public const string MAP_DATA_PATH = "ConfigFiles/MapData/";

    public const string SCENE_DATA_PATH = "ConfigFiles/SceneData/";
	
	public const string SCENE_UNITY_PATH = "ArtResources/Models/Scene/Other/Scene/";

	// SHADOW_PREFAB_PATH
	public const string SHADOW_PREFAB_PATH = "ArtResources/Characters/Other/Shadow/Prefabs/Shadow";

	// Pet Model Prefab
	public const string PET_PREFAB_PATH = "ArtResources/Characters/Pet/{0}/Prefabs/{0}";

	//Weapon Model Prefab
	public const string WEAPON_PREFAB_PATH = "ArtResources/Characters/Weapon/{0}/Prefabs/{0}";
	
	// Effects Path
	public const string SHADOW_MOTION_PATH = "Prefabs/Effect/ShadowMotionPrefab";
	// common effect path/Users/dustonlaw/TSJJ/xy_unity (trunk)/Assets/GameResources/ArtResources/Prefabs/Effect/Common/Eff_game
	public const string COMMON_EFFECT_PATH = "Prefabs/Effect/Common/Eff_game/";

	// Hero Model Prefab
	public const string HERO_PREFAB_PATH = "ArtResources/Prefabs/Model/Characters/Hero/";	

    // Npc Model Prefab
	public const string NPC_PREFAB_PATH = "ArtResources/Characters/Npc/{0}/Prefabs/{0}";

	// Map Model Prefab
	public const string MAP_PREFAB_PATH = "ArtRes/Map/MapPrefabs/";
	
	// Temp Model Prefab
	public const string TEMP_PREFAB_PATH = "Prefabs/Temporary/";
	
	// Story Conifig Data
	public const string STORY_DATA_PATH  = "ConfigFiles/Story/";
	
	//CAMERA ANIMATION
	public const string CAMERA_ANIMATION_PATH  = "ArtRes/CameraAnimation/";

	//SKILL EFFECT
	public const string OLD_SKILL_EFFECT_PATH  = "ArtRes/effect/skill/skill_prefab/";

    //SKILL EFFECT
	public const string SKILL_EFFECT_PATH = "ArtResources/Effects/Skill/{0}/Prefabs/{0}";

	//BUFF EFFECT
	public const string BUFF_EFFECT_PATH = "ArtResources/Effects/Buff/{0}/Prefabs/{0}";

	//SKILL DUMMY EFFECT
	public const string SKILL_DUMMY_EFFECT_PATH = "ArtResources/Effects/Common/{0}/Prefabs/{0}";	
	
	//SKILL SCENE EFFECT
	public const string SCENE_EFFECT_PATH = "ArtResources/Effects/Scene/{0}/Prefabs/{0}";

	//SKILL GAME EFFECT
	public const string GAME_EFFECT_PATH = "ArtResources/Effects/Game/{0}/Prefabs/{0}";
	
	//Static Data
	public const string STATIC_DATA_PATH  = "ConfigFiles/StaticData/";
	
	//Pet Matrials Path
	public const string PET_MATERIALS_PATH  = "ArtResources/Characters/Pet/{0}/Materials/{1}";
	
	//NpcCommon Matrials Path
	public const string NPC_COMMON_MATERIALS_PATH  = "ArtResources/Models/Characters/NPC/Common/Materials/";

	//NpcPlot Matrials Path
	public const string NPC_PLOT_MATERIALS_PATH  = "ArtResources/Models/Characters/NPC/Plot/Materials/";	
	
    //Audio 
    public const string AUDIO_MUSIC_PATH = "ArtResources/Audios/Music/";
    public const string AUDIO_SOUND_PATH = "ArtResources/Audios/Sound/";
	
	//LightMap
	public const string LIGHTMAP_PATH    = "ArtResources/LightMap/";
	
	//Images
	public const string IMAGES_PATH = "ArtResources/Images/";

    //Resource Setting Path
    public const string SETTING_PATH = "Setting/";
	
	//Resource atlas icon path
	public const string ATLAS_ICON_PATH = "Atlas/icon/";
    //Resource atlas pet path
    public const string ATLAS_PET_FACE_PATH = "Atlas/pet/";

    //Animation path
    public const string ANIMATION_CLIP_PATH = "ArtResources/Animation/Model/";
	
	//Player Cloth
	public const string PLAYER_CLOTH_PATH 	= "ArtResources/Models/Charactor/Hero/";

	public static string GetEffectPath(string effectName)
	{
		if (effectName.IndexOf("game_eff_") != -1)
		{
			return string.Format(GAME_EFFECT_PATH, effectName);
		}
		else if (effectName.IndexOf("skill_eff_") != -1)
		{
			return string.Format(SKILL_EFFECT_PATH, effectName);
		}
		else if (effectName.IndexOf("scene_eff_") != -1)
		{
			return string.Format(SCENE_EFFECT_PATH, effectName);
		}
		else
		{
			return null;
		}
	}
	
}
