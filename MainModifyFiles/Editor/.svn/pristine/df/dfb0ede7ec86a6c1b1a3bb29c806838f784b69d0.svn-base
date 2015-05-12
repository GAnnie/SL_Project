using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using LITJson;

public class BattleConfigEditor : EditorWindow
{
	private const string BattleConfig_WritePath = "Assets/GameResources/ConfigFiles/BattleConfig/BattleConfig.bytes";
	
	private const string BattleConfig_ReadPath = "ConfigFiles/BattleConfig/BattleConfig";
	
	private Dictionary<int,SkillConfigInfo> _configInfoDic = new Dictionary<int,SkillConfigInfo> ();
	
	private SkillConfigInfo _skillConfigInfo = null;

	enum ActionType
	{
		Normal,
		Move,
		MoveBack,
	}
	
	ActionType _actionType = ActionType.Normal;
	
	enum EffectType
	{
		TakeDamage,
		ShowInjure,
		Normal,
		Sound,
	}
	
	EffectType _effectType = EffectType.Normal;
	
	//	public const string Anim_idle    = "idle"; //场景待机
	//	public const string Anim_battle  = "battle"; //战斗待机
	//	public const string Anim_hit     = "hit"; //战斗受击
	//	public const string Anim_run     = "run"; //走动
	//	public const string Anim_death   = "death"; //死亡
	//	public const string Anim_def     = "def"; //防御
	//	public const string Anim_skill   = "skill"; //施法
	//	public const string Anim_attack1 = "attack1"; //攻击1
	//	public const string Anim_attack2 = "attack2"; //攻击2
	//	public const string Anim_attack3 = "attack3"; //攻击3
	
	enum AnimType
	{
		skill,
		attack1,
		attack2,
		attack3,
		hit,
		run,
	}
	private AnimType GetAnimType(string name)
	{
		if (name == AnimType.hit.ToString())
		{
			return AnimType.hit;
		}
		else if(name == AnimType.skill.ToString())
		{
			return AnimType.skill;
		}
		else if(name == AnimType.run.ToString())
		{
			return AnimType.run;
		}
		else if(name == AnimType.attack1.ToString())
		{
			return AnimType.attack1;
		}
		else if(name == AnimType.attack2.ToString())
		{
			return AnimType.attack2;
		}
		else if(name == AnimType.attack3.ToString())
		{
			return AnimType.attack3;
		}
		return AnimType.hit;
	}
	
	////特效目标  0默认， 1，场景中心 2，我方中心   3， 敌军中心
	enum TargetType
	{
		Default,
		Center,
		PlayerCenter,
		EnemyCenter,
	}
	
	enum MountType
	{
		Mount_Hit,
		Mount_HUD,
		Mount_Shadow,
	}
	private MountType GetMountType(string name)
	{
		if (name == MountType.Mount_Hit.ToString())
		{
			return MountType.Mount_Hit;
		}
		else if(name == MountType.Mount_HUD.ToString())
		{
			return MountType.Mount_HUD;
		}
		else if(name == MountType.Mount_Shadow.ToString())
		{
			return MountType.Mount_Shadow;
		}
		return MountType.Mount_Hit;
	}
	
	enum SkillEffectType
	{
		hit,
		hit1,
		hit2,
		att,
		fly,
		full,
	}
	private SkillEffectType GetSkillEffectType(string name)
	{
		if (name == SkillEffectType.hit.ToString())
		{
			return SkillEffectType.hit;
		}
		if (name == SkillEffectType.hit1.ToString())
		{
			return SkillEffectType.hit1;
		}
		else if(name == SkillEffectType.hit2.ToString())
		{
			return SkillEffectType.hit2;
		}
		else if(name == SkillEffectType.att.ToString())
		{
			return SkillEffectType.att;
		}
		else if(name == SkillEffectType.fly.ToString())
		{
			return SkillEffectType.fly;
		}
		else if(name == SkillEffectType.full.ToString())
		{
			return SkillEffectType.full;
		}
		return SkillEffectType.hit;
	}
	
	[MenuItem ("Window/BattleConfigEditor  #&b")]
	public static void  ShowWindow ()
	{     
		var window = EditorWindow.GetWindow<BattleConfigEditor> (false, "BattleConfigEditor", true);
		window.Show ();
	}
	
	void OnGUI ()
	{
		EditorGUILayout.BeginHorizontal ();
		
		EditorGUILayout.BeginVertical (); //Right Part Vertical Begin
		DrawSkillConfigListView ();
		DrawConfigToolView ();
		EditorGUILayout.EndVertical (); //Right Part Vertical End
		
		EditorGUILayout.BeginVertical (); //Right Part Vertical Begin
		DrawTypeSelectGroup ();
		GUILayout.Space (5f);
		DrawSkillConfigView ();
		EditorGUILayout.EndVertical (); //Right Part Vertical End
		
		EditorGUILayout.EndHorizontal ();
		
	}
	
	private int _newSkillId = 0;
	private string _newSkillName = "";
	
	private void DrawConfigToolView()
	{
		if (GUILayout.Button ("LoadConfig", GUILayout.Width (200f), GUILayout.Height (30))) {
			LoadConfig();
		}

		EditorGUILayout.BeginHorizontal ();
		_newSkillId = EditorHelper.DrawIntField ("ID", _newSkillId, 30, false);
		_newSkillName = EditorHelper.DrawTextField ("Name", _newSkillName, 80, false);
		EditorGUILayout.EndHorizontal ();

		if (GUILayout.Button ("AddSkill", GUILayout.Width (200f), GUILayout.Height (30))) {
			if (_configInfoDic.ContainsKey(_newSkillId) == false)
			{
				SkillConfigInfo info = new SkillConfigInfo();
				info.attackerActions = new System.Collections.Generic.List<BaseActionInfo> ();
				info.injurerActions = new System.Collections.Generic.List<BaseActionInfo> ();
				info.id = _newSkillId;
				info.name = _newSkillName;
				_skillConfigInfo = info;
				_configInfoDic.Add(info.id, info);
			}
		}

		if (GUILayout.Button ("SaveConfig", GUILayout.Width (200f), GUILayout.Height (30))) {
			SaveConfig();
			
			//			if (_battleConfigInfo != null)
			//			{
			//				_battleConfigInfo.list.Clear();
			//			}
			//			_battleConfigInfo = null;
			//			_skillConfigInfo = null;
		}
	}
	
	#region SelectionComponentView
	private Vector2 skillConfigListViewPos;
	#endregion
	
	private int _removeId = 0;
	
	private void DrawSkillConfigListView()
	{
		EditorGUILayout.BeginVertical (GUILayout.MinWidth (200f));
		
		EditorHelper.DrawHeader ("SkillConfigList");
		
		skillConfigListViewPos = EditorGUILayout.BeginScrollView (skillConfigListViewPos, "TextField");
		
		foreach (SkillConfigInfo info in _configInfoDic.Values) {
			DrawSkillCellView(info);
		}
		
		EditorGUILayout.EndScrollView ();
		
		EditorGUILayout.EndVertical ();
		
		if (_removeId > 0)
		{
			_configInfoDic.Remove(_removeId);
			_removeId = 0;
		}
	}
	
	private void DrawSkillCellView(SkillConfigInfo info)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Box (info.id.ToString(), GUILayout.Width(50));
		GUILayout.Box (info.name.ToString(), GUILayout.Width(50));
		
		GUI.color = Color.green;
		if (GUILayout.Button ("Sel", GUILayout.Width (40), GUILayout.Height (20))) {
			_skillConfigInfo = info;
		}
		GUI.color = Color.white;
		
		
		GUI.color = Color.red;
		if (GUILayout.Button ("X", GUILayout.Width (20), GUILayout.Height (20))) {
			if (_skillConfigInfo == info)
			{
				_skillConfigInfo = null;
			}
			_removeId = info.id;
		}
		GUI.color = Color.white;
		
		EditorGUILayout.EndHorizontal ();
	}
	
	void DrawTypeSelectGroup()
	{
		EditorGUILayout.BeginHorizontal ();
		
		if (_skillConfigInfo != null){
			GUILayout.Box (_skillConfigInfo.id.ToString(), GUILayout.Width(50));
			string skillName = EditorHelper.DrawTextField ("Name", _skillConfigInfo.name, 50);

			if (GUI.changed)
			{
				_skillConfigInfo.name = skillName;
			}
		}

		_actionType = (ActionType)EditorGUILayout.EnumPopup("ActionType", _actionType, GUILayout.Width(300f));
		
		GUILayout.Space (100f);
		
		_effectType = (EffectType)EditorGUILayout.EnumPopup("EffectType", _effectType, GUILayout.Width(300f));
		
		EditorGUILayout.EndHorizontal ();
	}
	
	private BaseActionInfo GetAddActionInfo()
	{
		BaseActionInfo info = null;
		switch (_actionType)
		{
		case ActionType.Normal:
			info =  new NormalActionInfo();
			info.type = NormalActionInfo.TYPE;
			break;
		case ActionType.Move:
			info =  new MoveActionInfo();
			info.type = MoveActionInfo.TYPE;
			break;
		case ActionType.MoveBack:
			info = new MoveBackActionInfo();
			info.type = MoveBackActionInfo.TYPE;
			break;
		}
		
		if (info != null)
		{
			info.effects = new System.Collections.Generic.List<BaseEffectInfo>();
		}
		
		return info;
	}
	
	private BaseEffectInfo GetAddEffectInfo()
	{
		BaseEffectInfo info = null;
		switch (_effectType)
		{
		case EffectType.Normal:
			info = new NormalEffectInfo();
			info.type = NormalEffectInfo.TYPE;
			break;
		case EffectType.ShowInjure:
			info = new ShowInjureEffectInfo();
			info.type = ShowInjureEffectInfo.TYPE;
			break;
		case EffectType.TakeDamage:
			info = new TakeDamageEffectInfo();
			info.type = TakeDamageEffectInfo.TYPE;
			break;
		case EffectType.Sound:
			info = new SoundEffectInfo();
			info.type = SoundEffectInfo.TYPE;
			break;
		}
		
		return info;
	}
	
	private void DrawSkillConfigView()
	{
		if (_skillConfigInfo != null)
		{
			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.green;
			if (GUILayout.Button ("A", GUILayout.Width (20), GUILayout.Height (20))) {
				_skillConfigInfo.attackerActions.Insert(0, GetAddActionInfo());
			}
			GUI.color = Color.white;
			EditorHelper.DrawHeader ("AttackerActions");
			EditorGUILayout.EndHorizontal ();
			
			
			for (int i=0; i<_skillConfigInfo.attackerActions.Count; i++)
			{
				BaseActionInfo actionInfo = _skillConfigInfo.attackerActions[i] as BaseActionInfo;
				DrawActionInfoView(actionInfo);
				GUILayout.Space(5f);
			}
			
			GUILayout.Space(10f);
			
			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.green;
			if (GUILayout.Button ("A", GUILayout.Width (20), GUILayout.Height (20))) {
				_skillConfigInfo.injurerActions.Insert(0, GetAddActionInfo());
			}
			GUI.color = Color.white;
			EditorHelper.DrawHeader ("InjurerActions");
			EditorGUILayout.EndHorizontal ();
			
			for (int i=0; i<_skillConfigInfo.injurerActions.Count; i++)
			{
				BaseActionInfo actionInfo = _skillConfigInfo.injurerActions[i] as BaseActionInfo;
				DrawActionInfoView(actionInfo);
				GUILayout.Space(5f);
			}
		}
	}
	
	private void DrawActionInfoView(BaseActionInfo info)
	{
		if (info == null)
		{
			return;
		}
		
		if (info.GetType() == typeof(MoveActionInfo))
		{
			DrawMoveActionInfoView(info as MoveActionInfo);
		}
		else if (info.GetType() == typeof(NormalActionInfo))
		{
			DrawNormalActionInfoView(info as NormalActionInfo);
		}
		else if (info.GetType() == typeof(MoveBackActionInfo))
		{
			DrawMoveBackActionInfoView(info as MoveBackActionInfo);
		}
		else
		{
			Debug.LogError("DrawActionInfoView type Error");
		}
		
		for (int i=0; i<info.effects.Count; i++)
		{
			BaseEffectInfo effectInfo = info.effects[i] as BaseEffectInfo;
			DrawEffectInfoView(info, effectInfo);
			GUILayout.Space(5f);
		}
	}
	
	private void DrawEffectInfoView(BaseActionInfo actionInfo, BaseEffectInfo info)
	{
		if (info == null)
		{
			return;
		}
		
		if (info.GetType() == typeof(NormalEffectInfo))
		{
			DrawNormalEffectInfoView(actionInfo, info as NormalEffectInfo);
		}
		else if (info.GetType() == typeof(TakeDamageEffectInfo))
		{
			DrawTakeDamageEffectInfoView(actionInfo, info as TakeDamageEffectInfo);
		}
		else if (info.GetType() == typeof(ShowInjureEffectInfo))
		{
			DrawShowInjureEffectInfoView(actionInfo, info as ShowInjureEffectInfo);
		}
		else if (info.GetType() == typeof(SoundEffectInfo))
		{
			DrawSoundEffectInfoView(actionInfo, info as SoundEffectInfo);
		}
		else
		{
			Debug.LogError("DrawEffectInfoView type Error");
		}
	}
	
	private void DrawBaseActionInifoView(BaseActionInfo info)
	{
		GUI.color = Color.yellow;
		GUILayout.Box (info.type, GUILayout.Width(70));
		GUI.color = Color.white;
		
		GUI.color = Color.red;
		if (GUILayout.Button ("X", GUILayout.Width (20), GUILayout.Height (20))) {
			_skillConfigInfo.attackerActions.Remove(info);
			_skillConfigInfo.injurerActions.Remove(info);
			return;
		}
		GUI.color = Color.white;
		
		GUI.color = Color.green;
		if (GUILayout.Button ("A", GUILayout.Width (20), GUILayout.Height (20))) {
			
			BaseActionInfo actionInfo = GetAddActionInfo();
			if (actionInfo != null)
			{
				if (_skillConfigInfo.attackerActions.Contains(info))
				{
					int index = _skillConfigInfo.attackerActions.IndexOf(info);
					_skillConfigInfo.attackerActions.Insert(index+1, actionInfo);
				}
				
				if (_skillConfigInfo.injurerActions.Contains(info))
				{
					int index = _skillConfigInfo.injurerActions.IndexOf(info);
					_skillConfigInfo.injurerActions.Insert(index+1, actionInfo);
				}
			}
		}
		
		if (GUILayout.Button ("E", GUILayout.Width (20), GUILayout.Height (20))) {
			BaseEffectInfo effectInfo = GetAddEffectInfo();
			info.effects.Insert(0, effectInfo);
		}
		GUI.color = Color.white;
		
		GUI.changed = false;
		
		AnimType animType = DrawAnimType ("动作", info.name);
		
		//		int rotateX = EditorHelper.DrawIntField("rotateX", info.rotateX);
		//		int rotateY = EditorHelper.DrawIntField("rotateY", info.rotateY);
		//		int rotateZ = EditorHelper.DrawIntField("rotateZ", info.rotateZ);
		
		if (GUI.changed)
		{
			info.name = animType.ToString();
			//			info.rotateX = rotateX;
			//			info.rotateY = rotateY;
			//			info.rotateZ = rotateZ;
		}
	}
	
	private void DrawMoveActionInfoView(MoveActionInfo info)
	{
		EditorGUILayout.BeginHorizontal ();
		
		GUILayout.Space(30f);
		
		DrawBaseActionInifoView (info);
		
		GUI.changed = false;
		
		float time = EditorHelper.DrawFloatField ("时长", info.time, 30, false);
		float distance = EditorHelper.DrawFloatField ("距离", info.distance, 30, false);
		bool center = EditorHelper.DrawToggle ("是否中心", info.center, 30, false);
		
		if (GUI.changed)
		{
			info.time = time;
			info.distance = distance;
			info.center = center;
		}
		
		EditorGUILayout.EndHorizontal ();
	}
	
	private void DrawNormalActionInfoView(NormalActionInfo info)
	{
		EditorGUILayout.BeginHorizontal ();
		
		GUILayout.Space(20f);
		
		DrawBaseActionInifoView (info);
		
		GUI.changed = false;

		float startTime = EditorHelper.DrawFloatField ("开始", info.startTime, 30, false);
		float delayTime = EditorHelper.DrawFloatField ("延时", info.delayTime, 30, false);

		if (GUI.changed)
		{
			info.startTime = startTime;
			info.delayTime = delayTime;
		}
		
		EditorGUILayout.EndHorizontal ();
	}
	
	private void DrawMoveBackActionInfoView(MoveBackActionInfo info)
	{
		EditorGUILayout.BeginHorizontal ();
		
		GUILayout.Space(20f);
		
		DrawBaseActionInifoView (info);
		
		GUI.changed = false;
		
		float time = EditorHelper.DrawFloatField ("时长", info.time, 30, false);
		
		if (GUI.changed)
		{
			info.time = time;
		}
		
		EditorGUILayout.EndHorizontal ();
	}
	
	private void DrawNormalEffectInfoView(BaseActionInfo actionInfo, NormalEffectInfo info)
	{
		EditorGUILayout.BeginHorizontal ();
		
		GUILayout.Space(50f);
		
		DrawBaseEffectInifoView (actionInfo, info);
		
		GUI.changed = false;
		
		SkillEffectType effectType = DrawSkillEffectType ("特效类型", info.name);
		string effectTypeStr = EditorHelper.DrawTextField ("", info.name, 120, false);
		float delayTime = EditorHelper.DrawFloatField ("延时", info.delayTime, 30, false);

		bool hitEff = EditorHelper.DrawToggle ("是否受击", info.hitEff, 30, false);

		int target = EditorHelper.DrawIntField ("作用目标", info.target, 30, false);
		MountType mountType = DrawMountType ("作用锚点", info.mount);
		
		bool fly = EditorHelper.DrawToggle ("是否飞行", info.fly, 30, false);
		int flyTarget = EditorHelper.DrawIntField ("飞向目标", info.flyTarget, 30, false);
		
		if (GUI.changed)
		{
			if (effectTypeStr != null && (effectTypeStr.Contains("skill_") || effectTypeStr.Contains("game_")))
			{
				info.name = effectTypeStr;
			}
			else
			{
				info.name = effectType.ToString();
			}
			info.delayTime = delayTime;
			info.target = target;
			info.mount = mountType.ToString();
			info.fly = fly;
			info.flyTarget = flyTarget;
			info.hitEff = hitEff;
		}
		
		EditorGUILayout.EndHorizontal ();
	}
	
	private SkillEffectType DrawSkillEffectType(string title, string effect)
	{
		GUILayout.Box (title);
		return (SkillEffectType)EditorGUILayout.EnumPopup("", GetSkillEffectType(effect), GUILayout.Width(50));
	}
	
	private AnimType DrawAnimType(string title, string anim)
	{
		GUILayout.Box (title);
		return (AnimType)EditorGUILayout.EnumPopup("", GetAnimType(anim), GUILayout.Width(70));
	}
	
	private MountType DrawMountType(string title, string mount)
	{
		GUILayout.Box (title);
		return (MountType)EditorGUILayout.EnumPopup("", GetMountType(mount), GUILayout.Width(90));
	}
	
	private void DrawTakeDamageEffectInfoView(BaseActionInfo actionInfo, TakeDamageEffectInfo info)
	{
		EditorGUILayout.BeginHorizontal ();
		
		GUILayout.Space(50f);
		
		DrawBaseEffectInifoView (actionInfo, info);
		
		GUI.changed = false;
		
		EditorGUILayout.EndHorizontal ();
	}
	
	
	private void DrawShowInjureEffectInfoView(BaseActionInfo actionInfo, ShowInjureEffectInfo info)
	{
		EditorGUILayout.BeginHorizontal ();
		
		GUILayout.Space(50f);
		
		DrawBaseEffectInifoView (actionInfo, info);
		
		EditorGUILayout.EndHorizontal ();
	}

	private void DrawSoundEffectInfoView(BaseActionInfo actionInfo, SoundEffectInfo info)
	{
		EditorGUILayout.BeginHorizontal ();
		
		GUILayout.Space(50f);
		
		DrawBaseEffectInifoView (actionInfo, info);

		string soundName = EditorHelper.DrawTextField ("音效", info.name, 120, false);
		if (GUI.changed)
		{
			info.name = soundName;
		}

		EditorGUILayout.EndHorizontal ();
	}
	
	private void DrawBaseEffectInifoView(BaseActionInfo actionInfo, BaseEffectInfo info)
	{
		GUI.color = Color.yellow;
		GUILayout.Box (info.type, GUILayout.Width(70));
		GUI.color = Color.white;
		
		GUI.color = Color.red;
		if (GUILayout.Button ("X", GUILayout.Width (20), GUILayout.Height (20))) {
			actionInfo.effects.Remove(info);
			return;
		}
		GUI.color = Color.white;
		
		GUI.color = Color.green;
		if (GUILayout.Button ("E", GUILayout.Width (20), GUILayout.Height (20))) {
			BaseEffectInfo effectInfo = GetAddEffectInfo();
			int index = actionInfo.effects.IndexOf(info);
			actionInfo.effects.Insert(index+1, effectInfo);
		}
		GUI.color = Color.white;
		
		GUI.changed = false;
		
		float playTime = EditorHelper.DrawFloatField ("播放时间", info.playTime, 30, false);
		
		if (GUI.changed)
		{
			info.playTime = playTime;
		}
	}
	
	void LoadConfig ()
	{
		JsonBattleConfigInfo configInfo = DataHelper.GetJsonFile<JsonBattleConfigInfo>(BattleConfig_ReadPath, "bytes", false);
		
		if (configInfo != null)
		{
			JsonSkillConfigInfo[] list = configInfo.list.ToArray();
			Array.Sort(list, delegate(JsonSkillConfigInfo x, JsonSkillConfigInfo y) {
				if (x.id >= y.id)
				{
					return 1;
				}else{
					return -1;
				}
			});	
			_configInfoDic.Clear();
			foreach(JsonSkillConfigInfo info in list)
			{
				_configInfoDic.Add(info.id, info.ToSkillConfigInfo());
			}
		}
	}
	
	void SaveConfig ()
	{
		BattleConfigInfo configInfo = new BattleConfigInfo();
		configInfo.time = (DateTime.UtcNow.Ticks / 10000).ToString ();
		configInfo.list = new List<SkillConfigInfo> (_configInfoDic.Values);
		
		bool isCompress = false;
		
		string json = JsonMapper.ToJson (configInfo);
		byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes (json);
		if (isCompress) {
			buff = ZipLibUtils.Compress (buff);
		}
		
		if (json.Length > 0) {
			DataHelper.SaveFile (BattleConfig_WritePath, buff);
		}
		
		AssetDatabase.Refresh();
		//DataHelper.SaveJsonFile (configInfo, BattleConfig_WritePath, false);
	}
	
	void OnDestroy ()
	{
		AssetDatabase.Refresh();
		_skillConfigInfo = null;
	}
}
