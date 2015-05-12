using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using CodeGeneration;

public class UIViewGenerator : EditorWindow
{
	private const string VIEW_CACHE_PATH = "Assets/Editor/UIViewGenerator/Cache/";
	private const string VIEWCODE_GENERATED_PATH = "Assets/Scripts/MyGameScripts/Module/";
	private const string STATIONARY_GENERATEDLUA_PATH = "Assets/_luaTest/Lua/GameScripts/UIModule/UIView/";
	private const string STATIONARY_GENERATEDLUA_PATH_DEFAULT = "/_luaTest/Lua/GameScripts/UIModule/UIView/";

	[MenuItem ("Window/UIViewGenerator  #&u")]
	public static void  ShowWindow ()
	{
		var window = EditorWindow.GetWindow<UIViewGenerator> (false, "UIViewGenerator", true);
		window.Show ();
	}

	#region Title
	private Transform dragItem;
	private Transform UIPrefabRoot;
	#endregion

	#region SelectionComponentView
	private Vector2 selectionViewPos;
	#endregion

	#region ComponentInfoView
	private UIComponentInfo _curComInfo = null;
	#endregion

	#region ExportListView
	private string _codeGeneratePath = "";
	private string _codeLuaGeneratePath = "";
	private bool exportDicChanged = true;

	private Vector2 exportViewPos;
	private Dictionary<string,UIComponentInfo> _exportInfoDic = new Dictionary<string,UIComponentInfo> ();
	private Dictionary<string,bool> _validatedInfoDic = new Dictionary<string, bool>();
	private List<string> _deleteUIDList = new List<string> ();
	#endregion

	void OnSelectionChange ()
	{
		EditorGUIUtility.editingTextField = false;
//		_curComInfo = null;
		Repaint ();
	}

	void OnProjectChange ()
	{
		exportDicChanged = true;
	}

	void OnDestroy ()
	{
		if (UIPrefabRoot && exportDicChanged) {
			if (EditorUtility.DisplayDialog ("Save UIViewCache", "当前UIViewCache尚未保存，请问是否保存", "Save", "Canel")) {
				SaveUIViewCache ();
			}
		}
		
		AssetDatabase.Refresh();
	}

	void OnGUI ()
	{
		DrawTitleGroup ();

		if (!UIPrefabRoot)
			return;

		EditorGUILayout.BeginHorizontal ();

		//SelectionComponentView
		DrawSelectionComponentView ();
		
		EditorGUILayout.BeginVertical (); //Right Part Vertical Begin
		//ComponentInfoView
		DrawComponentInfoView ();

		//ExportListView
		DrawExportListView ();
		EditorGUILayout.EndVertical (); //Right Part Vertical End
		
		EditorGUILayout.EndHorizontal ();
	}

	//读取缓存UIViewCache数据
	void LoadUIViewCache ()
	{
		OnChangeUIPrefab();
		UIViewCache record = EditorHelper.LoadScriptableObjectAsset<UIViewCache> (VIEW_CACHE_PATH + UIPrefabRoot.name);

		if (record != null) {
			AdjustCodePath(record);
			_codeGeneratePath = record.codePath;
			AdjustLuaCodePath(record);
			_codeLuaGeneratePath = record.codeLuaPath;
			foreach (UIComponentInfo item in record.componentInfoList) {
				if (!_exportInfoDic.ContainsKey (item.uid)){
					_exportInfoDic.Add (item.uid, new UIComponentInfo (item));
					Transform itemTrans = UIPrefabRoot.Find(item.path);
					_validatedInfoDic.Add (item.uid,(itemTrans != null));
				}
				else
					Debug.Log ("There is the same key in UIViewCach record");
			}
		} else {
//			if(EditorUtility.DisplayDialog("保存UIViewCache","当前UIPrefab没有缓存记录,是否新建一个记录以便日后修改？","OK","Cancel")){
			SaveUIViewCache ();	
//			}
		}
	}

	private void AdjustCodePath(UIViewCache record)
	{
		string codePath = record.codePath;
		string flag = "/Assets";
		int index = codePath.IndexOf (flag);
		if (index != -1)
		{
			record.codePath = codePath.Substring(index+flag.Length);
		}
	}
	
	private void AdjustLuaCodePath(UIViewCache record)
	{
		string codeLuaPath = record.codeLuaPath;
		string flag = "/Assets";
		int index = codeLuaPath.IndexOf (flag);
		if (index != -1)
		{
			record.codeLuaPath = codeLuaPath.Substring(index+flag.Length);
		}
	}

	//保存UIViewCache信息
	UIViewCache SaveUIViewCache ()
	{
		UIViewCache record = ScriptableObject.CreateInstance<UIViewCache> ();
		record.codePath = _codeGeneratePath;
		record.codeLuaPath = _codeLuaGeneratePath;

		record.componentInfoList = new List<UIComponentInfo> (_exportInfoDic.Values);
		EditorHelper.CreateScriptableObjectAsset (record, VIEW_CACHE_PATH, UIPrefabRoot.name);
		
		return record;
	}

	void DrawTitleGroup ()
	{
		EditorGUILayout.BeginHorizontal ();

		EditorGUILayout.BeginVertical();
		GUILayout.Space(15f);
		EditorGUILayout.LabelField (@"UIPrefab", GUILayout.Width (60f));
		GUILayout.Space(5f);
		dragItem = EditorGUILayout.ObjectField (dragItem, typeof(Transform), true, GUILayout.ExpandWidth(false), GUILayout.Width(140f)) as Transform;
		if (dragItem != null) {
			if (dragItem != UIPrefabRoot) {
				if (PrefabInstanceCheck (dragItem)) {
					dragItem = PrefabUtility.FindPrefabRoot (dragItem.gameObject).transform;
					if (UIPrefabRoot != dragItem) {
						UIPrefabRoot = dragItem;
						this.RemoveNotification ();
						LoadUIViewCache ();
					} else {
						this.ShowNotification (new GUIContent ("这是同一个UIPrefab"));
					}
				} else {
					this.ShowNotification (new GUIContent ("这不是一个PrefabInstance"));
					CleanUp ();
				}
			}
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical();

		EditorGUILayout.BeginHorizontal ();
		EditorHelper.DrawBoxLabel("C#\nGenertePath:", _codeGeneratePath == "" ? "NONE" : _codeGeneratePath, false);
		if (GUILayout.Button ("Clean\nC#Path", GUILayout.Width (60f), GUILayout.Height(45f))) {
			_codeGeneratePath = null;
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorHelper.DrawBoxLabel("Lua\nGenertePath:", _codeLuaGeneratePath == "" ? "NONE" : _codeLuaGeneratePath, false);
		if (GUILayout.Button ("Clean\nLuaPath", GUILayout.Width (60f), GUILayout.Height(45f))) {
			_codeLuaGeneratePath = null;
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.EndVertical();

		EditorGUILayout.EndHorizontal ();
	}

	private bool PrefabInstanceCheck (Object target)
	{ 
		PrefabType type = PrefabUtility.GetPrefabType (target);
		
		if (type == PrefabType.PrefabInstance) {
			return true;
		}
		return false;
	}

	void DrawSelectionComponentView ()
	{
		EditorGUILayout.BeginVertical (GUILayout.MinWidth (150f));
		if (EditorHelper.DrawHeader ("Node Components")) {
			selectionViewPos = EditorGUILayout.BeginScrollView (selectionViewPos, "TextField");

			//根节点只显示除GameObject、Transform以外的组件
			int validateCode = ValidateSelection (Selection.activeTransform);
			if (validateCode != ERROR_NODE) {

				if (validateCode != ROOT_NODE && GUILayout.Button ("---GameObject---")) {
					UIComponentInfo newInfo = new UIComponentInfo (Selection.activeGameObject, UIPrefabRoot);
					if (_exportInfoDic.ContainsKey (newInfo.uid))
						_curComInfo = _exportInfoDic [newInfo.uid];
					else
						_curComInfo = newInfo;
					
					EditorGUIUtility.editingTextField = false;
				}
				GUILayout.Space (3f);

				GUILayout.Label ("ComponentList");
				Component [] comList = Selection.activeTransform.GetComponents<Component> ();
				foreach (Component com in comList) {

					if (validateCode == ROOT_NODE && com.GetType ().Name == "Transform")
						continue;

					if (GUILayout.Button (com.GetType ().Name)) {
						UIComponentInfo newInfo = new UIComponentInfo (com, UIPrefabRoot);
						if (_exportInfoDic.ContainsKey (newInfo.uid))
							_curComInfo = _exportInfoDic [newInfo.uid];
						else
							_curComInfo = newInfo;
	
						EditorGUIUtility.editingTextField = false;
					}
					GUILayout.Space (3f);
				}
			} else
				EditorGUILayout.HelpBox ("This is not a node of UIPrefab", MessageType.Info);
			EditorGUILayout.EndScrollView ();
		}
		
		EditorGUILayout.EndVertical ();
	}

	void DrawComponentInfoView ()
	{
		EditorHelper.DrawHeader ("ComponentInfo");
		if (_curComInfo != null) {
			EditorGUIUtility.labelWidth = 100f;
			EditorHelper.DrawBoxLabel ("UID: ", _curComInfo.uid, true);
			EditorHelper.DrawBoxLabel ("Type: ", _curComInfo.comType, false);

			string memberName = EditorGUILayout.TextField ("MemberName: ", _curComInfo.memberName, GUILayout.Height (20));
			if (memberName != _curComInfo.memberName) {
				_curComInfo.memberName = memberName;
				exportDicChanged = true;
			}
			EditorHelper.DrawBoxLabel ("Path: ", (_curComInfo.path == "" ? "This is a root node" : _curComInfo.path), true);

			GUILayout.BeginHorizontal (GUILayout.MinHeight (20f));
			if (!_exportInfoDic.ContainsKey (_curComInfo.uid)) {
				if (GUILayout.Button ("Add", GUILayout.Width (60f))) {
					_exportInfoDic.Add (_curComInfo.uid, _curComInfo);
					EditorGUIUtility.editingTextField = false;
					exportDicChanged = true;
				}
			}

			EditorGUILayout.EndHorizontal ();
		}
	}
	
	void DrawExportListView ()
	{
		EditorHelper.DrawHeader ("ExportList");
		GUILayout.BeginHorizontal ();
		GUILayout.Space (3f);
		GUILayout.BeginVertical ();
		exportViewPos = EditorGUILayout.BeginScrollView (exportViewPos);
		bool delete = false;
		int index = 0;
		foreach (UIComponentInfo item in _exportInfoDic.Values) {
			++index;
			GUILayout.Space (-1f);
			bool highlight = (_curComInfo != null) && (item.uid == _curComInfo.uid);
			if(_validatedInfoDic.ContainsKey(item.uid) && !_validatedInfoDic[item.uid]){
				GUI.backgroundColor = Color.red;
			}else
				GUI.backgroundColor = highlight ? Color.white : new Color (0.8f, 0.8f, 0.8f);
			GUILayout.BeginHorizontal ("AS TextArea", GUILayout.MinHeight (20f));

			GUI.backgroundColor = Color.white;
			GUILayout.Label (index.ToString (), GUILayout.Width (20f));

			if (GUILayout.Button (item.GetName (), "OL TextField", GUILayout.Height (20f))) {
				_curComInfo = item;
				Selection.activeTransform = UIPrefabRoot.Find (_curComInfo.path);
			}

			if (_deleteUIDList.Contains (item.uid)) {
				GUI.backgroundColor = Color.red;
			
				if (GUILayout.Button ("Delete", GUILayout.Width (60f))) {
					delete = true;
				}
				GUI.backgroundColor = Color.green;
				if (GUILayout.Button ("X", GUILayout.Width (22f))) {
					_deleteUIDList.Remove (item.uid);
					delete = false;
				}
				GUI.backgroundColor = Color.white;
			} else {
				// If we have not yet selected a sprite for deletion, show a small "X" button
				if (GUILayout.Button ("X", GUILayout.Width (22f)))
					_deleteUIDList.Add (item.uid);
			}
			GUILayout.EndHorizontal ();
		}
		EditorGUILayout.EndScrollView ();
		GUILayout.EndVertical (); 
		GUILayout.Space (3f);
		GUILayout.EndHorizontal ();

		if (delete) {
			foreach (string uid in _deleteUIDList) {
				_exportInfoDic.Remove (uid);
				_validatedInfoDic.Remove (uid);
			}
			_deleteUIDList.Clear ();
			exportDicChanged = true;
		}

		EditorGUILayout.BeginHorizontal ();
//		if(GUILayout.Button("Save",GUILayout.Width(100f),GUILayout.Height(40f)))
//		{
//			SaveUIViewCache(false);
//		}

		//	C# 代码生成	###############################
		if (GUILayout.Button ("GenerateC#Code", GUILayout.Width (120f), GUILayout.Height (40f))) {
			if (exportDicChanged) {
				UIViewCache record = SaveUIViewCache ();
				GenerateCSharpCode (record);
				exportDicChanged = false;
			}
			else
				this.ShowNotification(new GUIContent("Failure 保存为C# 但是没有任何变更操作"));
		}

		//	Lua 代码生成	###############################
		GUI.color = Color.green;
		//GUI.backgroundColor = Color.green;
//		if (GUILayout.Button ("GenerateLuaCode", GUILayout.Width (120f), GUILayout.Height (40f))) {
//			if (exportDicChanged) {
//				UIViewCache record = SaveUIViewCache ();
//				GenerateLuaCode (record);
//				exportDicChanged = false;
//			}
//			else
//				this.ShowNotification(new GUIContent("Failure 保存为Lua 但是没有任何变更操作"));
//		}
		GUI.color = Color.white;

		EditorGUILayout.EndHorizontal ();
		GUILayout.Space (10f);
	}

	/// <summary>
	/// Validates the selection.
	/// -1:不是当前Prefab的节点
	/// 0：是当前Prefab的根节点
	/// 1：是当前Prefab的子节点
	/// </summary>
	private const int ERROR_NODE = -1;
	private const int ROOT_NODE = 0;
	private const int CHILD_NODE = 1;

	private int ValidateSelection (Transform selection)
	{
		if (selection == UIPrefabRoot)
			return 0;

		Transform trans = selection;
		while (trans != null) {
			if (trans == UIPrefabRoot)
				return 1;
			trans = trans.parent;
		}
		return -1;
	}

	private void CleanUp ()
	{
		//清空操作
		dragItem = null;
		UIPrefabRoot = null;
		OnChangeUIPrefab();
	}

	private void OnChangeUIPrefab()
	{
		_codeGeneratePath = "";
		_codeLuaGeneratePath = "";
		_curComInfo = null;
		exportDicChanged = true;
		_deleteUIDList.Clear ();
		_exportInfoDic.Clear ();
		_validatedInfoDic.Clear();
	}
	
	public void GenerateCSharpCode (UIViewCache source)
	{
		// Build the generator with the class name and data source.
		UIViewCodeTemplate generator = new UIViewCodeTemplate (source.name, source.componentInfoList);

		// Generate output (class definition).
		var classDefintion = generator.TransformText ();

		if(string.IsNullOrEmpty(_codeGeneratePath) && !Directory.Exists(_codeGeneratePath))
		{
			var outputPath = EditorUtility.SaveFilePanel("Save C# UIView Code",
			                                             VIEWCODE_GENERATED_PATH,
			                                             source.name,
			                                             "cs");

			outputPath = outputPath.Replace(Application.dataPath, "");

			_codeGeneratePath = outputPath;
			source.codePath = _codeGeneratePath;
		}

		if(string.IsNullOrEmpty(_codeGeneratePath))
			return;

		try {
			// Save new class to assets folder.
			File.WriteAllText (Application.dataPath + _codeGeneratePath, classDefintion);

			this.ShowNotification(new GUIContent("Success 生成C#代码完毕"));
			// Refresh assets.
//			AssetDatabase.Refresh ();
		} catch (System.Exception e) {
			Debug.Log ("An error occurred while saving file: " + e);
		}
	}

	#region View 保存方法 Begin
	public void GenerateLuaCode(UIViewCache source) {
		UIViewCodeTemplateToLua generatorToLua = new UIViewCodeTemplateToLua(source.name, source.componentInfoList);

		var classDefintionToLua = generatorToLua.TransformText();
		if(string.IsNullOrEmpty(_codeLuaGeneratePath) && !Directory.Exists(_codeLuaGeneratePath)) {
			//var outputPath = EditorUtility.SaveFilePanel("Save Lua UIView Code",
			//                                             STATIONARY_GENERATEDLUA_PATH,
			//                                             source.name,
			//                                             "lua");
			//GameDebuger.OrangeDebugLog(outputPath.ToString());

			//outputPath = outputPath.Replace(Application.dataPath, "");
			var outputPath = string.Format("{0}{1}.lua", STATIONARY_GENERATEDLUA_PATH_DEFAULT, source.name);
			
			_codeLuaGeneratePath = outputPath;
			source.codeLuaPath = _codeLuaGeneratePath;
		}
		
		if(string.IsNullOrEmpty(_codeLuaGeneratePath))
			return;
		
		try {
			// Save new class to assets folder.
			File.WriteAllText (Application.dataPath + _codeLuaGeneratePath, classDefintionToLua);
			this.ShowNotification(new GUIContent("Success 生成Lua代码完毕"));
		} catch (System.Exception e) {
			Debug.Log ("An error occurred while saving file: " + e);
		}
	}
	#endregion
}
