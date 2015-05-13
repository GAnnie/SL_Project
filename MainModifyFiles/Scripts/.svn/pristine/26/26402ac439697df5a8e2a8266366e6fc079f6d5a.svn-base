using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.charactor.data;


/// <summary>
/// Model helper.
/// <para>包含了模型帮助函数，大致分为三大类</para>
/// <para>1.获取模型资源路径</para>
/// <para>2.设置模型材质，或一些附加效果</para>
/// <para>3.模型动作相关方法</para>
/// </summary>
public static class ModelHelper
{
	public const int Animator_Layer_BaseLayer = 0;
	public const int Animator_Layer_BattleLayer = 1;

	public const int DefaultModelId = 2020; //默认宠物ID

	//模型基础动作名称
	public const string Anim_idle    = "idle"; //场景待机
	public const string Anim_battle  = "battle"; //战斗待机
	public const string Anim_hit     = "hit"; //战斗受击
	public const string Anim_hit2 	 = "hit2";//战斗受击2
	public const string Anim_run     = "run"; //走动
	public const string Anim_death   = "death"; //死亡
	public const string Anim_def     = "def"; //防御
	public const string Anim_skill   = "skill"; //施法
	public const string Anim_attack1 = "attack1"; //攻击1
	public const string Anim_attack2 = "attack2"; //攻击2
	public const string Anim_attack3 = "attack3"; //攻击3
	public const string Anim_show    = "show"; //show

	//模型锚点
	public const string Mount_hit    = "Mount_Hit"; //受击锚点
	public const string Mount_hud    = "Mount_HUD"; //头顶锚点
	public const string Mount_shadow = "Mount_Shadow"; //阴影锚点
	public const string Mount_face   = "Mount_face"; //面部锚点

	#region Model Path Func
	private const string STYLE_TYPE_PET = "pet";

	private const string Config_ReadPath = "ConfigFiles/WeaponConfig/WeaponConfig";
	private static Dictionary< string, WeaponBindConfig> weaponConfigMaps;
	
	public static void Setup()
	{
		if (weaponConfigMaps == null)
		{
			weaponConfigMaps = new Dictionary<string, WeaponBindConfig>();

			WeaponConfig weaponConfig = DataHelper.GetJsonFile<WeaponConfig>(Config_ReadPath, "bytes", false);
			if (weaponConfig != null)
			{
				foreach (WeaponBindConfig config in weaponConfig.list)
				{
					weaponConfigMaps.Add(config.key, config);
				}
			}
		}
	}

	private static WeaponBindConfig GetWeaponBindConfig(string key)
	{
		WeaponBindConfig config = null;
		if (weaponConfigMaps != null)
		{
			weaponConfigMaps.TryGetValue(key, out config);
		}
		return config;
	}

	public static string GetDefaultPrefabPath()
	{
		return string.Format(PathHelper.PET_PREFAB_PATH, "pet_1");
	}

	static public string GetCharacterPrefabPath(int modelId)
	{
		return GetCharacterPrefabPath(modelId.ToString());
	}

	public static string GetCharacterPrefabPath(string model)
	{
		if (!model.StartsWith(STYLE_TYPE_PET))
		{
			model = "pet_" + model;
		}
		return string.Format(PathHelper.PET_PREFAB_PATH, model);
	}
	#endregion

	#region Material or Shader Func
	public static void SetPetLook(GameObject petObject, int texture , int mutateTexture, string colorParams)
	{
		if (petObject == null){
			GameDebuger.Log("SetPetLook Error petObject = null");
			return;
		}

		string petDir = "";
		string matName = "";

		if (mutateTexture > 0)
		{
			petDir = "pet_" + mutateTexture;
			matName = "pet_" + mutateTexture + "_mutate";
		}
		else if (colorParams != "")
		{
			petDir = "pet_" + texture;
			matName = "pet_" + texture + "_mask";
		}
		else
		{
			petDir = "pet_" + texture;
			matName = "pet_" + texture;
		}

		//遍历找到模型Renderer所在节点
		Transform petTrans = petObject.transform;
		Renderer modelRenderer = null;
		for (int i = 0, imax = petTrans.GetChildCount(); i < imax; ++i)
		{
			Transform child = petTrans.GetChild(i);
			if (child.name.StartsWith("pet_"))
			{
				modelRenderer = child.GetComponent<Renderer>();
				break;
			}
		}

		if (modelRenderer != null){
			string curMatName = modelRenderer.sharedMaterial.name.Replace("(Instance)","");
			if(curMatName == matName){
				ChangeModelHSV(modelRenderer,colorParams);
			}else{
				ModelHelper.ChangeMaterialAsync(modelRenderer, 
				                                string.Format(PathHelper.PET_MATERIALS_PATH, petDir, matName),
				                                ()=>{
					ChangeModelHSV(modelRenderer,colorParams);
				});
			}
		}
	}

	public static void ChangeModelHSV(Renderer renderer,string colorParams){
		if (!string.IsNullOrEmpty(colorParams))
		{
			ModelHSV hsv = renderer.gameObject.GetMissingComponent<ModelHSV>();
			hsv.SetupColorParams(colorParams);
		}
	}

	//同步加载新材质球进行替换
	public static void ChangeMaterial(Renderer renderer, string materialUrl )
	{
		if(renderer == null)
		{
			Debug.LogError("the gameObject has not a Renderer component.");
			return;
		}
		
		//实例化,材质//
		//读取材质球//
		Material newMaterial = ResourceLoader.Load(materialUrl,"mat") as Material;
		
		if ( newMaterial == null )
		{
			GameDebuger.Log(string.Format("asset not found!,path:{0}", materialUrl));
			return;
		}
		
		renderer.material = newMaterial;
	}

	//异步加载材质球进行替换
	public static void ChangeMaterialAsync(Renderer renderer, string materialUrl , System.Action finishCallback = null)
	{
		if(renderer == null)
		{
			Debug.LogError("the gameObject has not a Renderer component.");
			return;
		}
		
		ResourcePoolManager.Instance.Spawn( materialUrl, delegate( UnityEngine.Object obj )
		                                   {
			if (obj == null){
				GameDebuger.Log(string.Format("materialUrl not found!,path:{0}", materialUrl));
				return;
			}
			
			//实例化,材质//
			//读取材质球//
			Material newMaterial = obj as Material;
			
			if ( newMaterial == null )
			{
				GameDebuger.Log(string.Format("asset not found material!,path:{0}", materialUrl));
				return;
			}
			
			renderer.material = newMaterial;
			
			if (finishCallback != null)
			{
				finishCallback();
			}
		},
		ResourcePoolManager.PoolType.DESTROY_CHANGE_SCENE);
	}

	public static void SetPetShadow(GameObject model)
	{
		Transform shadow = model.transform.Find ("Shadow(Clone)");
		if (shadow == null)
		{
			ResourcePoolManager.Instance.Spawn(PathHelper.SHADOW_PREFAB_PATH, delegate(Object obj) {
				if (obj == null)
				{
					return;
				}
				
				GameObject shadowGO = GameObjectExt.AddPoolChild(model, obj as GameObject);
				shadowGO.transform.localPosition = new Vector3(0f, 0.01f, 0f);
				shadowGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

			}, ResourcePoolManager.PoolType.DONT_DESTROY);
		}
	}
	#endregion

	#region ChangeModelWeapon
	public static void UpdateModelWeapon(GameObject go, int actorModelId, int wpId)
	{
		DoUpdateModelWeapon(go, actorModelId, wpId, "Bip001/Bip001 Prop1");
		DoUpdateModelWeapon(go, actorModelId, wpId, "Bip001/Bip001 Prop2");
	}

	private static void DoUpdateModelWeapon(GameObject go, int actorModelId, int wpId, string bip001Name)
	{
		Transform tran = go.transform.Find (bip001Name);
		if (tran != null)
		{
			if (wpId == 0)
			{
				foreach (Transform child in tran) {
					if (child.gameObject.name.Contains("weapon_"))
					{
						GameObject.Destroy (child.gameObject);
					}
				}
			}
			else
			{
				string wpPath = string.Format (PathHelper.WEAPON_PREFAB_PATH, "weapon_" + wpId);
				ResourcePoolManager.Instance.Spawn(wpPath, delegate(Object obj) {
					if (obj != null)
					{
						foreach (Transform child in tran) {
							if (child.gameObject.name.Contains("weapon_"))
							{
								GameObject.Destroy (child.gameObject);
							}
						}
						GameObject wpGO = obj as GameObject;
						if (wpGO != null)
						{
							GameObjectExt.AddPoolChild(go, wpGO);
							wpGO.transform.parent = tran;

							WeaponBindConfig config = GetWeaponBindConfig("pet_" + actorModelId + "/" + bip001Name);
							if (config != null)
							{
								wpGO.transform.localPosition = config.localPosition;
								wpGO.transform.localEulerAngles = config.localEulerAngles;

								if (bip001Name == "Bip001/Bip001 Prop2")
								{
									Transform subTrans = wpGO.transform.Find("weapon_" + wpId);
									if (subTrans != null)
									{
										subTrans.localEulerAngles = new Vector3(subTrans.localEulerAngles.x, subTrans.localEulerAngles.y+180f, subTrans.localEulerAngles.z);
										subTrans.localPosition = new Vector3(-1*subTrans.localPosition.x, subTrans.localPosition.y, subTrans.localPosition.z);
									}
								}
							}
						}
					}
					
				}, ResourcePoolManager.PoolType.DONT_DESTROY);
			}
		}
	}
	#endregion

	#region Animation Func
	public static void PlayAnimation(Animator anim, string action, bool crossFade, System.Action< string , float > animClipCallBack = null, bool checkSameAnim = false, int layer = 0)
	{
		if (anim == null)
		{
			if (animClipCallBack != null) animClipCallBack(action, 0);
			return;
		}
		
		if (string.IsNullOrEmpty(action))
		{
			if (animClipCallBack != null) animClipCallBack(action, 0);
			return;
		}
		
		if (checkSameAnim)
		{
			AnimatorStateInfo animatorState = anim.GetCurrentAnimatorStateInfo(layer);
			if (animatorState.IsName(action))
			{
				if (animClipCallBack != null) animClipCallBack(action, 0);
				return;
			}
		}
		
		try
		{
			if (layer == ModelHelper.Animator_Layer_BaseLayer)
			{
				anim.SetLayerWeight(ModelHelper.Animator_Layer_BattleLayer, 0);
			}
			else
			{
				anim.SetLayerWeight(ModelHelper.Animator_Layer_BattleLayer, 1);
			}

			if (crossFade)
			{
				anim.CrossFade(action, 0.2f, layer);
			}
			else
			{
				List<string> checkList = new List<string>(){"idle","run","hit","death","attack1","attack2","attack3","battle","skill","def","show"};
				if (checkList.Contains(action) == false)
				{
					Debug.LogError("ErrorAction=" + action);
				}
				anim.Play(action, layer, 0f);
			}

			AnimatorStateInfo animatorState = anim.GetCurrentAnimatorStateInfo(layer);
			if (animatorState.IsName(action))
			{
				if (animClipCallBack != null) animClipCallBack(action, animatorState.length);
				return;
			}

			if (animClipCallBack != null) animClipCallBack(action, 0);
		}
		catch(System.Exception e)
		{
			GameDebuger.Log(" Can not find action : " + action);
			if (animClipCallBack != null) animClipCallBack(action, 0);
		}
	}
	#endregion
	
	static public Transform GetMountingPoint(GameObject obj, string point)
	{
		if (obj == null)
		{
			return null;
		}
		
		Transform[] trs = obj.GetComponentsInChildren<Transform>();
		
		foreach (Transform tr in trs)
		{
			if (point == tr.gameObject.name)
				return tr;
		}
		
		Debug.LogError("模型" + obj.name + " 没有配置锚点:" + point);
		return null;
	}
}
